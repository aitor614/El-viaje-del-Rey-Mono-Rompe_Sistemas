using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BrickBase : MonoBehaviour
{
    private ControlBreakout control;

    [Header("Configuración")]
    public int vidas;
    public int puntosPorDestruir;
    public AudioClip sonidoDestruccion;
    public AudioClip sonidoToque;

    private AudioSource audioSource;

    private void Start()
    {
        control = ControlBreakout.InstanciaControl;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        vidas--;

        // Reproducir sonido de toque
        if (vidas > 0 && sonidoToque != null) audioSource.PlayOneShot(sonidoToque);

        if (vidas <= 0)
        {
            if (sonidoDestruccion != null)
            {
                AudioSource.PlayClipAtPoint(sonidoDestruccion, transform.position);
            }

            // Sumar puntos y ladrillos
            PlayerPrefs.SetInt("Ladrillos", PlayerPrefs.GetInt("Ladrillos") + 1);
            PlayerPrefs.SetInt("PuntuacionPartida", PlayerPrefs.GetInt("PuntuacionPartida") + puntosPorDestruir);
            PlayerPrefs.Save();

            Destroy(gameObject); // Destruir tras reproducir sonido
        }
    }
}
