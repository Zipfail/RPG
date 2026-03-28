using UnityEngine;
using UnityEngine.SceneManagement;

public class ss_smena_loki : MonoBehaviour
{
    private Transform playe => GameObject.FindGameObjectWithTag("player").transform;
    [SerializeField] private Vector3 mesto;
    [SerializeField] private int loka;
    public void smen()
    {
        playe.GetComponent<pl_move>()._nf_cursor(false);
        playe.position = mesto;
        SceneManager.LoadScene(loka);
    }
}
