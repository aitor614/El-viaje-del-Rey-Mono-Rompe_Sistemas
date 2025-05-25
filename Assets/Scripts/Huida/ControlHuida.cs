using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public AudioClip colision;
    public AudioClip premio;
    public AudioClip vidaExtra;

    [Header("Elementos de la escena")]
    public AudioSource audioSource;
    public PlayerHuida player;
    public GameObject canvasBotonPlay;
    public GameObject obstaculo;

    [Header("Parámetros")]
    public float tiempoRestante;
    public int objetivoOrbes;
    public int puntuacionVictoria;

    // Variables
    private int vidas = 3;
    private int puntuacion = 0;
    private int orbesObtenidos = 0;
    private bool objetoPartida = false;
    private bool saltoInicial = false;

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
        PlayerPrefs.SetInt("PremiosObtenidos", 0);
        PlayerPrefs.SetInt("ObjetoHuida", 0);
        PlayerPrefs.SetInt("TiempoPartida", (int)tiempoRestante);
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
        orbesObtenidos = PlayerPrefs.GetInt("PremiosObtenidos");
        vidas = PlayerPrefs.GetInt("VidasRestantes");

        RestarTiempo();
        ActualizarPuntos();
        ActualizarContador();
        ActualizarVidas();
        ComprobarVictoriaObjeto();
    }

    // Comprobar si se ha obtenido el objeto de victoria
    private void ComprobarVictoriaObjeto()
    {
        if (orbesObtenidos >= objetivoOrbes)
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
            Debug.Log("Escena de premio cargada completamente.");
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
        Time.timeScale = 1f;
        controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Exito);
    }

    private void ActualizarContador()
    {
        controlHud.ActualizarContador("ORBES", orbesObtenidos);
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
            // Si la la puntuación es mayor o igual puntuaciónVictoria, se gana el juego
            if (puntuacion >= puntuacionVictoria)
            {
                GuardarPuntos();
                controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Exito);
            }
            // Si es menor, se pierde el juego
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
        if (saltoInicial == false)
        {
            player.Salto();
            saltoInicial = true;
        }

    }

    public void ObstaculoSalvado()
    {
        PlayerPrefs.SetInt("PuntuacionPartida", PlayerPrefs.GetInt("PuntuacionPartida") + 5);
        PlayerPrefs.Save();
    }

    // Función para gestionar la colisión del jugador con los obstáculos
    public void ColisionObstaculo()
    {
        AudioSource.PlayClipAtPoint(colision, transform.position);
        PlayerPrefs.SetInt("VidasRestantes", PlayerPrefs.GetInt("VidasRestantes") - 1);
        PlayerPrefs.Save();
        canvasBotonPlay.SetActive(true);
        CheckVida();
        Pausa();
    }

    public void ColisionPremio(Collider2D other)
    {
        AudioSource.PlayClipAtPoint(premio, transform.position);
        PlayerPrefs.SetInt("PuntuacionPartida", PlayerPrefs.GetInt("PuntuacionPartida") + 20);
        PlayerPrefs.SetInt("PremiosObtenidos", PlayerPrefs.GetInt("PremiosObtenidos") + 1);
        PlayerPrefs.Save();
        ActualizarPuntos();
        Destroy(other.gameObject);
    }

    public void ColisionVida(Collider2D other)
    {
        AudioSource.PlayClipAtPoint(vidaExtra,transform.position);
        PlayerPrefs.SetInt("PuntuacionPartida", PlayerPrefs.GetInt("PuntuacionPartida") + 1);
        int vidas = PlayerPrefs.GetInt("VidasRestantes");
        if (vidas < 3)
        {
            PlayerPrefs.SetInt("VidasRestantes", vidas + 1);
            Debug.Log("Vida extra obtenida. Antes " + vidaExtra + ". Ahora: " + PlayerPrefs.GetInt("VidasRestantes"));
        }
        PlayerPrefs.Save();
        Debug.Log("Vidas Restantes: " + PlayerPrefs.GetInt("VidasRestantes"));
        ActualizarVidas();
        Destroy(other.gameObject);
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
        PlayerPrefs.SetInt("VidasRestantes", vidas);
        PlayerPrefs.Save();
    }

}