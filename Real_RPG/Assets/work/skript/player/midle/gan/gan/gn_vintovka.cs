using UnityEngine;

public class gn_vintovka : gn_baza_dal
{
    void Start()
    {
        ammo = max_ammo;
    }

    // Update is called once per frame
    void Update()
    {

        if (razr_attac) {
            if (timer > time_atac)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    attac();
                    timer = 0;
                }
            }
            else
            {
                timer += Time.fixedDeltaTime;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                reload();
            }
        }
    }
}
