using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPausa : MonoBehaviour
{


    [Header("Controles")]
    public static ControlPausa InstanciaControl { get; private set; }
    private GameObject controlEscena;
    private ControlAR_old controlAR_old;
    private ControlAR controlAR;
    private ControlOpenXR controlOpenXR;
    //private ControlCardBoard controlCardboard;

    // Banderas
    private bool menuCargado = false;
    private bool arEstabaActivo = false;
    private bool vrEstabaActivo = false;

    private AudioSource musica;
    private bool musicaOn = false;
    private float tiempoEscena;

    private void Awake()
    {
        if (InstanciaControl != null && InstanciaControl != this)
        {
            Destroy(gameObject);
            return;
        }
        InstanciaControl = this;
    }

    private void Start()
    {
        // Buscar automáticamente el ControlAR en la escena si existe
        if(FindAnyObjectByType<ControlAR_old>() != null)
        {
            Debug.Log("ControlAR_old encontrado en la escena.");
            controlAR_old = FindAnyObjectByType<ControlAR_old>();

        }
        else if (FindAnyObjectByType<ControlAR>() != null)
        {
            Debug.Log("ControlAR encontrado en la escena.");
            controlAR = FindAnyObjectByType<ControlAR>();
        }

        // Buscar automáticamente el ControlOpenXR en la escena si existe
        if (FindAnyObjectByType<ControlOpenXR>() != null)
        {
            Debug.Log("ControlOpenXR encontrado en la escena.");
            controlOpenXR = FindFirstObjectByType<ControlOpenXR>();
        }

        //if (FindAnyObjectByType<ControlCardBoard>() != null)
        //{
        //    Debug.Log("ControlCardBoard encontrado en la escena.");
        //    //controlCardboard = FindFirstObjectByType<ControlCardBoard>();

        //}

        controlEscena = GameObject.FindGameObjectWithTag("ControlEscena");
        if (controlEscena != null)
        {
            musica = controlEscena.GetComponent<AudioSource>();
            Debug.Log("AudioSource obtenido del control de escena.");
        }
        else
        {
            Debug.Log("Control de la escena no encontrado.");
        }


    }

    // Pausar el juego y cargar el menú de pausa
    public void PausarJuego()
    {

        // Si el menú ya está cargado, no hacemos nada
        if (!menuCargado)
        {


            if (musica != null && musica.isPlaying) {
                musicaOn = true;
                musica.Pause();
            }

            tiempoEscena = Time.timeScale;
            if (tiempoEscena != 0f) Time.timeScale = 0f;

            Debug.Log("Cargando MenuPausa...");
            // Pausar tiempo y cargar la escena del menú de pausa
            SceneManager.LoadScene("MenuPausa", LoadSceneMode.Additive);
            // Si hay AR o VR activo, lo pausamos
            PausarAR();
            PausarVR();
            menuCargado = true;
            StartCoroutine(AsignarCamaraAlCanvas());

        }

    }

    // Reanudar el juego y descargar el menú de pausa
    public void ReanudarJuego()
    {

        if (musica != null && musicaOn == true) musica.Play();

        // Reanudar el tiempo y descargar la escena del menú de pausa
        Time.timeScale = tiempoEscena;
        SceneManager.UnloadSceneAsync("MenuPausa");
        menuCargado = false;

        // Reanudar AR o VR si estaban activos antes de pausar
        ReanudarAR();
        ReanudarVR();

        Debug.Log("[ControlPausa] Pausa desactivada.");

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
        if (arEstabaActivo && (controlAR_old != null || controlAR != null) && controlAR.isActiveAndEnabled)
        {
            if (controlAR != null && controlAR.isActiveAndEnabled) controlAR.ActivarAR();
            else if (controlAR_old != null && controlAR_old.isActiveAndEnabled) controlAR_old.ActivarAR();
            arEstabaActivo = false;
            Debug.Log("[ControlPausa] AR reanudado tras pausa.");
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

        if (vrEstabaActivo && controlOpenXR != null && controlOpenXR.isActiveAndEnabled)
        {
            controlOpenXR.ActivarOpenXR();
            vrEstabaActivo = false;
            Debug.Log("[ControlPausa] VR reanudado tras pausa.");
        }
    }

    // Desactivar AR con flag si estaba activo al pausar
    private void PausarAR()
    {
        if (controlAR != null && controlOpenXR.isActiveAndEnabled)
        {
            controlAR.DesactivarAR();
            arEstabaActivo = true;
        }
        else if (controlAR_old != null && controlOpenXR.isActiveAndEnabled) 
        {
            controlAR_old.DesactivarAR();
            arEstabaActivo = true;
        }
        if (arEstabaActivo) Debug.Log("[ControlPausa] AR pausado por el sistema de pausa.");
        else Debug.Log("[ControlPausa] AR no estaba activo al pausar.");

    }

    // Desactivar VR con flag si estaba activo al pausar
    private void PausarVR()
    {
        //if (controlCardboard != null)
        //{
        //    controlCardboard.DesactivarCardBoard();
        //    vrEstabaActivo = true;
        //}

        if (controlOpenXR != null && controlOpenXR.isActiveAndEnabled)
        {
            controlOpenXR.DesactivarOpenXR();
            vrEstabaActivo = true;
        }
        if (vrEstabaActivo) Debug.Log("[ControlPausa] VR pausado por el sistema de pausa.");
        else Debug.Log("[ControlPausa] VR no estaba activo al pausar.");
    }

    // Desactivar AR reiniciar o volver al menú principal
    private void DesactivarAR()
    {
        if (controlAR != null && controlAR.isActiveAndEnabled) controlAR.DesactivarAR();
        else if (controlAR_old != null && controlAR_old.isActiveAndEnabled) controlAR_old.DesactivarAR();

        arEstabaActivo = false;
    }

    // Desactivar VR reiniciar o volver al menú principal
    private void DesactivarVR()
    {
        //if (controlCardboard != null) controlCardboard.DesactivarCardBoard();
        if (controlOpenXR != null && controlOpenXR.isActiveAndEnabled) controlOpenXR.DesactivarOpenXR();

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
            Debug.LogError("[ControlPausa] La escena del menú de pausa no está cargada.");
            yield break;
        }

        // Recorre los objetos raíz de la escena del menú de pausa
        foreach (GameObject rootObj in escenaPausa.GetRootGameObjects())
        {
            // Busca el canvas en los objetos raíz
            Canvas canvas = rootObj.GetComponentInChildren<Canvas>(true);

            // Asigna la cámara principal al canvas si es ScreenSpaceCamera y el canvas no es null
            if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                if (Camera.main == null)
                {
                    Debug.LogError("[ControlPausa] No se ha encontrado una cámara principal en la escena. Creando una temporal.");
                    // Si no hay cámara principal, crea una temporal
                    GameObject tempCamera = new GameObject("MainCamera");
                    Camera camara = tempCamera.AddComponent<Camera>();
                    camara.tag = "MainCamera";
                    yield break;
                }
                // Asigna la cámara principal al canvas
                canvas.worldCamera = Camera.main;
                Debug.Log("[ControlPausa] Cámara principal asignada al canvas del menú de pausa.");
            }
        }
    }
}