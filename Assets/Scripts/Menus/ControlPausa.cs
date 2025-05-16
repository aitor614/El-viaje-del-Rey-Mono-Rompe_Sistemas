using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPausa : MonoBehaviour
{

    private bool menuCargado = false;

    [Header("Controles")]
    public static ControlPausa InstanciaControl { get; private set; }
    private ControlAR_old controlAR_old;
    private ControlAR controlAR;
    //private ControlCardBoard controlCardboard;
    private ControlOpenXR controlOpenXR;


    // Banderas para saber si AR o VR estaban activos al pausar
    private bool arEstabaActivo = false;
    private bool vrEstabaActivo = false;

    private void Awake()
    {
        InstanciaControl = this;
    }

    private void Start()
    {
        // Buscar automáticamente el ControlAR en la escena si existe
        if(FindAnyObjectByType<ControlAR_old>() != null)
            controlAR_old = FindAnyObjectByType<ControlAR_old>();
        else if (FindAnyObjectByType<ControlAR>() != null)
            controlAR = FindAnyObjectByType<ControlAR>();

        // Buscar automáticamente el ControlVR en la escena si existe
        //controlCardboard = FindFirstObjectByType<ControlCardBoard>();
        if (FindAnyObjectByType<ControlOpenXR>() != null)
            controlOpenXR = FindFirstObjectByType<ControlOpenXR>();
    }

    // Pausar el juego y cargar el menú de pausa
    public void PausarJuego()
    {
        // Si el menú ya está cargado, no hacemos nada
        if (!menuCargado)
        {
            // Si hay AR o VR activo, lo pausamos
            PausarAR();
            PausarVR();

            // Pausar tiempo y cargar la escena del menú de pausa
            Time.timeScale = 0f;
            SceneManager.LoadScene("MenuPausa", LoadSceneMode.Additive);
            menuCargado = true;

        }

        Debug.Log("Pausa activada.");

        // Asignar la cámara principal al canvas del menú de pausa
        //if (PlayerPrefs.GetString("EscenaActual") == "JuegoAREspiritusDesencarnados" ||
        //      PlayerPrefs.GetString("EscenaActual") == "JuegoVRBatallaCelestial")
            StartCoroutine(AsignarCamaraAlCanvas());
    }

    // Reanudar el juego y descargar el menú de pausa
    public void ReanudarJuego()
    {
        // Reanudar el tiempo y descargar la escena del menú de pausa
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("MenuPausa");
        menuCargado = false;

        // Reanudar AR o VR si estaban activos antes de pausar
        ReanudarAR();
        ReanudarVR();

        Debug.Log("Pausa desactivada.");


    }

    // Reiniciar el juego
    public void ReiniciarJuego()
    {
        // Reiniciar el juego y cargar la escena actual
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        Time.timeScale = 1f;

        // Desactivar AR y VR si estaban activos para reiniciar correctamente
        DesactivarAR();
        DesactivarVR();

        // Reiniciar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Volver al menú principal
    public void MenuPrincipal()
    {
        // Reiniciar el juego y cargar el menú principal
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        Time.timeScale = 1f;

        // Desactivar AR y VR si estaban activos para volver al menú principal
        DesactivarAR();
        DesactivarVR();

        // Volver al menú principal
        SceneManager.LoadScene("MenuPrincipal");
    }

    // Reanudar AR si estaba activo antes de pausar
    private void ReanudarAR()
    {
        // Reanudar AR o VR si estaban activos antes de pausar
        if (arEstabaActivo && (controlAR_old != null || controlAR != null))
        {
            if (controlAR != null) controlAR.ActivarAR();
            else if (controlAR_old != null) controlAR_old.ActivarAR();
            arEstabaActivo = false;
            Debug.Log("AR reanudado tras pausa.");
        }
    }

    // Reanudar VR si estaba activo antes de pausar
    private void ReanudarVR()
    {
        //if (vrEstabaActivo && controlCardboard != null)
        //{
        //    controlCardboard.ActivarCardBoard();
        //    vrEstabaActivo = false;
        //    Debug.Log("VR reanudado tras pausa.");
        //}

        if (vrEstabaActivo && controlOpenXR != null)
        {
            controlOpenXR.ActivarAR();
            vrEstabaActivo = false;
            Debug.Log("VR reanudado tras pausa.");
        }
    }

    // Desactivar AR con flag si estaba activo al pausar
    private void PausarAR()
    {
        if (controlAR != null)
        {
            controlAR.DesactivarAR();
            arEstabaActivo = true;
        }
        else if (controlAR_old != null) 
        {
            controlAR_old.DesactivarAR();
            arEstabaActivo = true;
        }
        if (arEstabaActivo) Debug.Log("AR pausado por el sistema de pausa.");
        else Debug.Log("AR no estaba activo al pausar.");

    }

    // Desactivar VR con flag si estaba activo al pausar
    private void PausarVR()
    {
        //if (controlCardboard != null)
        //{
        //    controlCardboard.DesactivarCardBoard();
        //    vrEstabaActivo = true;
        //}

        if (controlOpenXR != null)
        {
            controlOpenXR.DesactivarAR();
            vrEstabaActivo = true;
        }
        if (vrEstabaActivo) Debug.Log("VR pausado por el sistema de pausa.");
        else Debug.Log("VR no estaba activo al pausar.");
    }

    // Desactivar AR reiniciar o volver al menú principal
    private void DesactivarAR()
    {
        if (controlAR != null) controlAR.DesactivarAR();
        else if (controlAR_old != null) controlAR_old.DesactivarAR();

        arEstabaActivo = false;
    }

    // Desactivar VR reiniciar o volver al menú principal
    private void DesactivarVR()
    {
        //if (controlCardboard != null) controlCardboard.DesactivarCardBoard();
        if (controlOpenXR != null) controlOpenXR.DesactivarAR();

        vrEstabaActivo = false;
    }

    // Asignar la cámara principal al canvas del menú de pausa
    private IEnumerator AsignarCamaraAlCanvas()
    {
        // Espera 1 frame para asegurar que la escena está cargada
        yield return null;

        // Asignar la cámara principal al canvas del menú de pausa
        Scene escenaPausa = SceneManager.GetSceneByName("MenuPausa");

        // Verifica si la escena del menú de pausa está cargada
        if (!escenaPausa.isLoaded)
        {
            Debug.LogError("La escena del menú de pausa no está cargada.");
            yield break;
        }

        // Recorre los objetos raíz de la escena del menú de pausa
        foreach (GameObject rootObj in escenaPausa.GetRootGameObjects())
        {
            // Busca el canvas en los objetos raíz
            Canvas canvas = rootObj.GetComponentInChildren<Canvas>(true);
            //if (canvas == null)
            //{
            //    Debug.LogError("No se encontró el canvas en la escena del menú de pausa.");
            //    yield break;
            //}
            // Asigna la cámara principal al canvas si es ScreenSpaceCamera y el canvas no es null
            if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                // Asigna la cámara principal al canvas
                canvas.worldCamera = Camera.main;
                Debug.Log("Cámara principal asignada al canvas del menú de pausa.");
            }
        }
    }
}