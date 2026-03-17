using UnityEngine;

public class vr_telo : MonoBehaviour
{
    [SerializeField] private vr_baza vrag;
    [SerializeField] private float damag;
    [SerializeField]private float xp;
    private bool mesto = true;
    private void Update()
    {
        if(xp <= 0)
        {
            death();
        }
    }

    public void damage(float x)
    {
        xp -= x;
    }
    private void death()
    {
        if(mesto)vrag._damag(damag);
        mesto = false;
        Destroy(gameObject);
    }
}
