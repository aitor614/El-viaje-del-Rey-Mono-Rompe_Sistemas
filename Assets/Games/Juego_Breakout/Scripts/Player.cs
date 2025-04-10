using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rigiBody2D;

    private float imputValue;

    public float moveSpeed = 25;

    private Vector2 direction;
    private Vector2 startPosition;
    
    private ControlBreakout control;

    private void Start()
    {
        startPosition = transform.position;
        control = ControlBreakout.InstanciaControl;
    }

    // Update is called once per frame
    private void Update()
    {
        imputValue = Input.GetAxisRaw("Horizontal");

        if (imputValue == 1)
        {
            direction = Vector2.right;
        }
        else if ( imputValue == -1)
        {
            direction = Vector2.left;
        }
        else
        {
            direction = Vector2.zero;
        }

        rigiBody2D.AddForce(100 * moveSpeed * Time.deltaTime * direction);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el jugador colisiona con la bola no le afecta
        if (collision.gameObject.CompareTag("Ball"))
        {
            direction = Vector2.zero;
            rigiBody2D.linearVelocity = Vector2.zero;
            rigiBody2D.angularVelocity = 0;
            rigiBody2D.AddForce(100 * moveSpeed * Time.deltaTime * direction);
        }
    }

    public void ResetPlayer()
    {
        transform.position = startPosition;

        rigiBody2D.linearVelocity = Vector2.zero;
    }


}
