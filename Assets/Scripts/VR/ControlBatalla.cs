using UnityEngine;

public class ControlBatalla : MonoBehaviour
{
    [Header("Controles")]
    public static ControlBatalla Instancia { get; private set; }
    public ControlHud controlHud;
    public ControlOpenXR controlOpenXR;
    public ControlAR controlAR;
    //public ControlCardBoard controlCardBoard;
    private ControlMenuPrincipal controlMenuPrincipal;

    [Header("Elementos de la escena")]
    public GameObject canvasBotonPlay;

    [Header("Parámetros")]
    public float tiempoRestante;
    public int puntuacionVictoria;
    public int enemigosObjetivo;

    [Header("Sonidos")]
    public AudioClip musica;
    public AudioSource audioSource;

    [Header("XR")]
    private GestorXR gestorXR;
    public GestorXR gestorXR_pruebas;

    public enum ModoXR { OpenXR, Cardboard, ARCore }
    public ModoXR modoSeleccionado;

    // Variables
    private int vidas = 3;
    private int puntuacion = 0;
    private int enemigosEliminados = 0;
    private bool vrActivado = false;

    void Awake()
    {
        Instancia = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        // Activar plugin OpenXR si no está activado
        if (!vrActivado)
        {
            ActivarVR();
        }

        // Inicializamos los valores de PlayerPrefs
        PlayerPrefs.SetInt("TiempoPartida", (int)tiempoRestante);
        PlayerPrefs.SetInt("VidasRestantes", vidas);
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        PlayerPrefs.SetInt("EnemigosEliminados", 0);
        PlayerPrefs.SetInt("ObjetoBatalla", 0);
        PlayerPrefs.Save();

        // Inicializar música
        audioSource.clip = musica;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();
        Pausa();
    }

    // Update is called once per frame
    void Update()
    {

        enemigosEliminados = PlayerPrefs.GetInt("EnemigosEliminados");
        puntuacion = PlayerPrefs.GetInt("PuntuacionPartida");

        CheckVida();
        RestarTiempo();
        ActualizarEnemigos();
        ActualizarPuntos();
        CheckObjeto();
    }
    private void CheckObjeto()
    {
        if (enemigosEliminados >= enemigosObjetivo)
        {
            PlayerPrefs.SetInt("ObjetoBatalla", 1);
            PlayerPrefs.Save();
            GuardarPuntos();
            controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Exito);
        }
    }

    // Función para el control de vidas
    public void CheckVida()
    {
        //dDebug.Log("Comprobando vidas...");
        if (vidas != PlayerPrefs.GetInt("VidasRestantes"))
        {
            Debug.Log("Vidas cambiadas. Actualizando...");
            vidas = PlayerPrefs.GetInt("VidasRestantes");
            ActualizarVidas();
            // Si el jugador pierde todas las vidas, el juego termina con derrota
            if (vidas <= 0)
            {
                Debug.Log("Jugador sin vidas. Fin del juego.");
                GuardarPuntos();
                controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Derrota);
            }
        }
    }

    // Función para el control de vidas
    public void PerderVida()
    {
        Debug.Log("Perdiendo vida...");
        vidas--;
        ActualizarVidas();
        // Si el jugador pierde todas las vidas, el juego termina con derrota
        if (vidas <= 0)
        {
            Debug.Log("No quedan vidas. Fin del juego.");
            GuardarPuntos();
            controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Derrota);
        }

    }

    // Actualiza el contador de saltos
    private void ActualizarEnemigos()
    {
        controlHud.ActualizarContador("Enemigos", enemigosEliminados);
    }

    // Actualiza el contador de puntos
    private void ActualizarPuntos()
    {
        controlHud.ActualizarPuntos("SCORE", puntuacion);
    }

    // Actualiza el contador de vidas
    private void ActualizarVidas()
    {
        controlHud.ActualizarEmblemas(vidas);
    }

    private void ActualizarTiempo(float tiempo)
    {
        controlHud.ActualizarTiempo(tiempo);
    }

    // Guarda la puntuación en PlayerPrefs
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
            if (tiempoRestante < 0)
                tiempoRestante = 0;
            // Mostramos el tiempo restante en el UI
            ActualizarTiempo(tiempoRestante);
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
        if (vrActivado)
        {
            DesactivarVR();
        }
    }

    private void OnEnable()
    {
        if (!vrActivado)
        {
            ActivarVR();
        }
    }

    // Desactiva el plugin VR
    private void DesactivarVR()
    {
        if (gestorXR == null)
        {
            Debug.LogError("[ControlBatalla] GestorXR no encontrado.");
            return;
        }

        switch (modoSeleccionado)
        {
            case ModoXR.OpenXR:
                if (controlOpenXR != null) controlOpenXR.DesactivarOpenXR();
                break;
            case ModoXR.Cardboard:
                // if (controlCardBoard != null) controlCardBoard.DesactivarCardBoard();
                break;
            case ModoXR.ARCore:
                if (controlAR != null) controlAR.DesactivarAR();
                break;
            default:
                Debug.LogError("[ControlBatalla] Modo XR no reconocido.");
                break;
        }

        vrActivado = false;
    }

    // Activa el plugin VR
    private void ActivarVR()
    {
        if (gestorXR == null)
        {
            Debug.LogError("[ControlBatalla] GestorXR no encontrado.");
            return;
        }

        switch (modoSeleccionado)
        {
            case ModoXR.OpenXR:
                if(controlOpenXR != null) controlOpenXR.ActivarOpenXR();
                break;
            case ModoXR.Cardboard:
                // if (controlCardBoard != null) controlCardBoard.DesactivarCardBoard();
                break;
            case ModoXR.ARCore:
                if (controlAR != null) controlAR.ActivarAR();
                break;
            default:
                Debug.LogError("[ControlBatalla] Modo XR no reconocido.");
                break;
        }

        vrActivado = true;
    }

}
