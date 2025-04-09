using TMPro;
using UnityEngine;

public class ControlEscenaFinal : MonoBehaviour
{
    public TextMeshProUGUI TxtScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int puntos = ControlMenuPrincipal.InstanciaControl.puntuacionTotal;
        TxtScore.text = "SCORE " + puntos;
    }

    public void Click_BtnContinuar()
    {
        ExitoDerrota exitoDerrota = FindFirstObjectByType<ExitoDerrota>();
        exitoDerrota.Continuar();
    }
}
