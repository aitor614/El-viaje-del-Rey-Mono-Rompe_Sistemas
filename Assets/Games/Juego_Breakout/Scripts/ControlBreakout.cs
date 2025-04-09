using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlBreakout : MonoBehaviour
{
    public static ControlBreakout InstanciaControl { get; private set; }
    private ControlMenuPrincipal controlMenuPrincipal;

    public int lives = 3;

    public Transform puntoRespawn;
    public Ball ball;
    private Player player;
    private Brick[] bricks;
    public Temp tempScript;
    public TextMeshProUGUI TxtScore;

    public float leftTime = 30f;
    public int puntuacion = 0;

    // Funcion para inicializar el singleton (instancia de ControlBreakout)
    void Awake()
    {
        InstanciaControl = this;
    }

    void Start()
    {
        controlMenuPrincipal = ControlMenuPrincipal.InstanciaControl;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Asigna ballScript si ya existe en escena
        ball = FindFirstObjectByType<Ball>();
        player = FindFirstObjectByType<Player>();
        bricks = FindObjectsByType<Brick>(FindObjectsSortMode.None);

    }

    void Update()
    {
        FinishTime();
        TxtScore.text = "SCORE: " + puntuacion.ToString();
        // Si el jugador presiona la tecla Escape, se muestra el menú principal
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Menu);
        }

    }

    public void Pausar()
    {
        var controlPausa = FindAnyObjectByType<ControlPausa>();
        if (controlPausa != null)
        {
            controlPausa.Pausar();
        }
    }

    void FinishTime()
    {
        if (leftTime > 0)
        {
            leftTime -= Time.deltaTime;
            if (leftTime < 0)
                leftTime = 0;

            tempScript.RefreshText(leftTime); // Mostramos el tiempo
        }

        if (leftTime == 0)
        {
            if (puntuacion > 50)
            {
                controlMenuPrincipal.SumarPuntos(puntuacion);
                controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Exito);
            }
            else
            {
                controlMenuPrincipal.SumarPuntos(puntuacion);
                controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Derrota);
            }
        }

    }

    public void LooseHealth()
    {
        lives--;

        if (lives <= 0)
        {
            ball.gameObject.SetActive(false);
            controlMenuPrincipal.SumarPuntos(puntuacion);
            controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Derrota);
        }
        else
        {
            ResetObjetos();
        }
    }

    public void SumarPuntuacion(int puntos)
    {
        puntuacion += puntos;
    }

    public void ResetObjetos()
    {
        ball.ResetBall();
        player.ResetPlayer();
    }

}
