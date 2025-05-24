using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControlGolpe : MonoBehaviour
{
    [Header("Controles")]
    public static ControlGolpe InstanciaControl { get; private set; }
    private ControlMenuPrincipal controlMenuPrincipal;
    private ControlHud controlHud;

    [Header("Sonidos")]
    public AudioClip musica;


    [Header("Elementos de la escena")]
    public Ball ball;
    public PlayerBaston player;
    public AudioSource audioSource;
    public GameObject canvasBotonPlay;

    [Header("Parámetros")]
    public float tiempoRestante;
    public int objetivoLadrillos;
    public int puntuacionVictoria;

    // Variables
    private int vidas = 3;
    private int puntuacion = 0;
    private int ladrillosRotos = 0;
    private bool objetoPartida = false;

    // Funcion para inicializar el script
    void Awake()
    {
        // Inicializar el singleton (instancia de ControlBreakout)
        InstanciaControl = this;
    }

    // Función para ejecutar al inicio
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        controlHud = ControlHud.InstanciaControl;
        controlMenuPrincipal = ControlMenuPrincipal.InstanciaControl;
        PlayerPrefs.SetInt("VidasRestantes", vidas);
        PlayerPrefs.SetInt("PuntuacionPartida", puntuacion);
        PlayerPrefs.SetInt("Ladrillos", ladrillosRotos);
        PlayerPrefs.SetInt("ObjetoBaston", 0);
        PlayerPrefs.SetInt("TiempoPartida", (int)tiempoRestante);
        PlayerPrefs.Save();

        // Inicializar música
        audioSource.clip = musica;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();
        Pausa();

    }

    // Función para ejecutar en cada frame
    void Update()
    {
        puntuacion = PlayerPrefs.GetInt("PuntuacionPartida");
        ladrillosRotos = PlayerPrefs.GetInt("Ladrillos");

        RestarTiempo();
        ActualizarPuntos();
        ActualizarContador();
        CheckVida();
        ComprobarVictoriaObjeto();
        ComprobarInput();
    }

    // Función para comprobar la entrada del jugador
    private void ComprobarInput()
    {
        // Si el jugador presiona la tecla Escape, se muestra el menú principal
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Menu);
        }

    }

    // Función para comprobar si se ha ganado el juego obteniendo el objeto
    private void ComprobarVictoriaObjeto()
    {

        // Si se han roto todos los bloques, se gana el juego
        if (ladrillosRotos >= objetivoLadrillos)
        {
            PlayerPrefs.SetInt("ObjetoBaston", 1);
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
            Debug.Log("Escena de premio cargada completamente.");
            // Desuscribirse del evento de carga de escena
            SceneManager.sceneLoaded -= OnPremioSceneLoaded;
            // Desactivar el objeto de la escena actual
            StartCoroutine(EsperarPremio());
        }
    }

    // Función para esperar 5 segundos antes de descargar la escena de premio
    IEnumerator EsperarPremio()
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.UnloadSceneAsync("Premio");
        yield return null;
        Time.timeScale = 1f;
        controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Exito);
    }


    private void ActualizarContador()
    {
        controlHud.ActualizarContador("LADRILLOS", ladrillosRotos);
    }

    private void ActualizarPuntos()
    {
        controlHud.ActualizarPuntos("SCORE", puntuacion);
    }

    private void ActualizarVidas()
    {
        controlHud.ActualizarEmblemas(vidas);
    }

    private void ActualizarTiempo()
    {
        controlHud.ActualizarTiempo(tiempoRestante);
    }

    // Función para controlar el tiempo
    void RestarTiempo()
    {
        // Si el tiempo es mayor a 0, se resta el tiempo
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante < 0)
                tiempoRestante = 0;
            // Mostramos el tiempo restante en el UI
            ActualizarTiempo();
        }

        if (tiempoRestante == 0)
        {
            // Si la la puntuación es mayor a 50, se gana el juego
            if (puntuacion > puntuacionVictoria)
            {
                GuardarPuntos();
                controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Exito);
            }
            // Si es menor o igual a 50, se pierde el juego
            else
            {
                GuardarPuntos();
                controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Derrota);
            }
        }

    }

    // Función para el control de vidas
    public void CheckVida()
    {
        
        if (vidas != PlayerPrefs.GetInt("VidasRestantes"))
        {
            vidas = PlayerPrefs.GetInt("VidasRestantes");
            ActualizarVidas();
            // Si el jugador pierde todas las vidas, el juego termina con derrota
            if (vidas <= 0)
            {
                ball.gameObject.SetActive(false);
                GuardarPuntos();
                controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Derrota);
            }
            // Si el jugador aún tiene vidas, se reinician los objetos
            else
            {
                Pausa();
                if (canvasBotonPlay != null)
                    canvasBotonPlay.SetActive(true);
                Debug.Log("Reiniciando objetos.");
                ResetObjetos();
            }
        }

    }

    public void Pausa()
    {
        Time.timeScale = 0f;
        ball.enabled = false;
        player.enabled = false;
        audioSource.Pause();
    }

    public void Play()
    {
        if (canvasBotonPlay != null)
            canvasBotonPlay.SetActive(false);

        Time.timeScale = 1f;
        ball.enabled = true;
        player.enabled = true;
        audioSource.Play();
    }


    // Función para reiniciar objetos de juego
    public void ResetObjetos()
    {
        ball.ResetBall();
        player.ResetPlayer();
    }

    private void GuardarPuntos()
    {
        PlayerPrefs.SetInt("PuntuacionPartida", puntuacion);
        PlayerPrefs.SetInt("Puntuacion", PlayerPrefs.GetInt("Puntuacion") + puntuacion);
        PlayerPrefs.Save();
    }
}
