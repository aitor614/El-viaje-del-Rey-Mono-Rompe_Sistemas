using UnityEngine;
using UnityEngine.UI;

public class ControlHuida : MonoBehaviour
{
    [Header("Controles")]
    public static ControlHuida InstanciaControl { get; private set; }
    private ControlMenuPrincipal controlMenuPrincipal;
    private ControlHud controlHud;

    [Header("Sonidos")]
    public AudioClip musica;
    public AudioClip respawn;

    [Header("Elementos de la escena")]
    public AudioSource audioSource;
    public PlayerHuida player;
    public GeneradorObstaculos genObstaculos;
    public GameObject canvasBotonPlay;
    public GameObject obstaculo;

    [Header("Parámetros")]
    public float tiempoRestante;
    public int objetivoObjetos;
    public int puntuacionVictoria;

    // Variables
    private int vidas = 3;
    private int puntuacion = 0;
    private int obstaculosSalvados = 0;

    private void Awake()
    {
        InstanciaControl = this;
    }

    private void OnDestroy()
    {
        if (InstanciaControl == this)
        {
            InstanciaControl = null;
        }
    }

    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        controlMenuPrincipal = ControlMenuPrincipal.InstanciaControl;
        controlHud = ControlHud.InstanciaControl;
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        PlayerPrefs.SetInt("VidasRestantes", vidas);
        PlayerPrefs.SetInt("ObstaculosSalvados", 0);
        PlayerPrefs.SetInt("ObjetoHuida", 0);
        PlayerPrefs.Save();
        player.enabled = false;
        audioSource.clip = musica;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        Pausa();

    }

    private void Update()
    {
        puntuacion = PlayerPrefs.GetInt("PuntuacionPartida");
        obstaculosSalvados = PlayerPrefs.GetInt("ObstaculosSalvados");

        RestarTiempo();
        ActualizarPuntos();
        ActualizarContador();
        ComprobarVictoriaObjeto();
    }

    private void ComprobarVictoriaObjeto()
    {

        // Si se han roto todos los bloques, se gana el juego
        if (obstaculosSalvados == objetivoObjetos)
        {
            PlayerPrefs.SetInt("ObjetoBaston", 1);
            PlayerPrefs.Save();
            GuardarPuntos();
            controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Exito);
        }
    }

    private void ActualizarContador()
    {
        controlHud.ActualizarContador("OBSTáCULOS", obstaculosSalvados);
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

        vidas = PlayerPrefs.GetInt("VidasRestantes");
        ActualizarVidas();
        // Si el jugador pierde todas las vidas, el juego termina con derrota
        if (vidas <= 0)
        {
            GuardarPuntos();
            controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Derrota);
        }

    }

    // Función para pausar el juego a espera de que el jugador pulse el botón de play
    public void Pausa()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        audioSource.Pause();
    }

    // Función para iniciar el juego pulsando el botón de play
    public void Play()
    {
        canvasBotonPlay.SetActive(false);
        
        Time.timeScale = 1f;
        player.enabled = true;
        audioSource.Play();

        EliminarObstaculos();

    }

    // Función para eliminar obstáculos
    public void EliminarObstaculos()
    {
        // Se obtienen todos los obstáculos de la escena
        GameObject[] obstaculos = GameObject.FindGameObjectsWithTag(obstaculo.tag);

        // Se recorre todos los obstáculos y se destruyen
        for (int i = 0; i < obstaculos.Length; i++)
        {
            Destroy(obstaculos[i].gameObject);
            PlayerPrefs.SetInt("ObstaculosSalvados", PlayerPrefs.GetInt("ObstaculosSalvados") + 1);
            PlayerPrefs.Save();
        }
    }

    // Función para gestionar la colisión del jugador con los obstáculos
    public void Colision()
    {

        PlayerPrefs.SetInt("VidasRestantes", PlayerPrefs.GetInt("VidasRestantes") - 1);
        PlayerPrefs.Save();
        canvasBotonPlay.SetActive(true);
        Pausa();
        CheckVida();
    }

    // Función para reiniciar objetos de juego
    public void ResetObjetos()
    {
        player.ResetPlayer();
        audioSource.PlayOneShot(respawn);
    }

    // Función para guardar los puntos
    private void GuardarPuntos()
    {
        PlayerPrefs.SetInt("PuntuacionPartida", puntuacion);
        PlayerPrefs.SetInt("Puntuacion", PlayerPrefs.GetInt("Puntuacion") + puntuacion);
        PlayerPrefs.Save();
    }

}