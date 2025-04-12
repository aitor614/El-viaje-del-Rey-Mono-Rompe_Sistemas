using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPausa : MonoBehaviour
{
    public static ControlPausa InstanciaControl { get; private set; }

    private bool menuCargado = false;

    private ControlAR controlAR;
    private bool arEstabaActivo = false;

    private void Awake()
    {
        InstanciaControl = this;
    }

    private void Start()
    {
        // Buscar automáticamente el ControlAR en la escena si existe
        controlAR = FindFirstObjectByType<ControlAR>();
    }

    public void Pausar()
    {
        if (!menuCargado)
        {
            // Si hay AR activo, lo pausamos
            if (controlAR != null)
            {
                controlAR.DesactivarAR();
                arEstabaActivo = true;
                Debug.Log("AR pausado por el sistema de pausa.");
            }

            // Cargar la escena del menú de pausa
            Time.timeScale = 0f;
            SceneManager.LoadScene("MenuPausa", LoadSceneMode.Additive);
            menuCargado = true;


        }

        Debug.Log("Pausa activada.");
    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("MenuPausa");
        menuCargado = false;

        if (arEstabaActivo && controlAR != null)
        {
            controlAR.ActivarAR();
            arEstabaActivo = false;
            Debug.Log("AR reanudado tras pausa.");
        }
    }

    public void Reiniciar()
    {
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        Time.timeScale = 1f;

        if (controlAR != null)
        {
            controlAR.DesactivarAR();
            arEstabaActivo = false;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MenuPrincipal()
    {
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        Time.timeScale = 1f;

        if (controlAR != null)
        {
            controlAR.DesactivarAR();
            arEstabaActivo = false;
        }

        SceneManager.LoadScene("MenuPrincipal");
    }
}