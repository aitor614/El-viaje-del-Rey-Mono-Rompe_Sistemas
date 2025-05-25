using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlMenuPrincipal : MonoBehaviour
{

    [Header("Controles")]
    public GestorXR gestorXR;

    [Header("Sonidos")]
    public AudioClip musica;

    [Header("Elementos de la escena")]
    public AudioSource audioSource;

    public static ControlMenuPrincipal InstanciaControl { get; private set; }
    public static GestorXR InstanciaGestorXR { get; private set; }

    public enum ModoJuego { Individual, Continuo }
    public ModoJuego modoActual;

    public enum ResultadoMinijuego { Exito, Derrota, Reiniciar, Menu, Salir }
    public ResultadoMinijuego resultadoMinijuego;

    private string escenaActual = "";

    public int indiceActual = 0;

    public Button BtnSalirJuego;
    public Button BtnPlayViaje;
    public Button BtnPlayInfierno;
    public Button BtnPlayHuida;
    public Button BtnPlayBaston;
    public Button BtnPlayEspiritus;
    public Button BtnPlayCelestial;


    // Lista de escenas de minijuegos
    private readonly List<string> escenasMinijuegos = new()
    {
        "Juego2DEscapeInfierno", 
        "Juego2DHuidaCelestial", 
        "Juego2DGolpeBaston", 
        "JuegoAREspiritusDesencarnados",
        "JuegoVRBatallaCelestial"
    };

    // Inicializar el script
    void Start()
    {
        PlayerPrefs.SetString("EscenaActual", "");
        PlayerPrefs.Save();
        Screen.orientation = ScreenOrientation.Portrait;
        resultadoMinijuego = ResultadoMinijuego.Menu;

        // Asignar funciones a los botones
        BtnSalirJuego.onClick.AddListener(Click_SalirJuego);
        BtnPlayViaje.onClick.AddListener(Click_JugarTodos);
        BtnPlayInfierno.onClick.AddListener(Click_JuegoInfierno);
        BtnPlayHuida.onClick.AddListener(Click_JuegoHuida);
        BtnPlayBaston.onClick.AddListener(Click_JuegoBaston);
        BtnPlayEspiritus.onClick.AddListener(Click_JuegoEspiritus);
        BtnPlayCelestial.onClick.AddListener(Click_JuegoVR);

        audioSource.clip = musica;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();

        ReiniciarPuntuaciones();
    }

    // Cuando el objeto se activa
    void OnEnable()
    {
        // Suscribirse al evento de carga de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Cuando el objeto se desactiva
    void OnDisable()
    {
        // Desuscribirse del evento de carga de escena
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Cuando se carga una escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MenuPrincipal")
        {
            Screen.orientation = ScreenOrientation.Portrait;
            // Asignar los botones al script
            BtnSalirJuego = GameObject.Find("BtnSalirJuego") != null ? GameObject.Find("BtnSalirJuego").GetComponent<Button>() : null;
            BtnPlayViaje = GameObject.Find("BtnPlayViaje") != null ? GameObject.Find("BtnPlayViaje").GetComponent<Button>() : null;
            BtnPlayInfierno = GameObject.Find("BtnPlayInfierno") != null ? GameObject.Find("BtnPlayInfierno").GetComponent<Button>() : null;
            BtnPlayHuida = GameObject.Find("BtnPlayHuida") != null ? GameObject.Find("BtnPlayHuida").GetComponent<Button>() : null;
            BtnPlayBaston = GameObject.Find("BtnPlayBaston") != null ? GameObject.Find("BtnPlayBaston").GetComponent<Button>() : null;
            BtnPlayEspiritus = GameObject.Find("BtnPlayEspiritus") != null ? GameObject.Find("BtnPlayEspiritus").GetComponent<Button>() : null;
            BtnPlayCelestial = GameObject.Find("BtnPlayCelestial") != null ? GameObject.Find("BtnPlayCelestial").GetComponent<Button>() : null;

            // Asignar funciones a los botones
            if (BtnSalirJuego != null) BtnSalirJuego.onClick.AddListener(Click_SalirJuego);
            if (BtnPlayViaje != null) BtnPlayViaje.onClick.AddListener(Click_JuegoInfierno);
            if (BtnPlayInfierno != null) BtnPlayInfierno.onClick.AddListener(Click_JuegoInfierno);
            if (BtnPlayHuida != null) BtnPlayHuida.onClick.AddListener(Click_JuegoHuida);
            if (BtnPlayBaston != null) BtnPlayBaston.onClick.AddListener(Click_JuegoBaston);
            if (BtnPlayEspiritus != null) BtnPlayEspiritus.onClick.AddListener(Click_JuegoEspiritus);
            if (BtnPlayCelestial != null) BtnPlayCelestial.onClick.AddListener(Click_JuegoVR);

            if (!audioSource.isPlaying)
                audioSource.Play();

        }
    }

    // Inicializar el script
    void Awake()
    {
        // Comprobar si ya existe una instancia de ControlMenuPrincipal
        if (InstanciaControl != null && InstanciaControl != this)
        {
            Destroy(gameObject);
            return;
        }

        if (InstanciaGestorXR == null)
        {
            InstanciaGestorXR = gestorXR;
            if(InstanciaGestorXR == null)
            {
                Debug.LogError("[ControlMenuPrincipal] GestorXR no encontrado. Buscando en la escena...");
                InstanciaGestorXR = FindAnyObjectByType<GestorXR>();
                if (InstanciaGestorXR == null)
                {
                    Debug.LogError("[ControlMenuPrincipal] GestorXR sigue sin encontrarse.");
                    return;
                }
            }
        }

        // Asignar la instancia de ControlMenuPrincipal
        InstanciaControl = this;
        // No destruir el objeto al cargar una nueva escena
        DontDestroyOnLoad(gameObject);
    }

    // Función para ejecutar en cada frame
    private void Update()
    {
        // Si el jugador presiona la tecla Escape sale del juego
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // Procesar el resultado del minijuego como Salir
            ProcesarResultado(ResultadoMinijuego.Salir);
        }

    }

    // Funcion para cargar la escena del minijuego
    public void JugarMinijuego(string nombreEscena)
    {
        escenaActual = nombreEscena;
        PlayerPrefs.SetString("EscenaActual", nombreEscena);
        PlayerPrefs.Save();
        SceneManager.LoadScene(nombreEscena);
    }

    public void Click_JuegoInfierno()
    {       
        ReiniciarPuntuaciones();

        audioSource.Stop();

        // Cargar la escena del minijuego de Escape del Infierno
        modoActual = ModoJuego.Individual;
        JugarMinijuego("Juego2DEscapeInfierno");
    }

    public void Click_JuegoHuida()
    {
        ReiniciarPuntuaciones();

        audioSource.Stop();

        // Cargar la escena del minijuego de Huida Celestial
        modoActual = ModoJuego.Individual;
        JugarMinijuego("Juego2DHuidaCelestial");
    }

    public void Click_JuegoBaston()
    {
        ReiniciarPuntuaciones();
        
        audioSource.Stop();

        // Cargar la escena del minijuego de Golpe Bastón
        modoActual = ModoJuego.Individual;
        JugarMinijuego("Juego2DGolpeBaston");
    }

    public void Click_JuegoEspiritus()
    {
        ReiniciarPuntuaciones();

        audioSource.Stop();

        // Cargar la escena del minijuego de Espíritus Desencarnados
        modoActual = ModoJuego.Individual;
        JugarMinijuego("JuegoAREspiritusDesencarnados");
    }

    public void Click_JuegoVR()
    {
        ReiniciarPuntuaciones();

        audioSource.Stop();

        // Cargar la escena del minijuego de Batalla Celestial
        modoActual = ModoJuego.Individual;
        JugarMinijuego("JuegoVRBatallaCelestial");
    }

    public void Click_SalirJuego()
    {
        audioSource.Stop();
        // Procesar el resultado del minijuego como Salir
        ProcesarResultado(ResultadoMinijuego.Salir);
    }

    // Funcion para jugar todos los minijuegos en modo continuo
    public void Click_JugarTodos()
    {
        // Guardar el modo de juego como continuo
        modoActual = ModoJuego.Continuo;
        indiceActual = 0;
        ReiniciarPuntuaciones();

        audioSource.Stop();

        SceneManager.LoadScene(escenasMinijuegos[indiceActual]);
    }

    // Funcion para cargar el siguiente minijuego en modo continuo
    public void SiguienteMinijuego()
    {

        if (indiceActual < escenasMinijuegos.Count)
        {
            indiceActual++;
            // Cargar la siguiente escena de minijuego
            escenaActual = escenasMinijuegos[indiceActual];
            PlayerPrefs.SetString("EscenaActual", escenaActual);
            PlayerPrefs.Save();
            SceneManager.LoadScene(escenasMinijuegos[indiceActual]);
            Debug.Log("Cargando escena: " + escenasMinijuegos[indiceActual]);
        }
        else
        {
            // Si se han completado todos los minijuegos, cargar la escena final
            SceneManager.LoadScene("EscenaFinal"); 
        }
    }

    // Funcion para procesar el resultado del minijuego
    public void ProcesarResultado(ResultadoMinijuego resultado)
    {
        // Guardar el resultado del minijuego
        resultadoMinijuego = resultado;
        if (resultado == ResultadoMinijuego.Exito)
        {
            // Cargar la escena de éxito
            SceneManager.LoadScene("YouWin");
        }
        else if (resultado == ResultadoMinijuego.Derrota)
        {
            // Cargar la escena de Game Over
            SceneManager.LoadScene("GameOver");
        }
        else if (resultado == ResultadoMinijuego.Reiniciar)
        {
            // Reiniciar el minijuego actual
            JugarMinijuego(escenaActual);
        }
        else if (resultado == ResultadoMinijuego.Menu)
        {
            ReiniciarPuntuaciones();
            // Volver al menú principal
            escenaActual = "MenuPrincipal";
            SceneManager.LoadScene("MenuPrincipal");
        }
        else if (resultado == ResultadoMinijuego.Salir)
        {
            // Salir del juego
            Application.Quit();
        }
    }

    private void ReiniciarPuntuaciones()
    {
        // Reiniciar las puntuaciones
        PlayerPrefs.SetInt("Puntuacion", 0);
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        PlayerPrefs.Save();
    }

}


