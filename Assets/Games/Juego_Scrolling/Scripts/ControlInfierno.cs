using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlInfierno : MonoBehaviour
{
    [Header("Controles")]
    public static ControlInfierno Instancia { get; private set; }
    public ControlHud controlHud;
    private ControlMenuPrincipal controlMenuPrincipal;

    [Header("Elementos de la escena")]
    public PlayerInfierno player;

    [Header("Scripts")]
    //public Temp tempScript;

    [Header("Parámetros")]
    public Vector3 startPosition;
    public float tiempoRestante;
    public int puntuacionVictoria;
    public int puntosAltura = 10;

    // Variables
    private int vidas = 3;
    private int puntuacion = 0;
    private float alturaAlcanzada = 0f;

    void Awake()
    {
        Instancia = this;
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
        PlayerPrefs.Save();

        if (player != null)
        {
            startPosition = player.transform.position;
            alturaAlcanzada = player.transform.position.y;
        }
    }

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
            GuardarPuntos();
            controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Exito);
        }
    }

    private void ControlarAltura()
    {
        if (player == null) return;

        float yActual = player.transform.position.y;
        if (yActual - alturaAlcanzada > 1f)
        {
            alturaAlcanzada = yActual;
            puntuacion = Mathf.FloorToInt(alturaAlcanzada * puntosAltura);
            PlayerPrefs.SetFloat("AlturaMaxima", alturaAlcanzada);
            PlayerPrefs.Save();
        }
    }

    // Función para el control de vidas
    public void CheckVida()
    {
        //dDebug.Log("Comprobando vidas...");
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
                Debug.Log("Reiniciando jugador...");
                RespawnPlayer();
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
        // Si el jugador aún tiene vidas, se reinician los objetos
        else
        {
            Debug.Log("Reiniciando jugador...");
            RespawnPlayer();
        }
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

    private void RespawnPlayer()
    {
        player.ResetPlayer();
    }

}