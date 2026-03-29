using System.Collections.Generic;
using UnityEngine;

public class kv_kvest_kill_entiti : kv_kvest
{
    [SerializeField] private List<vr_baza> vrags;
    public override void _end()
    {
        if (!nagrada_bool)
        {
            konec = proverka();
        }
        if (nagrada_bool && konec)
        {
            konec = false;
        }

    }
    private void Update()
    {
        _end();
    }
    private bool proverka()
    {
        foreach (vr_baza i in vrags)
        {
            if(i != null)
            {
                return false;
            }
        }
        return true;
    }
}
