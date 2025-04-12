using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPausa : MonoBehaviour
{
    public static ControlPausa InstanciaControl { get; private set; }

    private bool menuCargado = false;

    // Función para inicializar el script
    private void Awake()
    {
        // Inicializar el singleton (instancia de ControlPausa)
        InstanciaControl = this;
    }

    // Función para cargar el menú de pausa
    public void Pausar()
    {
        if (!menuCargado)
        {
            Time.timeScale = 0f;
            SceneManager.LoadScene("MenuPausa", LoadSceneMode.Additive);
            menuCargado = true;
        }
        Debug.Log("Pausa");
    }

    // Función para reanudar el juego pausado
    public void Reanudar()
    {
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("MenuPausa");
        menuCargado = false;
    }

    // Función para reiniciar el juego pausado
    public void Reiniciar()
    {
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Función para salir del juego pausado
    public void MenuPrincipal()
    {
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
