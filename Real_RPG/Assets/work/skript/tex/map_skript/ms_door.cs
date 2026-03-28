using Unity.VisualScripting;
using UnityEngine;

public class ms_door : MonoBehaviour
{
    [SerializeField] private bool _sost_door;
    [SerializeField] private kv_nagrada_smen_bool nagrada;
    [SerializeField] private Animator anim;

    private void Update()
    {
        if(nagrada != null)
        {
            _sost_door = nagrada._sost();
        }
        anim.SetBool("open",_sost_door);
    }

    public void smen_door()
    {
        if (nagrada == null)
        {
            _sost_door = !_sost_door;
        }
        
    }

}
