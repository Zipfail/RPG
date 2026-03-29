using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

abstract public class kv_kvest : MonoBehaviour
{
    [SerializeField] public bool konec;
    protected bool nagrada_bool = false;
    [SerializeField] private List<kv_nagrada> nagrada;
    public void _endcvest()
    {
        if(konec)
        {
            nagrada_bool = true;
            foreach (kv_nagrada i in nagrada) {
                i.nagrada();
            }
        }
        
    }
    abstract public void _end(); 
}
