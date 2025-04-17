using TMPro;
using UnityEngine;

public class ControlEscenaFinal : MonoBehaviour
{
    public TextMeshProUGUI TxtScore;
    private ControlMenuPrincipal controlMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controlMenu = ControlMenuPrincipal.InstanciaControl;
        TxtScore.text = "SCORE " + PlayerPrefs.GetInt("Puntuacion");
    }

    public void Click_BtnContinuar()
    {
        controlMenu.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Menu);
    }
}
