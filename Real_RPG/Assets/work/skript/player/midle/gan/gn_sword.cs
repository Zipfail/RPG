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
        anim.SetBool("idel",razr_attac);
        if (razr_attac)
        {
            if (Input.GetMouseButtonDown(0) && anim.GetCurrentAnimatorClipInfo(0).Count() == 1)
            {
                switch (count)
                {
                    case 0:
                        anim.SetTrigger("atac0");
                        playSound(_sound[0]);
                        kostil = true;
                        break;
                    case 1:
                        anim.SetTrigger("atac1");
                        playSound(_sound[1]);
                        kostil = true;
                        break;
                    case 2:
                        anim.SetTrigger("atac2");
                        playSound(_sound[2]);
                        kostil = true;
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
            playSound(_sound[3]);
        }
        kostil = false;
    }
}
