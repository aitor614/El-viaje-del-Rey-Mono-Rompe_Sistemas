using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [Header("Controles")]
    private ControlInfierno controlInfierno;
    public Transform player;

    private bool vidaRestada = false;
    float limiteInferior = 6f;

    private void Awake()
    {
        controlInfierno = ControlInfierno.Instancia;
        player = controlInfierno.player.transform;
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("El jugador no está asignado en el script DeathZone.");
            return;
        }

        if (!vidaRestada && player.position.y < Camera.main.transform.position.y - limiteInferior)
        {
            PlayerPrefs.SetInt("VidasRestantes", PlayerPrefs.GetInt("VidasRestantes") - 1);
            PlayerPrefs.SetInt("PlayerCaido", 1);
            PlayerPrefs.Save();
        }

        // Reiniciar flag si el jugador vuelve a una posición segura (por encima del límite)
        if (vidaRestada && player.position.y > limiteInferior + 1f)
        {
            vidaRestada = false;
        }
    }
}