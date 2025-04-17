using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlExito : MonoBehaviour
{
    public TextMeshProUGUI TxtScore;
    public TextMeshProUGUI TxtScorePartida;
    private ControlMenuPrincipal controlMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controlMenu = ControlMenuPrincipal.InstanciaControl;
        TxtScorePartida.text = "SCORE PARTIDA: " + PlayerPrefs.GetInt("PuntuacionPartida");
        TxtScore.text = "SCORE TOTAL: " + PlayerPrefs.GetInt("Puntuacion");
    }

    // Click en el botón de continuar
    public void Click_BtnContinuar()
    {

        if (controlMenu.modoActual == ControlMenuPrincipal.ModoJuego.Continuo &&
            controlMenu.resultadoMinijuego == ControlMenuPrincipal.ResultadoMinijuego.Exito)
        {
            controlMenu.SiguienteMinijuego();
        }
        else
        {
            SceneManager.LoadScene("MenuPrincipal");
        }
    }
}
