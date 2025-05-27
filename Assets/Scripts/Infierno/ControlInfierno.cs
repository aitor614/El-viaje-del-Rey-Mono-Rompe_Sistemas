using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Audio;



public class ControlInfierno : MonoBehaviour
{
    [Header("Controles")]
    public static ControlInfierno Instancia { get; private set; }
    public ControlHud controlHud;
    private ControlMenuPrincipal controlMenuPrincipal;

    [Header("Sonidos")]
    public AudioClip musicaFondo;

    [Header("Elementos de la escena")]
    public PlayerInfierno player;
    public GameObject canvasBotonPlay;
    public AudioSource audioSource;

    [Header("Parámetros")]
    public Vector3 startPosition;
    public float tiempoRestante;
    public int puntuacionVictoria;
    public int puntosAltura;

    // Variables
    private int vidas = 3;
    private int puntuacion = 0;
    private float alturaAlcanzada = 0f;
    private bool objetoPartida = false;

    void Awake()
    {
        Instancia = this;
    }

    private void OnDestroy()
    {
        if (Instancia == this)
        {
            Instancia = null;
        }
    }

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        controlMenuPrincipal = ControlMenuPrincipal.InstanciaControl;
        controlHud = ControlHud.InstanciaControl;
        PlayerPrefs.SetInt("VidasRestantes", vidas);
        PlayerPrefs.SetInt("PuntuacionPartida", puntuacion);
        PlayerPrefs.SetFloat("AlturaMaxima", alturaAlcanzada);
        PlayerPrefs.SetInt("ObjetoInfierno", 0);
        PlayerPrefs.SetInt("TiempoPartida", (int)tiempoRestante);
        PlayerPrefs.Save();
        //SceneManager.sceneLoaded += OnSceneLoaded;

        if (audioSource != null && musicaFondo != null)
        {
            audioSource.clip = musicaFondo;
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.Play();
        }

        if (player != null)
        {
            startPosition = player.transform.position;
            alturaAlcanzada = player.transform.position.y;
        }

        if (canvasBotonPlay != null)
            canvasBotonPlay.SetActive(true); // Botón activo al inicio del juego

        Pausa();
    }


    /*
      void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Camera.main.GetComponent<CameraFollow>().ResetCameraPosition();
    }
     */
   

    private void Update()
    {
        CheckVida();
        ControlarAltura();
        RestarTiempo();
        ActualizarAltura();
        ActualizarPuntos();
        CheckObjeto();
    }

    private void CheckObjeto()
    {
        if (PlayerPrefs.GetInt("ObjetoInfierno") == 1)
        {
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

    private void ControlarAltura()
    {
        if (player == null) return;

        float yActual = player.transform.position.y;

        // Altura relativa desde el punto de partida
        float alturaRelativa = yActual - startPosition.y;

        // Asegurarse de que solo se registra si ha superado la marca anterior
        if (alturaRelativa > alturaAlcanzada + 1f)
        {
            alturaAlcanzada = alturaRelativa;

            // Asegurar que la puntuación también es siempre positiva
            puntuacion = Mathf.FloorToInt(Mathf.Max(0f, alturaRelativa) * puntosAltura);

            // Guardar la altura relativa
            PlayerPrefs.SetFloat("AlturaMaxima", alturaAlcanzada);
            PlayerPrefs.SetInt("PuntuacionPartida", puntuacion);
            PlayerPrefs.Save();
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
                GuardarPuntos();
                controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Derrota);
            }
            // Si el jugador aún tiene vidas, se reinician los objetos
            else
            {
                Pausa(); // Pausa del juego automáticamente
                if (canvasBotonPlay != null)
                    canvasBotonPlay.SetActive(true); // Se activa el botón para que el jugador continúe manualmente
                Debug.Log("Reiniciando jugador...");
                RespawnPlayer();
            }
        }

    }

    public void Pausa()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        audioSource.Pause();
    }

    public void Play()
    {
        if (canvasBotonPlay != null)
            canvasBotonPlay.SetActive(false);
        
        Time.timeScale = 1f;
        player.enabled = true;
        audioSource.Play();
    }

    // Actualiza el contador de saltos
    private void ActualizarAltura()
    {
        controlHud.ActualizarContador("Altura", (int)alturaAlcanzada);
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

    private void ActualizarTiempo()
    {
        controlHud.ActualizarTiempo(tiempoRestante);
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

    private void RespawnPlayer()
    {
        player.ResetPlayer();
    }

}