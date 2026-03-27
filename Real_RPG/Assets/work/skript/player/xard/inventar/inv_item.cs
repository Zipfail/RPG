using UnityEngine;

abstract public class inv_item : MonoBehaviour
{
    public string _name;
    public Sprite _icon;
    public int max_call;
    abstract public void _used();
}
