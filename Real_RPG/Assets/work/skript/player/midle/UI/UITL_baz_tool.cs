using UnityEngine;
using UnityEngine.SceneManagement;

public class UITL_baz_tool : MonoBehaviour
{
    [SerializeField] private int num_lok;
    public void exit()
    {
        Application.Quit();
    }
    public void smne_lok()
    {
        SceneManager.LoadScene(num_lok);
    }
}
