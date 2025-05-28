using System.Collections;
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
    public AudioClip sonidoJuego;
    public AudioSource audioSource;
    [SerializeField] [Range(0f, 1f)]
    public float volumenMusica;

    public static ControlMenuPrincipal InstanciaControl { get; private set; }
    public static GestorXR InstanciaGestorXR { get; private set; }

    public enum ModoJuego { Individual, Continuo }
    public ModoJuego modoActual;

    public enum ResultadoMinijuego { Exito, Derrota, Reiniciar, Menu, Salir }
    public ResultadoMinijuego resultadoMinijuego;

    private string escenaActual = "";
    private bool reproduciendo = false;

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

    private void Awake()
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
            if (InstanciaGestorXR == null)
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

    // Inicializar el script
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;

        InicializarMenu();
    }

    private void InicializarMenu()
    {

        Screen.orientation = ScreenOrientation.Portrait;
        modoActual = ModoJuego.Individual;
        resultadoMinijuego = ResultadoMinijuego.Menu;
        PlayerPrefs.SetString("EscenaActual", "MenuPrincipal");
        PlayerPrefs.SetInt("IndiceMinijuego", 0);
        PlayerPrefs.Save();

        escenaActual = "MenuPrincipal";
        indiceActual = 0;

        ReiniciarRegistros();

        // Asignar botones y funciones
        AsignarBotones();
        ActivarBotones();
        AsignarFuncionesBotones();

        audioSource.clip = musica;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = volumenMusica;
        if (audioSource != null && !audioSource.isPlaying) audioSource.Play();

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

            StartCoroutine(ReinicializarMenu());
        }
    }

    private IEnumerator ReinicializarMenu()
    {
        // Esperar un frame para asegurarse de que la escena se ha cargado completamente
        yield return null;

        AsignarBotones();
        AsignarFuncionesBotones();

        if (audioSource != null && !audioSource.isPlaying) audioSource.Play();
    }


    private void AsignarBotones()
    {
        // Asignar los botones al script
        BtnSalirJuego = BuscarBoton("BtnSalirJuego");
        BtnPlayViaje = BuscarBoton("BtnPlayViaje");
        BtnPlayInfierno = BuscarBoton("BtnPlayInfierno");
        BtnPlayHuida = BuscarBoton("BtnPlayHuida");
        BtnPlayBaston = BuscarBoton("BtnPlayBaston");
        BtnPlayEspiritus = BuscarBoton("BtnPlayEspiritus");
        BtnPlayCelestial = BuscarBoton("BtnPlayCelestial");
    }

    // Función para buscar un bot�n por su nombre
    private Button BuscarBoton(string nombre)
    {
        var boton = GameObject.Find(nombre);
        return boton != null ? boton.GetComponent<Button>() : null;
    }

    // Asignar funciones a los botones
    private void AsignarFuncionesBotones()
    {
        if (BtnPlayViaje != null) BtnPlayViaje.onClick.AddListener(Click_JugarTodos);
        if (BtnPlayInfierno != null) BtnPlayInfierno.onClick.AddListener(Click_JuegoInfierno);
        if (BtnPlayHuida != null) BtnPlayHuida.onClick.AddListener(Click_JuegoHuida);
        if (BtnPlayBaston != null) BtnPlayBaston.onClick.AddListener(Click_JuegoBaston);
        if (BtnPlayEspiritus != null) BtnPlayEspiritus.onClick.AddListener(Click_JuegoEspiritus);
        if (BtnPlayCelestial != null) BtnPlayCelestial.onClick.AddListener(Click_JuegoVR);
        if (BtnSalirJuego != null) BtnSalirJuego.onClick.AddListener(Click_SalirJuego);
    }

    // Función para desactivar los botones
    private void DesactivarBotones()
    {
        if (BtnPlayViaje != null) BtnPlayViaje.interactable = false;
        if (BtnPlayInfierno != null) BtnPlayInfierno.interactable = false;
        if (BtnPlayHuida != null) BtnPlayHuida.interactable = false;
        if (BtnPlayBaston != null) BtnPlayBaston.interactable = false;
        if (BtnPlayEspiritus != null) BtnPlayEspiritus.interactable = false;
        if (BtnPlayCelestial != null) BtnPlayCelestial.interactable = false;
        if (BtnSalirJuego != null) BtnSalirJuego.interactable = false;
    }

    // Función para activar los botones
    private void ActivarBotones()
    {
        if (BtnPlayViaje != null) BtnPlayViaje.interactable = true;
        if (BtnPlayInfierno != null) BtnPlayInfierno.interactable = true;
        if (BtnPlayHuida != null) BtnPlayHuida.interactable = true;
        if (BtnPlayBaston != null) BtnPlayBaston.interactable = true;
        if (BtnPlayEspiritus != null) BtnPlayEspiritus.interactable = true;
        if (BtnPlayCelestial != null) BtnPlayCelestial.interactable = true;
        if (BtnSalirJuego != null) BtnSalirJuego.interactable = true;
    }

    // Funci�n para ejecutar en cada frame
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
    public IEnumerator JugarMinijuego(string nombreEscena)
    {
        escenaActual = nombreEscena;

        ReiniciarRegistros();
        audioSource.Stop();
        DesactivarBotones();

        PlayerPrefs.SetString("EscenaActual", nombreEscena);
        PlayerPrefs.Save();
        if(!reproduciendo) {
            AudioSource.PlayClipAtPoint(sonidoJuego, Camera.main.transform.position, 0.5f);
            reproduciendo = true;
        }
        yield return new WaitForSeconds(sonidoJuego.length);
        SceneManager.LoadScene(nombreEscena);
    }

    public void Click_JuegoInfierno()
    {       
        // Cargar la escena del minijuego de Escape del Infierno
        modoActual = ModoJuego.Individual;
        StartCoroutine(JugarMinijuego("Juego2DEscapeInfierno"));
    }

    public void Click_JuegoHuida()
    {
        // Cargar la escena del minijuego de Huida Celestial
        modoActual = ModoJuego.Individual;
        StartCoroutine(JugarMinijuego("Juego2DHuidaCelestial"));
    }

    public void Click_JuegoBaston()
    {
        // Cargar la escena del minijuego de Golpe Bast�n
        modoActual = ModoJuego.Individual;
        StartCoroutine(JugarMinijuego("Juego2DGolpeBaston"));
    }

    public void Click_JuegoEspiritus()
    {
        // Cargar la escena del minijuego de Esp�ritus Desencarnados
        modoActual = ModoJuego.Individual;
        StartCoroutine(JugarMinijuego("JuegoAREspiritusDesencarnados"));
    }

    public void Click_JuegoVR()
    {
        // Cargar la escena del minijuego de Batalla Celestial
        modoActual = ModoJuego.Individual;
        StartCoroutine(JugarMinijuego("JuegoVRBatallaCelestial"));
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
        indiceActual = PlayerPrefs.GetInt("IndiceMinijuego");
        StartCoroutine(JugarMinijuego(escenasMinijuegos[indiceActual]));
    }

    // Funcion para cargar el siguiente minijuego en modo continuo
    public void SiguienteMinijuego()
    {
        indiceActual = PlayerPrefs.GetInt("IndiceMinijuego") + 1;

        if (indiceActual < escenasMinijuegos.Count)
        {
            // Cargar la siguiente escena de minijuego
            escenaActual = escenasMinijuegos[indiceActual];
            PlayerPrefs.SetString("EscenaActual", escenaActual);
            PlayerPrefs.SetInt("IndiceMinijuego", indiceActual);
            PlayerPrefs.Save();
            SceneManager.LoadScene(escenasMinijuegos[indiceActual]);
            Debug.Log("[MenuPrincipal] Cargando escena: " + escenasMinijuegos[indiceActual]);
            Debug.Log("[MenuPrincipal] Indice actual: " + indiceActual);
            Debug.Log("[MenuPrincipal] Escena actual: " + escenaActual);
        }
        else
        {
            PlayerPrefs.SetInt("IndiceMinijuego", 0);
            PlayerPrefs.Save();
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
            // Cargar la escena de �xito
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
            StartCoroutine(JugarMinijuego(escenaActual));
        }
        else if (resultado == ResultadoMinijuego.Menu)
        {
            ReiniciarRegistros();
            InicializarMenu();
            SceneManager.LoadScene("MenuPrincipal");
        }
        else if (resultado == ResultadoMinijuego.Salir)
        {
            // Salir del juego
            Application.Quit();
        }
    }

    // Reiniciar registros de PlayerPrefs
    private void ReiniciarRegistros()
    {
        PlayerPrefs.SetInt("Puntuacion", 0);
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        PlayerPrefs.SetInt("ObjetoInfierno", 0);
        PlayerPrefs.SetInt("ObjetoHuida", 0);
        PlayerPrefs.SetInt("ObjetoBaston", 0);
        PlayerPrefs.SetInt("ObjetoEspiritus", 0);
        PlayerPrefs.SetInt("ObjetoBatalla", 0);
        PlayerPrefs.Save();

        reproduciendo = false;

        Debug.Log("[MenuPrincipal] Registros reiniciados.");
        Debug.Log("[MenuPrincipal] Escena actual: " + PlayerPrefs.GetString("EscenaActual"));
        Debug.Log("[MenuPrincipal] Puntuaciónn: " + PlayerPrefs.GetInt("Puntuacion"));
        Debug.Log("[MenuPrincipal] Puntuaciónn de partida: " + PlayerPrefs.GetInt("PuntuacionPartida"));
    }

}


