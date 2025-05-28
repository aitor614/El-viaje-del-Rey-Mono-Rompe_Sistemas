using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ControlExito : MonoBehaviour
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
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        controlMenuPrincipal = ControlMenuPrincipal.InstanciaControl;
        TxtScorePartida.text = "SCORE PARTIDA: " + PlayerPrefs.GetInt("PuntuacionPartida");
        TxtScore.text = "SCORE TOTAL: " + PlayerPrefs.GetInt("Puntuacion");

        audioSource.clip = musica;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        if (controlMenuPrincipal != null) audioSource.volume = controlMenuPrincipal.volumenMusica;
        audioSource.Play();
    }

    // Click en el botón de continuar
    public void Click_BtnContinuar()
    {

        if (controlMenuPrincipal.modoActual == ControlMenuPrincipal.ModoJuego.Continuo &&
            controlMenuPrincipal.resultadoMinijuego == ControlMenuPrincipal.ResultadoMinijuego.Exito)
        {
            controlMenuPrincipal.SiguienteMinijuego();
        }
        else
        {
            SceneManager.LoadScene("MenuPrincipal");
        }
    }
}
