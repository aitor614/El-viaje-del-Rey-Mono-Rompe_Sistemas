using UnityEngine;

public class ControlBordesPantalla : MonoBehaviour
{
    [Header("Bordes")]
    public Transform izquierda;
    public Transform derecha;
    public Transform arriba;
    public Transform abajo;

    [Header("Parámetros")]
    public float grosor;
    public float solapamiento;
    void Update()
    {
        Camera cam = Camera.main;
        float alturaCamara = 2f * cam.orthographicSize;
        float anchoCamara = alturaCamara * cam.aspect;

        Vector2 centro = cam.transform.position;

        float izquierdaX = centro.x - anchoCamara / 2f;
        float derechaX = centro.x + anchoCamara / 2f;
        float arribaY = centro.y + alturaCamara / 2f;
        float abajoY = centro.y - alturaCamara / 2f;

        // Asignar posiciones (sin desplazar más allá del borde visible)
        if (izquierda)
        {
            izquierda.position = new Vector3(izquierdaX - grosor / 2f, centro.y, izquierda.position.z);
            izquierda.localScale = new Vector2(grosor, alturaCamara + 2 * solapamiento);
            izquierda.rotation = Quaternion.identity;
        }

        if (derecha)
        {
            derecha.position = new Vector3(derechaX + grosor / 2f, centro.y, derecha.position.z);
            derecha.localScale = new Vector2(grosor, alturaCamara + 2 * solapamiento);
            derecha.rotation = Quaternion.identity;
        }

        if (arriba)
        {
            arriba.position = new Vector3(centro.x, arribaY + grosor / 2f, arriba.position.z);
            arriba.localScale = new Vector2(anchoCamara + 2 * solapamiento, grosor);
            arriba.rotation = Quaternion.identity;
        }

        if (abajo)
        {
            abajo.position = new Vector3(centro.x, abajoY - grosor / 2f, abajo.position.z);
            abajo.localScale = new Vector2(anchoCamara + 2 * solapamiento, grosor);
            abajo.rotation = Quaternion.identity;
        }
    }

}
