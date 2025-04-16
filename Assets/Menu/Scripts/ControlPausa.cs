using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPausa : MonoBehaviour
{
    public static ControlPausa InstanciaControl { get; private set; }

    private bool menuCargado = false;

    private ControlAR controlAR;
    private ControlCardBoard controlVR;
    private bool arEstabaActivo = false;
    private bool vrEstabaActivo = false;

    private void Awake()
    {
        InstanciaControl = this;
    }

    private void Start()
    {
        // Buscar automáticamente el ControlAR en la escena si existe
        controlAR = FindFirstObjectByType<ControlAR>();
        // Buscar automáticamente el ControlVR en la escena si existe
        controlVR = FindFirstObjectByType<ControlCardBoard>();
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

            if (controlVR != null)
            {
                controlVR.DesactivarCardBoard();
                vrEstabaActivo = true;
                Debug.Log("VR pausado por el sistema de pausa.");
            }

            // Cargar la escena del menú de pausa
            Time.timeScale = 0f;
            SceneManager.LoadScene("MenuPausa", LoadSceneMode.Additive);
            menuCargado = true;


        }

        Debug.Log("Pausa activada.");
        if(PlayerPrefs.GetString("EscenaActual") == "JuegoAREspiritusDesencarnados" ||
              PlayerPrefs.GetString("EscenaActual") == "JuegoVRBatallaCelestial")
            StartCoroutine(AsignarCamaraAlCanvas());
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
        if (vrEstabaActivo && controlVR != null)
        {
            controlVR.ActivarCardBoard();
            vrEstabaActivo = false;
            Debug.Log("VR reanudado tras pausa.");
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

        if (controlVR != null)
        {
            controlVR.DesactivarCardBoard();
            vrEstabaActivo = false;
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

        if (controlVR != null)
        {
            controlVR.DesactivarCardBoard();
            vrEstabaActivo = false;
        }

        SceneManager.LoadScene("MenuPrincipal");
    }

    private IEnumerator AsignarCamaraAlCanvas()
    {
        // Espera 1 frame para asegurar que la escena está cargada
        yield return null;

        Scene escenaPausa = SceneManager.GetSceneByName("MenuPausa");

        foreach (GameObject rootObj in escenaPausa.GetRootGameObjects())
        {
            Canvas canvas = rootObj.GetComponentInChildren<Canvas>(true);
            if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                canvas.worldCamera = Camera.main;
                Debug.Log("Cámara principal asignada al canvas del menú de pausa.");
            }
        }
    }
}