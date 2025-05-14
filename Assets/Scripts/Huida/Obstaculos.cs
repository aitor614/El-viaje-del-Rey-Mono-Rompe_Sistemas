using UnityEngine;

public class Obstaculos : MonoBehaviour
{
    public float velocidad = 5f;
    private float bordeIzquierdo;

    private void Start()
    {
        // Calcula el borde izquierdo de la pantalla en coordenadas del mundo
        bordeIzquierdo = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 11f;
    }

    private void Update()
    {
        // Mueve el obstáculo hacia la izquierda
        transform.position += Time.deltaTime * velocidad * Vector3.left;

        // Si el objeto sale de la pantalla por la izquierda, se destruye
        if (transform.position.x < bordeIzquierdo)
        {
            Destroy(gameObject);
        }
    }
}
