using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 10f;

    public AudioClip bounceSound;
    public AudioClip boostSound;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    float movement = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f; // Ajusta aquí el volumen general
    }

    void Update()
    {
        movement = Input.GetAxis("Horizontal") * moveSpeed;

        // Mirar hacia la dirección de movimiento
        if (movement > 0)
        {
            sr.flipX = false;
        }
        else if (movement < 0)
        {
            sr.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = movement;
        rb.linearVelocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Comprobar que colisionó desde arriba
        foreach (ContactPoint2D contact in col.contacts)
        {
            // El contacto debe estar por debajo del jugador (o lo bastante plano)
            bool colisionPorDebajo = contact.normal.y > 0.5f;

            if (colisionPorDebajo)
            {
                if (col.collider.CompareTag("Platform"))
                {
                    if (bounceSound != null)
                        audioSource.PlayOneShot(bounceSound);
                }
                else if (col.collider.CompareTag("PlatformBoost"))
                {
                    if (boostSound != null)
                        audioSource.PlayOneShot(boostSound);
                }

                break; // solo necesita detectar una vez
            }
        }
    }


}
