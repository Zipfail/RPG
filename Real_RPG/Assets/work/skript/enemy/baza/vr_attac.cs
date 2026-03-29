using UnityEngine;

public class vr_attac : MonoBehaviour
{
    [SerializeField] private float damag; 
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<pl_player>())
        {
            other.GetComponent<pl_player>()._damag(damag);
        }
    }
}
