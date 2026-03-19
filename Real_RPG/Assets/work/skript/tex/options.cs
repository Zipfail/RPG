using UnityEngine;

public class options : MonoBehaviour
{
    [SerializeField] private float volume_sound;
    [SerializeField] private float okrug;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float _sound(int i)
    {
        switch (i)
        {
            case 1:
                return volume_sound;
            case 0:
                return okrug;
        }
        return 0;
    }
}
