using TMPro;
using UnityEngine;

public class ControlEscenaFinal : MonoBehaviour
{
    [Header("Elementos de la escena")]
    public TextMeshProUGUI TxtScore;
    private ControlMenuPrincipal controlMenuPrincipal;

    [Header("Sonidos")]
    public AudioClip musica;
    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        controlMenuPrincipal = ControlMenuPrincipal.InstanciaControl;
        TxtScore.text = "SCORE " + PlayerPrefs.GetInt("Puntuacion");
        audioSource.clip = musica;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        if (controlMenuPrincipal != null) audioSource.volume = controlMenuPrincipal.volumenMusica;
        audioSource.Play();
    }

    public void Click_BtnContinuar()
    {
        controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Menu);
    }
}
