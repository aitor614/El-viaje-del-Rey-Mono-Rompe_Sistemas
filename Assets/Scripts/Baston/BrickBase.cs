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

    [Header("Sprites")]
    public Sprite[] spritesPorEstado;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        control = ControlBreakout.InstanciaControl;
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spritesPorEstado.Length > 0)
            spriteRenderer.sprite = spritesPorEstado[0];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        vidas--;

        if (vidas > 0)
        {
            // Reproducir sonido de toque
            if (sonidoToque != null)
                audioSource.PlayOneShot(sonidoToque);

            // Si hay un sprite disponible para este nivel de daño, lo cambiamos
            int indexSprite = Mathf.Clamp(spritesPorEstado.Length - vidas, 0, spritesPorEstado.Length - 1);
            if (spritesPorEstado.Length > 1 && spritesPorEstado[indexSprite] != null)
                spriteRenderer.sprite = spritesPorEstado[indexSprite];
        }
        else
        {
            if (sonidoDestruccion != null)
                AudioSource.PlayClipAtPoint(sonidoDestruccion, transform.position);

            // Sumar puntos y ladrillos
            PlayerPrefs.SetInt("Ladrillos", PlayerPrefs.GetInt("Ladrillos") + 1);
            PlayerPrefs.SetInt("PuntuacionPartida", PlayerPrefs.GetInt("PuntuacionPartida") + puntosPorDestruir);
            PlayerPrefs.Save();

            Destroy(gameObject); // Destruir tras reproducir sonido
        }
    }
}
