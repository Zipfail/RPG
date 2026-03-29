using System.Collections.Generic;
using UnityEngine;

public class UI_human : MonoBehaviour
{
    [SerializeField] public List<UI_dialog> dialogs;
    public int count = 0;

    public void _smen()
    {
        dialogs[count]._start_diolog();
    }

    public void _smen_dialog(bool vipol)
    {
        if(vipol)
        {
            if(count < dialogs.Count-1)count++;
            _smen();
        }
        
    }
}
