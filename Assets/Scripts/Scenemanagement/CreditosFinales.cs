using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditosFinales : MonoBehaviour
{
    public float tiempoHastaMenu = 15f;

    void Start()
    {
        Invoke(nameof(VolverAlMenu), tiempoHastaMenu);
    }

    void VolverAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}