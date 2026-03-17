using System.Runtime.CompilerServices;
using UnityEngine;

public class vr_baza : entiti
{
    private void Update()
    {
        base.Update();
    }
    protected override void death()
    {
        Destroy(gameObject);
    }
}
