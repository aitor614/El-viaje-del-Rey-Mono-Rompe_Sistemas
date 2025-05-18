using UnityEngine;
using UnityEngine.UI;

public class DañoScreen : MonoBehaviour
{
    public Image imagenDaño; 
    public float duracion = 0.5f;
    public AudioClip sonidoDaño; // arrastrar desde el Inspector
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void MostrarDaño()
    {
        if (sonidoDaño != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoDaño);
        }
        StopAllCoroutines(); 
        StartCoroutine(FlashDaño());
    }

    private System.Collections.IEnumerator FlashDaño()
    {
        imagenDaño.gameObject.SetActive(true);
        yield return new WaitForSeconds(duracion);
        imagenDaño.gameObject.SetActive(false);
    }
}
