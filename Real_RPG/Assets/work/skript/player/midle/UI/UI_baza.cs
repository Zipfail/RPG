using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_baza : MonoBehaviour
{
    [SerializeField] private List<GameObject> window;
    [SerializeField] private pl_move player;

    [SerializeField] private TMP_Text[] _text;
    [SerializeField] private GameObject use;
    //[SerializeField] private GameObject xp_max;
    private void Start()
    {
        window_off();
    }
    private void listoff()
    {
        for (int i = 0; i < window.Count; i++)
        {
            window[i].SetActive(false);
        }
    }
    public void smen_window(int number)
    {
        listoff();
        window[number].SetActive(true);
        player._nf_cursor(true);
        player._nf_walk(false);
    }
    public void window_off()
    {
        listoff();
        window[0].SetActive(true);
        player._nf_cursor(false);
        player._nf_walk(true);
    }
    public void _used(bool tipe)
    {
        use.SetActive(tipe);
    }
    public void set_info_player(float[] sost)
    {
        for(int i =0;i < _text.Count();i++)
        {
            _text[i].text = sost[i].ToString();
        }
    }

    public void set_gun(float[] sost)
    {
        
    }
}
