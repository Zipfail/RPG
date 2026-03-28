using System.Collections.Generic;
using UnityEngine;

public class inv_baza : MonoBehaviour
{
    [SerializeField] private Transform slot;
    [SerializeField] private List<inv_slot> _slots;
    void Awake()
    {
        for(int i = 0; i < slot.childCount;i++)
        {
            _slots[i] = slot.GetChild(i).GetComponent<inv_slot>();
        }
    }
    public bool _add_items(inv_item item)
    {
        foreach (inv_slot i in _slots)
        {
            if(!i._zanito(item._name))
            {
                i._add_item(item);
                return true;
            }
        }
        return false;
    }

    public bool _have_item(string name_items)
    {
        foreach(inv_slot i in _slots)
        {
            if(i.item != null)
            {
                if(i.item._name == name_items)
                {
                    return true;
                }
            }
            
        }
        return false;
    }
    public int _number_items(string name_items)
    {
        for(int i =0;i < _slots.Count; i++) { 
            if (_slots[i].item != null)
            {
                if (_slots[i].item._name == name_items)
                {
                    return i;
                }
            }
        }
        return 1488;
    }
    public void _remuv_slots(int i)
    {
        if(i != 1488)_slots[i]._delet_items();
    }
}
