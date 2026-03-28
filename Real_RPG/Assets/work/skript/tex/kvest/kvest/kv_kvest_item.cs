using UnityEngine;

public class kv_kvest_item : kv_kvest
{
    private inv_baza _invent => GameObject.FindGameObjectWithTag("UI").transform.GetChild(0).GetChild(0).GetComponent<inv_baza>();
    [SerializeField] private string _name_item;
    public override void _end()
    {
        if (!nagrada_bool)
        {
            konec = _invent._have_item(_name_item);
        }
        if(nagrada_bool && konec)
        {
            _invent._remuv_slots(_invent._number_items(_name_item));
            konec = false;

        }
        
    }
    private void Update()
    {
        _end();
    }
}
