using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ControlBreakout : MonoBehaviour
{
    public static ControlBreakout InstanciaControl { get; private set; }
    private ControlMenuPrincipal controlMenuPrincipal;
    private ControlPausa controlPausa;

    public int vidas = 3;

    public Ball ball;
    public Player player;
    public Brick[] bricks;
    public Temp tempScript;
    public Emblema[] emblemas;
    public TextMeshProUGUI TxtScore;

    public float tiempoRestante = 30f;
    public int puntuacion = 0;

    // Funcion para inicializar el script
    void Awake()
    {
        // Inicializar el singleton (instancia de ControlBreakout)
        InstanciaControl = this;
    }

    // Función para ejecutar al inicio
    void Start()
    {
        controlMenuPrincipal = ControlMenuPrincipal.InstanciaControl;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Asigna ballScript si ya existe en escena
        ball = FindFirstObjectByType<Ball>();
        player = FindFirstObjectByType<Player>();
        bricks = FindObjectsByType<Brick>(FindObjectsSortMode.None);
    }

    // Función para ejecutar en cada frame
    void Update()
    {
        RestarTiempo();
        TxtScore.text = "SCORE: " + puntuacion.ToString();
        // Si el jugador presiona la tecla Escape, se muestra el menú principal
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Menu);
        }

        // Si se han roto todos los bloques, se gana el juego
        if (bricks.Length == 0)
        {
            GuardarPuntos();
            controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Exito);
        }

    }

    // Función para pausar el juego
    public void Pausar()
    {
        controlPausa = ControlPausa.InstanciaControl;
        if (controlPausa != null)
        {
            controlPausa.Pausar();
        }
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
            tempScript.RefreshText(tiempoRestante); // Mostramos el tiempo
        }

        if (tiempoRestante == 0)
        {
            // Si la la puntuación es mayor a 50, se gana el juego
            if (puntuacion > 50)
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
    public void PerderVida()
    {
        vidas--;
        emblemas[vidas].Destruir();
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
            ResetObjetos();
        }
    }

    // Función para sumar puntos
    public void SumarPuntuacion(int puntos)
    {
        puntuacion += puntos;
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
