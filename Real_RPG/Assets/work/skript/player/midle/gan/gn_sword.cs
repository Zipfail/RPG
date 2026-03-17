using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class gn_sword : gn_baz_mele
{
    private bool kostil;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attac();
    }
    protected override void attac()
    {
        if (razr_attac)
        {
            if (Input.GetMouseButtonDown(0) && anim.GetCurrentAnimatorClipInfo(0).Count() == 0)
            {
                kostil = true;
                switch (count)
                {
                    case 0:
                        anim.SetTrigger("atac0");
                        break;
                    case 1:
                        anim.SetTrigger("atac1");
                        break;
                    case 2:
                        anim.SetTrigger("atac2");
                        break;
                }
                count++;
                timer = 0;
                if (count > 2)
                {
                    count = 0;
                }
            }
            if (count != 0)
            {
                if (timer >= time_atac)
                {
                    count = 0;
                }
                else
                {
                    timer += Time.fixedDeltaTime;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<vr_telo>() && razr_attac && kostil)
        {
            other.GetComponent<vr_telo>().damage(damage);
            kostil = false;
        }
    }
}
