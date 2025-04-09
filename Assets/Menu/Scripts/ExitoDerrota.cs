using UnityEngine;
using static ControlMenuPrincipal;
using UnityEngine.SceneManagement;

public class ExitoDerrota : MonoBehaviour
{

    // Función para decidir qué hacer al presionar el botón "Continuar"
    public void Continuar()
    {
        ControlMenuPrincipal control = ControlMenuPrincipal.InstanciaControl;

        if (control.modoActual == ControlMenuPrincipal.ModoJuego.Continuo &&
            control.resultadoMinijuego == ResultadoMinijuego.Exito)
        {
            control.SiguienteMinijuego();
        }
        else
        {
            SceneManager.LoadScene("MenuPrincipal");
        }
    }

    // Función para salir al menu principal
    public void ReiniciarJuego()
    {
        ControlMenuPrincipal control = ControlMenuPrincipal.InstanciaControl;
        control.puntuacionTotal = 0;
        control.indiceActual = 0;
        control.resultadoMinijuego = ResultadoMinijuego.Exito;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
