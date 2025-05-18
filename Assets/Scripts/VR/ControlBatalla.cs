using UnityEngine;



public class ControlBatalla : MonoBehaviour
{
    [Header("Controles")]
    public static ControlBatalla Instancia { get; private set; }
    public ControlHud controlHud;
    private ControlMenuPrincipal controlMenuPrincipal;
    private DañoScreen dañoScreen;
    private bool enPausa = true; // Inicia pausado

    public bool EstaEnPausa() => enPausa;

    [Header("Parámetros")]
    public float tiempoRestante;
    public int puntuacionVictoria;
    public int enemigosObjetivo;

    [Header("Pausa Inicial")]
    public GameObject canvasPausaInicial;
    public AudioSource audioSource; // Asigna el AudioSource con la música

    // Variables
    private int vidas = 3;
    private int puntuacion = 0;
    private int enemigosEliminados = 0;

    void Awake()
    {
        Instancia = this;
    }

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        controlMenuPrincipal = ControlMenuPrincipal.InstanciaControl;
        controlHud = ControlHud.InstanciaControl;
        dañoScreen = FindAnyObjectByType<DañoScreen>();

        PlayerPrefs.SetInt("VidasRestantes", vidas);
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        PlayerPrefs.SetInt("EnemigosEliminados", 0);
        PlayerPrefs.SetInt("ObjetoBatalla", 0);
        PlayerPrefs.Save();

        Pausa(); // Pausamos el juego al iniciar
    }

    void Update()
    {
        if (enPausa) return;
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

    public void CheckVida()
    {
        if (vidas != PlayerPrefs.GetInt("VidasRestantes"))
        {
            Debug.Log("Vidas cambiadas. Actualizando...");
            vidas = PlayerPrefs.GetInt("VidasRestantes");
            ActualizarVidas();
            if (vidas <= 0)
            {
                Debug.Log("Jugador sin vidas. Fin del juego.");
                GuardarPuntos();
                controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Derrota);
            }
        }
    }

    public void PerderVida()
    {
        Debug.Log("Perdiendo vida...");
        vidas--;
        dañoScreen?.MostrarDaño();
        ActualizarVidas();
        if (vidas <= 0)
        {
            Debug.Log("No quedan vidas. Fin del juego.");
            GuardarPuntos();
            controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Derrota);
        }
    }

    private void ActualizarEnemigos()
    {
        controlHud.ActualizarContador("Enemigos", enemigosEliminados);
    }

    private void ActualizarPuntos()
    {
        controlHud.ActualizarPuntos("SCORE", puntuacion);
    }

    private void ActualizarVidas()
    {
        controlHud.ActualizarEmblemas(vidas);
    }

    private void ActualizarTiempo(float tiempo)
    {
        controlHud.ActualizarTiempo(tiempo);
    }

    private void GuardarPuntos()
    {
        PlayerPrefs.SetInt("Puntuacion", PlayerPrefs.GetInt("Puntuacion") + puntuacion);
        PlayerPrefs.Save();
    }

    void RestarTiempo()
    {
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante < 0) tiempoRestante = 0;
            ActualizarTiempo(tiempoRestante);
        }

        if (tiempoRestante == 0)
        {
            if (puntuacion > puntuacionVictoria)
            {
                GuardarPuntos();
                controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Exito);
            }
            else
            {
                GuardarPuntos();
                controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Derrota);
            }
        }
    }

    // MÉTODOS PARA PAUSA Y PLAY INICIAL
    public void Pausa()
    {
        enPausa = true;
        audioSource?.Pause();
        if (canvasPausaInicial != null)
            canvasPausaInicial.SetActive(true);
    }

    public void Play()
    {
        Debug.Log("Botón Play pulsado");
        enPausa = false;
        audioSource?.Play();
        if (canvasPausaInicial != null)
            canvasPausaInicial.SetActive(false);
    }
}

/*
public class ControlBatalla : MonoBehaviour
{
    [Header("Controles")]
    public static ControlBatalla Instancia { get; private set; }
    public ControlHud controlHud;
    private ControlMenuPrincipal controlMenuPrincipal;
    private DañoScreen dañoScreen;


    [Header("Parámetros")]
    public float tiempoRestante;
    public int puntuacionVictoria;
    public int enemigosObjetivo;

    // Variables
    private int vidas = 3;
    private int puntuacion = 0;
    private int enemigosEliminados = 0;

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
        dañoScreen = FindAnyObjectByType<DañoScreen>();

        PlayerPrefs.SetInt("VidasRestantes", vidas);
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        PlayerPrefs.SetInt("EnemigosEliminados", 0);
        PlayerPrefs.SetInt("ObjetoBatalla", 0);
        PlayerPrefs.Save();
        



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
        dañoScreen?.MostrarDaño();
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
}
*/