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
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            control.LooseHealth();
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
