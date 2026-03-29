using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_dialog : MonoBehaviour
{
    [SerializeField] private string _name_pers; // ﬂ —À»ÿ ŒÃ “”œŒ… ƒÀﬂ —Œ«ƒ¿Õ»ﬂ ÕŒ–Ã¿À‹Õ€’ ƒ»¿ÀŒ√Œ¬ “¿  ÿ“Œ “”“ ›“Œ ”≈¡»Ÿ≈  
    private UI_baza _UI => GameObject.FindGameObjectWithTag("UI").GetComponent<UI_baza>();

    [SerializeField] private string dialog;
    [SerializeField] private List<string> name_button;
    [SerializeField] private kv_kvest kvest;
    [SerializeField] private UI_human human;

    public void viborp()
    {
        if (kvest != null)
        {
            kvest._endcvest();
            human._smen_dialog( kvest.konec);
        }
        else
        {
            human._smen_dialog( true);
        }
    }
    public void _start_diolog()
    {
        _UI.smen_window(3);
        _UI.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = _name_pers;//ËÏˇ
        _UI.transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<TMP_Text>().text = dialog;//‰Ë‡ÎÓ„

        _UI.transform.GetChild(0).GetChild(2).GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = name_button[0];//ÍÌÓÔÍ‡
        _UI.transform.GetChild(0).GetChild(2).GetChild(4).GetChild(0).GetComponent<TMP_Text>().text = name_button[1];//ÍÌÓÔÍ‡2

        _UI.transform.GetChild(0).GetChild(2).GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
        //_UI.transform.GetChild(0).GetChild(2).GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();

        if(human.count < human.dialogs.Count-1)_UI.transform.GetChild(0).GetChild(2).GetChild(3).GetComponent<Button>().onClick.AddListener(viborp);
        else _UI.transform.GetChild(0).GetChild(2).GetChild(3).GetComponent<Button>().onClick.AddListener(_UI.GetComponent<UI_baza>().window_off);
        //_UI.transform.GetChild(0).GetChild(2).GetChild(4).GetComponent<Button>().onClick.AddListener(viborm);
    }
}
