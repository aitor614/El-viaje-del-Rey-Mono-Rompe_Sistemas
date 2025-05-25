using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
    public float jumpForce;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.relativeVelocity.y<= 0f)
        {
            if (collision.collider.TryGetComponent<Rigidbody2D>(out var rb))
            {
                Vector2 velocity = rb.linearVelocity;
                velocity.y = jumpForce;
                rb.linearVelocity = velocity;
            }
        }
        
    }
}
