using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlBreakout : MonoBehaviour
{
    public int lives = 3;

    public Transform puntoRespawn;
    public Ball ball;
    private Player player;
    public Temp tempScript;
    public TextMeshProUGUI TxtScore;

    public float leftTime = 30f;
    public int puntuacion = 0;

    private ControlMenuPrincipal controlMenuPrincipal;


    void Start()
    {
        controlMenuPrincipal = FindFirstObjectByType<ControlMenuPrincipal>();
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Asigna ballScript si ya existe en escena
        ball = FindFirstObjectByType<Ball>();
        ball.Inicializar(this);
        player = FindFirstObjectByType<Player>();
        player.Inicializar(this);
        Brick[] bricks = FindObjectsByType<Brick>(FindObjectsSortMode.None);
        foreach (Brick brick in bricks)
        {
            brick.Inicializar(this);
        }
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
            if(puntuacion > 50)
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
