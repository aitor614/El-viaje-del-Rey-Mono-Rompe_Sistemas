using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public float moveSpeed = 2f;

    private float leftLimit;
    private float rightLimit;
    private bool movingRight = true;
    public RectTransform fondo;

    void Start()
    {
        // Obtener los bordes visibles de la cámara en el mundo
        //float leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        //float rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        // Obtener los límites del fondo
        Vector3[] esquinas = new Vector3[4];
        fondo.GetWorldCorners(esquinas);

        // Asignar los límites izquierdo y derecho de la plataforma
        float leftEdge = esquinas[0].x;
        float rightEdge = esquinas[2].x;

        // Ancho de la plataforma
        float halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;

        // Ajustar los límites reales
        leftLimit = leftEdge + halfWidth;
        rightLimit = rightEdge - halfWidth;
    }

    void Update()
    {
        float movement = moveSpeed * Time.deltaTime;

        if (movingRight)
            transform.position += new Vector3(movement, 0f, 0f);
        else
            transform.position -= new Vector3(movement, 0f, 0f);

        // Rebotar en los bordes reales
        if (transform.position.x >= rightLimit)
            movingRight = false;
        else if (transform.position.x <= leftLimit)
            movingRight = true;
    }
}