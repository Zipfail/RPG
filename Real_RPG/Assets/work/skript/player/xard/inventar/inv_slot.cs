using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class inv_slot : MonoBehaviour
{
    private inv_item item;
    private int _call;
    [SerializeField] private Transform vzaim;
    [SerializeField] private Transform spavn;
    public void _add_item(inv_item items)
    {
        if(item != null)
        {
            if (item._name == items._name)
            {
                _call++;
                
            }
        }
        else
        {
            item = Instantiate(items,new Vector3(0,-100,0), transform.rotation);
            //item.GetComponent<Renderer>().enabled = false;
            _call = 1;
            
        }
        
    }
    private void Update()
    {
        if (item != null)
        {
            transform.GetChild(0).GetComponent<Image>().sprite = item._icon;
            transform.GetChild(1).GetComponent<TMP_Text>().text = _call.ToString();
            transform.GetChild(2).GetComponent<TMP_Text>().text = item._name;
            if(_call <= 0)
            {
                Destroy(item.transform.gameObject);
            }
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().sprite = null;
            transform.GetChild(1).GetComponent<TMP_Text>().text = "";
            transform.GetChild(2).GetComponent<TMP_Text>().text = "";

        }
    }
    public bool _zanito(string name)
    {
        if(item == null || (item._name == name && item.max_call > _call))
        {
            return false;
        }
        return true;
    }
    public void go_to_vzaim()
    {
        vzaim.position = transform.position;

        vzaim.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        vzaim.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();

        if (item != null) { 
            vzaim.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(used_item);
            vzaim.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(_drop);
        } 

    }
    public void _drop()
    {
        if(item != null)
        {
            print("drop");
            _call--;
            Instantiate(item, spavn.position, spavn.rotation);
            //item.GetComponent<Renderer>().enabled = true;

        }
    }
    public void used_item()
    {
        if(item != null)
        {
            item._used();
            _call--;
        }
        else
        {
            vzaim.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
            vzaim.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
        }
        
    }
}
