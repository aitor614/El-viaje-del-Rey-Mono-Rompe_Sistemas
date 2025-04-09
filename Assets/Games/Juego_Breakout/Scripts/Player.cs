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
    }

    public void Inicializar(ControlBreakout controlBreakout)
    {
        control = controlBreakout;
    }


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

        rigiBody2D.AddForce(direction * moveSpeed * Time.deltaTime * 100);

    }

    public void ResetPlayer()
    {
        transform.position = startPosition;

        rigiBody2D.linearVelocity = Vector2.zero;
    }


}
