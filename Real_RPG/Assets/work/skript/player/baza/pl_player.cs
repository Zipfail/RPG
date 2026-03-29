using UnityEngine;

public class pl_player : entiti
{
    [SerializeField] private float yr;
    [SerializeField] private int money;
    private UI_baza UI => GameObject.FindGameObjectWithTag("UI").GetComponent<UI_baza>();
    private pl_move move => transform.GetComponent<pl_move>();
    private bool meni_bool = true;
    private bool inventar_bool = true;
    [SerializeField] private float _distance_hit;
    [SerializeField] private Transform camer;
    [SerializeField] private inv_baza inventory;
    private string _hit_tag;

    private bool _interact_e;
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        _interact_e = false;

        UI.set_info_player(new float[2] { xp, move._stamina });
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (meni_bool)
            {
                UI.smen_window(1);
                meni_bool = false;
            }
            else
            {
                UI.window_off();
                meni_bool = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventar_bool)
            {
                UI.smen_window(2);
                inventar_bool = false;
            }
            else
            {
                UI.window_off();
                inventar_bool = true;
            }
        }

        RaycastHit hit;
        if(Physics.Raycast(camer.position,camer.forward,out hit,_distance_hit))
        {
            _hit_tag = hit.transform.tag;
            switch (_hit_tag)
            {
                case "Weapon":
                    _interact_e=true;
                    break;
            }

            if (hit.transform.GetComponent<inv_item>())
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    inventory._add_items(hit.transform.GetComponent<inv_item>());
                    Destroy(hit.transform.gameObject);
                }
               _interact_e=true;
            }
            else if(hit.transform.GetComponent<UI_zapispa>())
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.GetComponent<UI_zapispa>()._used();
                }
                _interact_e = true;
            }
            else if (hit.transform.GetComponent<UI_human>())
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.GetComponent<UI_human>()._smen();
                }
                _interact_e = true;
            }

        }

        UI._used(_interact_e);




    }
    /*tstic unsafe void Main() using system
    {}
    */
    protected override void death()
    {
        UI.smen_window(6);
    }

    public void _add_ex(float yy)
    {
        yr += yy;
    }
    public void _add_money(int mone)
    {
        if (mone > 0)
        {
            money += mone;
        }
    }

}
