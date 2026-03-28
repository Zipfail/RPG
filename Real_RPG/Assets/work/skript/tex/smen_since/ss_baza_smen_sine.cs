using UnityEngine;
using UnityEngine.SceneManagement;

public class ss_baza_smen_sine : MonoBehaviour
{
    [SerializeField]private Transform playe;
    [SerializeField] private UI_baza _UI;
    [SerializeField]private Vector3 mesto;
    private void Update()
    {
        playe.GetComponent<pl_move>()._nf_cursor(true);
        _UI.smen_window(5);
        playe.position = mesto;
        SceneManager.LoadScene(1);
    }
}
