using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rigiBody2D;

    private float imputValue;

    public float moveSpeed = 25;

    private Vector2 direction;

    private Vector2 startPosition;

    public AudioClip hitSound;

    private void Start()
    {
        startPosition = transform.position;
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }
    }


}
