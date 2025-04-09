using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenuPrincipal : MonoBehaviour
{
    public static ControlMenuPrincipal InstanciaControl { get; private set; }

    public static int puntuacion;
    public enum ModoJuego { Individual, Continuo }
    public ModoJuego modoActual;

    public enum ResultadoMinijuego { Exito, Derrota, Reiniciar, Menu }
    public ResultadoMinijuego resultadoMinijuego;

    private string escenaActual = "";

    public int indiceActual = 0;
    public int puntuacionTotal = 0;

    // Lista de escenas de minijuegos
    private readonly List<string> escenasMinijuegos = new()
    {
        "Juego2DEscapeInfierno", 
        "Juego2DHuidaCelestial", 
        "Juego2DGolpeBaston", 
        "JuegoAREspiritusDesencarnados", 
        "JuegoVRBatallaCelestial"
    };

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        resultadoMinijuego = ResultadoMinijuego.Menu;
    }

    // Funcion para inicializar el singleton (instancia de ControlMenuPrincipal)
    void Awake()
    {
        if (InstanciaControl != null && InstanciaControl != this)
        {
            Destroy(gameObject);
            return;
        }

        InstanciaControl = this;
        DontDestroyOnLoad(gameObject);
    }

    // Funcion para cargar la escena del minijuego
    public void JugarMinijuego(string nombreEscena)
    {
        modoActual = ModoJuego.Individual;
        escenaActual = nombreEscena;
        SceneManager.LoadScene(nombreEscena);
    }

    public void Click_JuegoInfierno()
    {
        // Cargar la escena del juego completo
        JugarMinijuego("Juego2DEscapeInfierno");
    }

    public void Click_JuegoHuida()
    {
        // Cargar la escena del juego completo
        JugarMinijuego("Juego2DHuidaCelestial");
    }

    public void Click_JuegoBaston()
    {
        // Cargar la escena del juego completo
        JugarMinijuego("Juego2DGolpeBaston");
    }

    public void Click_JuegoEspiritus()
    {
        // Cargar la escena del juego completo
        JugarMinijuego("JuegoAREspiritusDesencarnados");
    }

    public void Click_JuegoVR()
    {
        // Cargar la escena del juego completo
        JugarMinijuego("JuegoVRBatallaCelestial");
    }

    public void Click_SalirJuego()
    {
        // Salir del juego
        Application.Quit();
    }

    // Funcion para jugar todos los minijuegos en modo continuo
    public void Click_JugarTodos()
    {
        modoActual = ModoJuego.Continuo;
        indiceActual = 0;
        puntuacionTotal = 0;
        SceneManager.LoadScene(escenasMinijuegos[indiceActual]);
    }

    // Funcion para cargar el siguiente minijuego en modo continuo
    public void SiguienteMinijuego()
    {
        indiceActual++;
        if (indiceActual < escenasMinijuegos.Count)
        {
            // Cargar la siguiente escena de minijuego
            escenaActual = escenasMinijuegos[indiceActual];
            SceneManager.LoadScene(escenasMinijuegos[indiceActual]);
        }
        else
        {
            // Si se han completado todos los minijuegos, cargar la escena final
            SceneManager.LoadScene("EscenaFinal"); 
        }
    }

    // Funcion para sumar puntos desde los minijuegos
    public void SumarPuntos(int puntos)
    {
        puntuacionTotal += puntos;
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
            SceneManager.LoadScene(escenaActual);
        }
        else if (resultado == ResultadoMinijuego.Menu)
        {
            // Volver al menú principal
            SceneManager.LoadScene("MenuPrincipal");
        }
    }

}


