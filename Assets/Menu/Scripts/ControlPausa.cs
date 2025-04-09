using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPausa : MonoBehaviour
{
    public static ControlPausa InstanciaControl { get; private set; }

    private bool menuCargado = false;

    public void Pausar()
    {
        if (!menuCargado)
        {
            Time.timeScale = 0f;
            SceneManager.LoadScene("MenuPausa", LoadSceneMode.Additive);
            menuCargado = true;
        }
    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("MenuPausa");
        menuCargado = false;
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
