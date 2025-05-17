using UnityEngine;
using UnityEngine.UI;

public class DañoScreen : MonoBehaviour
{
    public Image imagenDaño; 
    public float duracion = 1f;

    public void MostrarDaño()
    {
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
