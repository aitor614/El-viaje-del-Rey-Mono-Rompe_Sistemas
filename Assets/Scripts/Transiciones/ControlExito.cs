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

    private ControlMenuPrincipal controlMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        controlMenu = ControlMenuPrincipal.InstanciaControl;
        TxtScorePartida.text = "SCORE PARTIDA: " + PlayerPrefs.GetInt("PuntuacionPartida");
        TxtScore.text = "SCORE TOTAL: " + PlayerPrefs.GetInt("Puntuacion");

        audioSource.clip = musica;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();
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
