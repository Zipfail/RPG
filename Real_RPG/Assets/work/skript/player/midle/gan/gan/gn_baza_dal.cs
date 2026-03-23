using UnityEngine;
using static UnityEngine.UI.Image;

abstract public class gn_baza_dal : gn_baz_mele
{
    [SerializeField]protected int max_ammo;
    [SerializeField]protected int call_caset;
    protected int ammo;
    [SerializeField] protected float timer_rerol;
    protected float rerol;

    [SerializeField] private float distance;
    [SerializeField] private Transform camer;

    protected override void attac()
    {
        if(ammo > 0)
        {
            RaycastHit hit; 
            if (Physics.Raycast(camer.position, camer.forward, out hit, distance))
            {
                if (hit.collider.transform.GetComponent<vr_telo>())
                {
                    hit.collider.transform.GetComponent<vr_telo>().damage(damage);
                    print("zz");
                }
                print(hit.collider.transform);

            }

            playSound(_sound[0]);
            anim.SetTrigger("attac");
            ammo--;
        }

    }
    protected void reload()
    {
        if(call_caset > 0)
        {
            ammo = max_ammo;
            call_caset--;
            playSound(_sound[1]);
            anim.SetTrigger("reload");
        }
    }

    public void add_caset(int call)
    {
        call_caset += call;
    }

}
