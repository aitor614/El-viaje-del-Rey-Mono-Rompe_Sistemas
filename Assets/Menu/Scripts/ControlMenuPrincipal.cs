using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenuPrincipal : MonoBehaviour
{
    public static ControlMenuPrincipal Instancia { get; private set; }

    public static int puntuacion;
    public enum ModoJuego { Individual, Continuo }
    public ModoJuego modoActual;

    public enum ResultadoMinijuego { Exito, Derrota }

    private string escenaActual = "";

    private int indiceActual = 0;
    public int puntuacionTotal = 0;

    private List<string> escenasMinijuegos = new()
    {
        "Minijuego1", "Minijuego2", "Minijuego3", "Minijuego4", "Minijuego5"
    };
    void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    public void JugarMinijuego(string nombreEscena)
    {
        modoActual = ModoJuego.Individual;
        SceneManager.LoadScene(nombreEscena);
    }

    public void JugarTodos()
    {
        modoActual = ModoJuego.Continuo;
        indiceActual = 0;
        puntuacionTotal = 0;
        SceneManager.LoadScene(escenasMinijuegos[indiceActual]);
    }

    public void SiguienteMinijuego()
    {
        indiceActual++;
        if (indiceActual < escenasMinijuegos.Count)
        {
            SceneManager.LoadScene(escenasMinijuegos[indiceActual]);
        }
        else
        {
            SceneManager.LoadScene("ResultadosFinales"); 
        }
    }

    public void SumarPuntos(int puntos)
    {
        puntuacionTotal += puntos;
    }

}


