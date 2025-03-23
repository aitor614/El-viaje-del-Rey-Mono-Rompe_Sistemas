using UnityEngine;

public class Ball : MonoBehaviour
{

    public Rigidbody2D rigidBody2D;

    public float speed = 300;

    private Vector2 velocity;

    private Vector2 startPosition;

    private GameManager gameManager;


    void Start()
    {
        
        gameManager = FindFirstObjectByType<GameManager>();

        startPosition = transform.position;
        ResetBall();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            gameManager.looseHealth();

        }
    }
    public void ResetBall()
    {
        transform.position = startPosition;

        rigidBody2D.linearVelocity = Vector2.zero;
        rigidBody2D.angularVelocity = 0;

        float randomX = Random.Range(-1f, 1f);
        Vector2 direction = new Vector2(randomX, 1).normalized;

        rigidBody2D.linearVelocity = direction * speed * Time.fixedDeltaTime;
    }

}
