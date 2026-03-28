using UnityEngine;
using UnityEngine.UI;

public class UI_zapispa : MonoBehaviour
{
    private UI_baza _UI => GameObject.FindGameObjectWithTag("UI").GetComponent<UI_baza>();
    [SerializeField] private Sprite image;

    public void _used()
    {
        _UI.smen_window(4);
        _UI.transform.GetChild(0).GetChild(4).GetChild(1).GetComponent<Image>().sprite = image;
    }
}
