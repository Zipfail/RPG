using UnityEngine;

public class item_xolder : inv_item
{
    private pl_player _player => GameObject.FindGameObjectWithTag("player").GetComponent<pl_player>();
    public override void _used()
    {
        _player._damag(50);
    }
}
