using System.Collections.Generic;
using UnityEngine;

public class inv_baza : MonoBehaviour
{
    [SerializeField] private Transform slot;
    [SerializeField]private List<inv_slot> _slots;
    void Start()
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

}
