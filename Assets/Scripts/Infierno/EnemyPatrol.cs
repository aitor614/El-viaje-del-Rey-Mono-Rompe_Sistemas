using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float minSpeed = 1.5f;
    public float maxSpeed = 3.5f;

    private float speed;
    private int direction; // -1 izquierda, 1 derecha
    private SpriteRenderer spriteRenderer;

    private float leftLimit;
    private float rightLimit;
    private float offset = 0.5f; // margen de seguridad para que no se salga

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Dirección aleatoria
        direction = Random.value < 0.5f ? -1 : 1;
        spriteRenderer.flipX = direction < 0;

        // Velocidad aleatoria
        speed = Random.Range(minSpeed, maxSpeed);

        // Calcular límites de la pantalla
        leftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + offset;
        rightLimit = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - offset;
    }

    void Update()
    {
        // Movimiento
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Limitar por los bordes
        if (transform.position.x <= leftLimit || transform.position.x >= rightLimit)
        {
            direction *= -1;
            spriteRenderer.flipX = direction < 0;
        }
    }
}
