using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Configuración de la bola")]
    public float speed;

    /// Variables
    private Rigidbody2D rigidBody2D;
    private Vector2 startPosition;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startPosition = transform.position;
        ResetBall();
    }

    private void Update()
    {

    }

    void FixedUpdate()
    {
        Vector2 velocity = rigidBody2D.linearVelocity;

        float velocidadActual = velocity.magnitude;
        float velocidadMinima = speed; 

        if (velocidadActual < velocidadMinima)
        {
            // Mantenemos la dirección, reforzamos la magnitud
            Vector2 direccion = velocity.normalized;

            rigidBody2D.linearVelocity = direccion * velocidadMinima;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Se obtiene la dirección de la bola
        Vector2 dir = rigidBody2D.linearVelocity.normalized;

        // Corrige trayectorias demasiado verticales
        if (Mathf.Abs(dir.y) > 0.95f && Mathf.Abs(dir.x) < 0.05f)
        {
            dir.x = Random.Range(0.2f, 0.4f) * Mathf.Sign(Random.Range(-1, 1));
            dir.Normalize();
        }

        // Si la bola colisiona contra cualquier objeto, tiende a bajar
        if (collision.gameObject.CompareTag("Brick"))
        {
            rigidBody2D.linearVelocity = new Vector2(rigidBody2D.linearVelocityX, -Mathf.Abs(rigidBody2D.linearVelocityY));
        }


        // Si la bola colisiona con el jugador, tiende a subir
        if (collision.gameObject.CompareTag("Player"))
        {
            // Se calcula la dirección de rebote en función de la posición del jugador
            float offset = transform.position.x - collision.transform.position.x;

            // Se calcula el ancho del jugador
            float width = collision.collider.bounds.size.x / 2;

            // Se normaliza el offset entre -1 y 1
            float normalizedOffset = offset / width;

            // Se calcula la nueva dirección de la bola
            Vector2 nuevaDireccion = new Vector2(normalizedOffset, 1f).normalized;

            // Se aplica la nueva dirección a la bola
            rigidBody2D.linearVelocity = nuevaDireccion * rigidBody2D.linearVelocity.magnitude;

        }


        // Si la bola cae en la zona de muerte, se pierde una vida
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            PlayerPrefs.SetInt("VidasRestantes", PlayerPrefs.GetInt("VidasRestantes") - 1);
            PlayerPrefs.Save();
        }
    }

    // Resetea la posición de la bola y su velocidad
    public void ResetBall()
    {
        transform.position = startPosition;

        rigidBody2D.linearVelocity = Vector2.zero;
        rigidBody2D.angularVelocity = 0;

        float randomX = Random.Range(-1f, 1f);
        Vector2 direction = new Vector2(randomX, 1).normalized;

        rigidBody2D.linearVelocity = speed * Time.fixedDeltaTime * direction;
    }

}
