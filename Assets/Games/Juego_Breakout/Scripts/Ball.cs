using UnityEngine;

public class Ball : MonoBehaviour
{

    private Rigidbody2D rigidBody2D;

    public float speed = 300;

    private Vector2 velocity;

    private Vector2 startPosition;

    private ControlBreakout control;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void Start()
    {
        control = ControlBreakout.InstanciaControl;
        ResetBall();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si la bola colisiona contra cualquier objeto, tiende a bajar
        if (collision.gameObject.CompareTag("Brick"))
        {
            rigidBody2D.linearVelocity = new Vector2(rigidBody2D.linearVelocityX, -Mathf.Abs(rigidBody2D.linearVelocityY));
        }

        // Si la bola colisiona con el jugador, tiende a subir
        if (collision.gameObject.CompareTag("Player"))
        {
            rigidBody2D.linearVelocity = new Vector2(rigidBody2D.linearVelocityX, +Mathf.Abs(rigidBody2D.linearVelocityY));

        }


        // Si la bola cae en la zona de muerte, se pierde una vida
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            control.PerderVida();
        }
    }

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
