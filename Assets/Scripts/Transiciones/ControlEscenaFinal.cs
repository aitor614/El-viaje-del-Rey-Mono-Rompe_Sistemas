using TMPro;
using UnityEngine;

public class ControlEscenaFinal : MonoBehaviour
{
    [Header("Elementos de la escena")]
    public TextMeshProUGUI TxtScore;
    private ControlMenuPrincipal controlMenu;

    [Header("Sonidos")]
    public AudioClip musica;
    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controlMenu = ControlMenuPrincipal.InstanciaControl;
        TxtScore.text = "SCORE " + PlayerPrefs.GetInt("Puntuacion");
        audioSource.clip = musica;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();
    }

    public void Click_BtnContinuar()
    {
        controlMenu.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Menu);
    }
}
