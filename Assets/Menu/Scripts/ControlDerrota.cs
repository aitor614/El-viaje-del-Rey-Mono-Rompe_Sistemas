using TMPro;
using UnityEngine;

public class ControlDerrota : MonoBehaviour
{
    public TextMeshProUGUI TxtScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int puntos = ControlMenuPrincipal.InstanciaControl.puntuacionTotal;
        TxtScore.text = "SCORE " + puntos;
    }

    public void Click_BtnReset()
    {
        ControlMenuPrincipal control = ControlMenuPrincipal.InstanciaControl;
        control.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Reiniciar);
    }

    public void Click_BtnMenu()
    {
        ExitoDerrota exitoDerrota = FindFirstObjectByType<ExitoDerrota>();
        exitoDerrota.Continuar();
    }
}