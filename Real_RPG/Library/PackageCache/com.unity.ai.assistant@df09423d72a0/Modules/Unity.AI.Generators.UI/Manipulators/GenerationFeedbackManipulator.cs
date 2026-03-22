using System;
using System.IO;
using Unity.AI.Generators.IO.Utilities;
using Unity.AI.Generators.Redux;
using Unity.AI.Generators.Redux.Thunks;
using Unity.AI.Generators.UI.Actions;
using Unity.AI.Generators.UI.Utilities;
using Unity.AI.Generators.UIElements.Extensions;
using Unity.AI.Toolkit.Asset;
using UnityEngine.UIElements;

namespace Unity.AI.Generators.UI
{
    /// <summary>
    /// A manipulator that adds generation feedback functionality (thumbs up/down) to a visual element.
    /// Handles hover visibility and feedback submission to the backend.
    /// </summary>
    class GenerationFeedbackManipulator : Manipulator
    {
        const string k_FeedbackActiveClass = "feedback-active";
        const string k_FeedbackSubmittedClass = "feedback-submitted";

        readonly string m_DialogType;
        readonly Func<Uri> m_GetGenerationUri;
        readonly Func<AssetReference> m_GetAsset;
        readonly Func<IStoreApi> m_GetStoreApi;
        readonly Func<GenerationFeedbackSentiment?> m_GetSubmittedSentiment;
        readonly Func<bool> m_CanShowFeedback;

        VisualElement m_FeedbackContainer;
        Button m_ThumbsUpButton;
        Button m_ThumbsDownButton;

        public GenerationFeedbackManipulator(
            string dialogType,
            Func<Uri> getGenerationUri,
            Func<AssetReference> getAsset,
            Func<IStoreApi> getStoreApi,
            Func<GenerationFeedbackSentiment?> getSubmittedSentiment,
            Func<bool> canShowFeedback)
        {
            m_DialogType = dialogType;
            m_GetGenerationUri = getGenerationUri;
            m_GetAsset = getAsset;
            m_GetStoreApi = getStoreApi;
            m_GetSubmittedSentiment = getSubmittedSentiment;
            m_CanShowFeedback = canShowFeedback;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            m_FeedbackContainer = target.Q<VisualElement>("feedback-container");
            m_ThumbsUpButton = target.Q<Button>("thumbs-up");
            m_ThumbsDownButton = target.Q<Button>("thumbs-down");

            target.AddStyleSheetBasedOnEditorSkin();

            if (m_ThumbsUpButton != null)
                m_ThumbsUpButton.clicked += OnThumbsUpClicked;
            if (m_ThumbsDownButton != null)
                m_ThumbsDownButton.clicked += OnThumbsDownClicked;

            target.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            target.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            if (m_ThumbsUpButton != null)
                m_ThumbsUpButton.clicked -= OnThumbsUpClicked;
            if (m_ThumbsDownButton != null)
                m_ThumbsDownButton.clicked -= OnThumbsDownClicked;

            target.UnregisterCallback<MouseEnterEvent>(OnMouseEnter);
            target.UnregisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }

        void OnMouseEnter(MouseEnterEvent evt) => UpdateFeedbackVisibility(true);

        void OnMouseLeave(MouseLeaveEvent evt) => UpdateFeedbackVisibility(false);

        void UpdateFeedbackVisibility(bool show)
        {
            if (m_FeedbackContainer == null)
                return;

            var canShow = show && m_CanShowFeedback();
            m_FeedbackContainer.SetShown(canShow);

            if (canShow)
                UpdateFeedbackButtonStates(null);
        }

        void UpdateFeedbackButtonStates(GenerationFeedbackSentiment? optimisticSentiment)
        {
            if (m_ThumbsUpButton == null || m_ThumbsDownButton == null)
                return;

            var sentiment = optimisticSentiment ?? m_GetSubmittedSentiment();
            var hasSubmitted = sentiment.HasValue;

            m_ThumbsUpButton.EnableInClassList(k_FeedbackActiveClass, sentiment == GenerationFeedbackSentiment.Positive);
            m_ThumbsDownButton.EnableInClassList(k_FeedbackActiveClass, sentiment == GenerationFeedbackSentiment.Negative);
            m_ThumbsUpButton.EnableInClassList(k_FeedbackSubmittedClass, hasSubmitted && sentiment != GenerationFeedbackSentiment.Positive);
            m_ThumbsDownButton.EnableInClassList(k_FeedbackSubmittedClass, hasSubmitted && sentiment != GenerationFeedbackSentiment.Negative);
        }

        void OnThumbsUpClicked() => SubmitFeedback(GenerationFeedbackSentiment.Positive);

        void OnThumbsDownClicked() => SubmitFeedback(GenerationFeedbackSentiment.Negative);

        void SubmitFeedback(GenerationFeedbackSentiment sentiment)
        {
            var generationUri = m_GetGenerationUri();
            var asset = m_GetAsset();
            var storeApi = m_GetStoreApi();

            if (generationUri == null || asset == null || storeApi == null)
                return;

            if (m_GetSubmittedSentiment() == sentiment)
                return;

            UpdateFeedbackButtonStates(sentiment);

            _ = storeApi.Dispatch(FeedbackActions.submitGenerationFeedback, new SubmitGenerationFeedbackPayload(
                asset,
                generationUri.AbsoluteUri,
                sentiment,
                m_DialogType,
                downloadedAssetId: Path.GetFileNameWithoutExtension(generationUri.GetLocalPath())));
        }
    }
}
