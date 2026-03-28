using UnityEngine;

public class kv_nagrada_smen_bool : kv_nagrada
{
    [SerializeField] private bool sost;
    public override void nagrada()
    {
        sost = !sost;
    }

    public bool _sost()
    {
        return sost;
    }
}
