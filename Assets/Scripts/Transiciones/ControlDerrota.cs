using TMPro;
using UnityEngine;

public class ControlDerrota : MonoBehaviour
{
    [Header("Elementos de la escena")]
    public TextMeshProUGUI TxtScore;
    public TextMeshProUGUI TxtScorePartida;
    [Header("Sonidos")]
    public AudioClip musica;
    public AudioSource audioSource;

    private ControlMenuPrincipal controlMenuPrincipal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controlMenuPrincipal = ControlMenuPrincipal.InstanciaControl;
        TxtScorePartida.text = "SCORE PARTIDA: " + PlayerPrefs.GetInt("PuntuacionPartida");
        TxtScore.text = "SCORE TOTAL: " + PlayerPrefs.GetInt("Puntuacion");

        audioSource.clip = musica;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();
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