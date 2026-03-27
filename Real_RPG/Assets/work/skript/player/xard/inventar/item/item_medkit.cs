using System.Runtime.CompilerServices;
using UnityEngine;

public class item_medkit : inv_item
{
    private pl_player player => GameObject.FindGameObjectWithTag("player").GetComponent<pl_player>();
    [SerializeField] private float xp_reg;
    public override void _used()
    {
        player._regen(xp_reg);
    }
}
