using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlEspiritus : MonoBehaviour
{
    [Header("Controles")]
    public ControlAR controlAR;
    public ControlAR_old controlAR_old;
    public SpawnerEspiritus spawnerEspiritus;
    public ControlMenuPrincipal controlMenuPrincipal;
    public ControlHud controlHud;

    [Header("Elementos de la escena")]
    public GameObject canvasBotonPlay;

    [Header("Parámetros")]
    public int tiempoDeEspera;
    public int puntuacionVictoria;
    public int espiritusObjetivo;
    public float tiempoRestante;

    [Header("Sonidos")]
    public AudioClip musica;
    public AudioSource audioSource;

    [Header("XR")]
    private GestorXR gestorXR;
    public GestorXR gestorXR_pruebas;

    // Variables
    private int espiritus;
    private int puntuacion;
    private bool arActivado = false;
    private bool objetoPartida = false;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        controlMenuPrincipal = ControlMenuPrincipal.InstanciaControl;
        controlHud = ControlHud.InstanciaControl;
        // Obtener el gestor de XR
        if (controlMenuPrincipal == null || controlMenuPrincipal.gestorXR == null)
        {
            Debug.Log("[ControlBatalla] GestorXR no encontrado. Buscando en la escena...");
            gestorXR = gestorXR_pruebas;
            if (gestorXR == null)
            {
                Debug.LogError("[ControlBatalla] GestorXR sigue sin encontrarse.");
                return;
            }
            else gestorXR.enabled = true;
        }
        else
        {
            gestorXR = controlMenuPrincipal.gestorXR;
        }

        //Activar plugin AR si no está activado
        if (!arActivado)
        {
            ActivarAR();
        }

        // Inicializamos los valores de PlayerPrefs
        PlayerPrefs.SetInt("Espiritus", 0);
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        PlayerPrefs.SetInt("ObjetoEspiritus", 0);
        PlayerPrefs.SetInt("TiempoPartida", (int)tiempoRestante);
        PlayerPrefs.Save();

        // Inicializar música
        audioSource.clip = musica;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();
        Pausa();
    }

    private void Update()
    {
        espiritus = PlayerPrefs.GetInt("Espiritus");
        puntuacion = PlayerPrefs.GetInt("PuntuacionPartida");
        ActualizarPuntos();
        ActualizarContador();
        RestarTiempo();

    }

    private void ActualizarContador()
    {
        controlHud.ActualizarContador("ESPíRITUS", espiritus);
    }

    private void ActualizarPuntos()
    {
        controlHud.ActualizarPuntos("SCORE", puntuacion);
    }

    private void GuardarPuntos()
    {
        PlayerPrefs.SetInt("Puntuacion", PlayerPrefs.GetInt("Puntuacion") + puntuacion);
        PlayerPrefs.Save();
    }

    // Función para controlar el tiempo
    void RestarTiempo()
    {
        // Si el tiempo es mayor a 0, se resta el tiempo
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante < 0) tiempoRestante = 0;
            controlHud.ActualizarTiempo(tiempoRestante);
        }

        if (tiempoRestante == 0)
        {

            if (puntuacion > puntuacionVictoria)
            {
                ComprobarVictoriaObjeto();

                if (!objetoPartida)
                {
                    GuardarPuntos();
                    DesactivarAR();
                    controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Exito);
                }
            }
            else
            {
                GuardarPuntos();
                DesactivarAR();
                controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Derrota);
            }
        }
    }

    // Función para comprobar si se ha ganado el juego obteniendo el objeto
    private void ComprobarVictoriaObjeto()
    {

        if (espiritus >= espiritusObjetivo)
        {
            PlayerPrefs.SetInt("ObjetoEspiritus", 1);
            PlayerPrefs.Save();

            if (SceneManager.sceneCount <= 1 && !objetoPartida)
            {
                objetoPartida = true;
                GuardarPuntos();
                CargarPremio();
            }
        }
    }

    private void CargarPremio()
    {
        Time.timeScale = 0f;
        audioSource.Stop();

        // Cargar la escena de premio
        Debug.Log("Cargando escena: PremioGolpeBaston");
        SceneManager.sceneLoaded += OnPremioSceneLoaded;
        SceneManager.LoadScene("Premio", LoadSceneMode.Additive);
    }

    // Función para lanzar espera de la escena de premio cuando se carga
    private void OnPremioSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Premio")
        {
            Debug.Log("[Premio] Escena de premio cargada completamente.");
            // Desuscribirse del evento de carga de escena
            SceneManager.sceneLoaded -= OnPremioSceneLoaded;
            // Desactivar el objeto de la escena actual
            StartCoroutine(EsperarPremio());
        }
    }

    // Función para esperar antes de descargar la escena de premio
    IEnumerator EsperarPremio()
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.UnloadSceneAsync("Premio");
        yield return null;
        DesactivarAR();
        Time.timeScale = 1f;
        controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Exito);
    }

    // Función para pausar el juego
    public void Pausa()
    {
        Time.timeScale = 0f;
        audioSource.Pause();
    }

    // Función para reanudar el juego
    public void Play()
    {
        if (canvasBotonPlay != null)
            canvasBotonPlay.SetActive(false);

        Time.timeScale = 1f;
        audioSource.Play();
    }


    private void OnDisable()
    {
        if (arActivado)
        { 
            DesactivarAR();
        }
    }

    private void OnEnable()
    {
        if (!arActivado)
        {
            ActivarAR();
        }
    }

    private void DesactivarAR()
    {
        if (controlAR != null)
        {
            controlAR.DesactivarAR();
        }
        else if (controlAR_old != null)
        {
            controlAR_old.DesactivarAR();
        }
        arActivado = false;
    }

    private void ActivarAR()
    {
        if (controlAR != null)
        {
            controlAR.ActivarAR();
        }
        else if (controlAR_old != null)
        {
            controlAR_old.ActivarAR();
        }
        arActivado = true;
    }
}
