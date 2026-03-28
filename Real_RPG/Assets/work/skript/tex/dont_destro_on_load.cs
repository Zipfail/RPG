using UnityEngine;

public class dont_destro_on_load : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
