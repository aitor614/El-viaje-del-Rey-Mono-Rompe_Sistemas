using TMPro;
using UnityEngine;

public class ControlDerrota : MonoBehaviour
{
    public TextMeshProUGUI TxtScore;
    public TextMeshProUGUI TxtScorePartida;
    public ControlMenuPrincipal controlMenuPrincipal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controlMenuPrincipal = ControlMenuPrincipal.InstanciaControl;
        TxtScorePartida.text = "SCORE PARTIDA: " + PlayerPrefs.GetInt("PuntuacionPartida");
        TxtScore.text = "SCORE TOTAL: " + PlayerPrefs.GetInt("Puntuacion");
    }

    // Click en el botón de reset
    public void Click_BtnReset()
    {
        PlayerPrefs.SetInt("Puntuacion", PlayerPrefs.GetInt("Puntuacion") - PlayerPrefs.GetInt("PuntuacionPartida"));
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        PlayerPrefs.Save();
        controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Reiniciar);
    }

    // Click en el botón de salir
    public void Click_BtnMenu()
    {
        controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Menu);
    }
}