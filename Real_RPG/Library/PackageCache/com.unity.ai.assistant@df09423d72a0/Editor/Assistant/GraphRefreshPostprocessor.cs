using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.AI.Assistant.Editor.GraphGeneration;
using Unity.AI.Assistant.Utils;
using UnityEditor;
using UnityEngine;

namespace Unity.AI.Assistant.Editor
{
    /// <summary>
    /// AssetPostprocessor that detects asset changes and writes them to .pending_changes.json
    /// for incremental graph refresh by the backend.
    /// </summary>
    internal class GraphRefreshPostprocessor : AssetPostprocessor
    {
        private const string PENDING_CHANGES_FILE = ".pending_changes.json";
        static readonly object s_PendingChangesFileLock = new object();

        internal enum AssetChangeType
        {
            Imported,
            Deleted,
            Moved,
            DomainReload
        }

        internal class AssetChangeTypeJsonConverter : JsonConverter<AssetChangeType>
        {
            private const string ImportedStr = "imported";
            private const string DeletedStr = "deleted";
            private const string MovedStr = "moved";
            private const string DomainReloadStr = "domain_reload";

            public override void WriteJson(JsonWriter writer, AssetChangeType value, JsonSerializer serializer)
            {
                writer.WriteValue(ToJsonString(value));
            }

            public override AssetChangeType ReadJson(JsonReader reader, Type objectType, AssetChangeType existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var s = reader.Value?.ToString();
                return FromJsonString(s);
            }

            internal static string ToJsonString(AssetChangeType value)
            {
                return value switch
                {
                    AssetChangeType.Imported => ImportedStr,
                    AssetChangeType.Deleted => DeletedStr,
                    AssetChangeType.Moved => MovedStr,
                    AssetChangeType.DomainReload => DomainReloadStr,
                    _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
                };
            }

            internal static AssetChangeType FromJsonString(string value)
            {
                return value switch
                {
                    ImportedStr => AssetChangeType.Imported,
                    DeletedStr => AssetChangeType.Deleted,
                    MovedStr => AssetChangeType.Moved,
                    DomainReloadStr => AssetChangeType.DomainReload,
                    _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown AssetChangeType")
                };
            }
        }

        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths,
            bool didDomainReload)
        {
            try
            {
                // Find the graph root directory
                string graphRoot = FindGraphRoot();
                if (string.IsNullOrEmpty(graphRoot))
                {
                    // Graph doesn't exist yet, no need to track changes
                    return;
                }

                // Build change events
                var changes = new List<AssetChangeEvent>();

                // Track imported (new or modified) assets
                foreach (var assetPath in importedAssets)
                {
                    if (ShouldTrackAsset(assetPath))
                    {
                        changes.Add(new AssetChangeEvent
                        {
                            type = AssetChangeType.Imported,
                            path = assetPath,
                            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                        });
                    }
                }

                // Track deleted assets
                foreach (var assetPath in deletedAssets)
                {
                    if (ShouldTrackAsset(assetPath))
                    {
                        changes.Add(new AssetChangeEvent
                        {
                            type = AssetChangeType.Deleted,
                            path = assetPath,
                            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                        });
                    }
                }

                // Track moved/renamed assets
                for (int i = 0; i < movedAssets.Length; i++)
                {
                    string newPath = movedAssets[i];
                    string oldPath = movedFromAssetPaths[i];
                    if (ShouldTrackAsset(newPath))
                    {
                        changes.Add(new AssetChangeEvent
                        {
                            type = AssetChangeType.Moved,
                            path = newPath,
                            oldPath = oldPath,
                            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                        });
                    }
                    else if (ShouldTrackAsset(oldPath))
                    {
                        // New location is untracked (e.g. Packages/ or Library/) - emit Deleted for old path so backend removes it
                        changes.Add(new AssetChangeEvent
                        {
                            type = AssetChangeType.Deleted,
                            path = oldPath,
                            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                        });
                    }
                }

                // Track domain reload (compilation)
                if (didDomainReload)
                {
                    changes.Add(new AssetChangeEvent
                    {
                        type = AssetChangeType.DomainReload,
                        path = "",
                        timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                    });
                }

                // Write changes to file if any
                if (changes.Count > 0)
                {
                    WritePendingChanges(graphRoot, changes);
                }
            }
            catch (Exception ex)
            {
                InternalLog.LogError($"[GraphRefreshPostprocessor] Error tracking asset changes: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static string FindGraphRoot()
        {
            string graphPath = AssistantGraphGenerator.GraphRoot;
            return Directory.Exists(graphPath) ? graphPath : null;
        }

        internal static bool ShouldTrackAsset(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return false;

            // Skip meta files
            if (assetPath.EndsWith(".meta"))
                return false;

            // Skip files outside Assets folder
            if (!assetPath.StartsWith("Assets/"))
                return false;

            // Skip Unity's temporary files
            if (assetPath.Contains("/Temp/") || assetPath.Contains("\\Temp\\"))
                return false;

            // Track all asset types - scripts, prefabs, scenes, materials, textures, etc.
            return true;
        }

        private static void WritePendingChanges(string graphRoot, List<AssetChangeEvent> newChanges)
        {
            var newChangesCopy = new List<AssetChangeEvent>(newChanges);
            Task.Run(() =>
            {
                lock (s_PendingChangesFileLock)
                {
                    try
                    {
                        string pendingFile = Path.Combine(graphRoot, PENDING_CHANGES_FILE);

                        List<AssetChangeEvent> allChanges = new List<AssetChangeEvent>();
                        if (File.Exists(pendingFile))
                        {
                            try
                            {
                                string existingJson = File.ReadAllText(pendingFile);
                                var existingData = JsonConvert.DeserializeObject<PendingChangesFile>(existingJson);
                                if (existingData != null && existingData.changes != null)
                                {
                                    allChanges.AddRange(existingData.changes);
                                }
                            }
                            catch (Exception ex)
                            {
                                InternalLog.LogWarning($"[GraphRefreshPostprocessor] Could not read existing pending changes: {ex.Message}");
                            }
                        }

                        allChanges.AddRange(newChangesCopy);
                        var deduped = DeduplicateChanges(allChanges);

                        var changesFile = new PendingChangesFile
                        {
                            version = "1.0",
                            lastUpdate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                            totalChanges = deduped.Count,
                            changes = deduped
                        };

                        string json = JsonConvert.SerializeObject(changesFile, Formatting.Indented);
                        File.WriteAllText(pendingFile, json);

                        InternalLog.Log($"[GraphRefreshPostprocessor] Tracked {newChangesCopy.Count} asset changes (total pending: {deduped.Count})");
                    }
                    catch (Exception ex)
                    {
                        InternalLog.LogError($"[GraphRefreshPostprocessor] Failed to write pending changes: {ex.Message}\n{ex.StackTrace}");
                    }
                }
            });
        }

        internal static List<AssetChangeEvent> DeduplicateChanges(List<AssetChangeEvent> changes)
        {
            // Keep track of the latest event for each path
            var pathToEvent = new Dictionary<string, AssetChangeEvent>();

            foreach (var change in changes)
            {
                string key = change.path ?? "";

                // Special handling for different event types
                if (change.type == AssetChangeType.Deleted)
                {
                    // If current event at key is Moved with oldPath, inject Deleted(oldPath) so backend removes the original asset
                    if (pathToEvent.TryGetValue(key, out var current) && current.type == AssetChangeType.Moved && !string.IsNullOrEmpty(current.oldPath))
                    {
                        pathToEvent[current.oldPath] = new AssetChangeEvent
                        {
                            type = AssetChangeType.Deleted,
                            path = current.oldPath,
                            timestamp = change.timestamp
                        };
                    }
                    pathToEvent[key] = change;
                }
                else if (change.type == AssetChangeType.Moved)
                {
                    if (!string.IsNullOrEmpty(change.oldPath))
                    {
                        string originalOldPath = change.oldPath;
                        if (pathToEvent.TryGetValue(originalOldPath, out var oldEvent)
                            && oldEvent.type == AssetChangeType.Moved && !string.IsNullOrEmpty(oldEvent.oldPath))
                        {
                            // Coalesce chained moves: A→B, B→C => A→C
                            change.oldPath = oldEvent.oldPath;
                        }
                        pathToEvent.Remove(originalOldPath);
                    }
                    pathToEvent[key] = change;
                }
                else if (change.type == AssetChangeType.DomainReload)
                {
                    // Keep domain reload events separate (empty path)
                    pathToEvent[key] = change;
                }
                else if (change.type == AssetChangeType.Imported)
                {
                    // Imported overwrites any existing event (including Deleted) so recreated files are seen by the backend
                    pathToEvent[key] = change;
                }
            }

            return pathToEvent.Values.ToList();
        }

        [Serializable]
        private class PendingChangesFile
        {
            public string version;
            public string lastUpdate;
            public int totalChanges;
            public List<AssetChangeEvent> changes;
        }

        [Serializable]
        internal class AssetChangeEvent
        {
            [JsonConverter(typeof(AssetChangeTypeJsonConverter))]
            public AssetChangeType type;
            public string path;
            public string oldPath;     // For "moved" events
            public string timestamp;
        }
    }
}
