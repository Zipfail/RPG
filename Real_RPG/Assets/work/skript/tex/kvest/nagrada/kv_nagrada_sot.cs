using UnityEngine;

public class kv_nagrada_sot : kv_nagrada
{
    [SerializeField] private int tipe;
    [SerializeField] private int call;
    private pl_player _player => GameObject.FindGameObjectWithTag("player").GetComponent<pl_player>();
    public override void nagrada()
    {
        switch(tipe)
        {
            case 0:
                _player._add_ex(call);
                break;
            case 1:
                _player._add_money(call);
                break;
        }
    }
}
