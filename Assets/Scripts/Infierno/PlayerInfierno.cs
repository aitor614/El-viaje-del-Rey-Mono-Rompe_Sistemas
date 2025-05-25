using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInfierno : MonoBehaviour
{
    [Header("Controles")]
    public InputAction moverAccion;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip bounceSound;
    public AudioClip boostSound;
    public AudioClip colisionEnemigo;
    public AudioClip respawn;

    [Header("Componentes")]
    public Rigidbody2D rigiBody2D;
    public SpriteRenderer spritePlayer;

    [Header("Parámetros")]
    public float volumen = 0.5f;
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float moverSpeed;

    // Variables
    private Vector2 posUltimoToque = Vector2.zero;
    private float moverInput;
    private float alturaMaxima = 0f;

    void Start()
    {
        audioSource.volume = volumen;
    }

    private void Update()
    {
        moverInput = 0f; // Reiniciar cada frame

        if (transform.position.y > alturaMaxima)
            alturaMaxima = transform.position.y;

        MovimientoHorizontal();
        InvertirImagenX();
    }

    private void MovimientoHorizontal()
    {
        // Teclado
        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
                moverInput = -1f;
            else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
                moverInput = 1f;
        }

        // Touchscreen
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 posicionActual = Touchscreen.current.primaryTouch.position.ReadValue();
            float mitadPantalla = Screen.width / 2;
            // Si es la primera vez o ha cambiado de lado respecto al centro
            bool haCambiadoDeLado =
                (posUltimoToque.x < mitadPantalla && posicionActual.x >= mitadPantalla) ||
                (posUltimoToque.x >= mitadPantalla && posicionActual.x < mitadPantalla);

            if (posUltimoToque == Vector2.zero || haCambiadoDeLado)
            {
                // Dirección actual según la posición respecto al centro
                if (posicionActual.x < mitadPantalla)
                    moverInput = -1f;
                else
                    moverInput = 1f;

                // Actualizar última posición
                posUltimoToque = posicionActual;
            }
            else
            {
                // Mantener dirección anterior si no cambió de lado
                if (posUltimoToque.x < mitadPantalla)
                    moverInput = -1f;
                else
                    moverInput = 1f;
            }
        }
        else
        {
            // Reiniciar si no hay toque
            posUltimoToque = Vector2.zero; 
        }

        // Movimiento
        Vector3 direction = new(moverInput, 0f, 0f);
        transform.position += moverSpeed * Time.deltaTime * direction;
    }

    private void InvertirImagenX()
    {
        if (spritePlayer == null)
        {
            Debug.LogError("SpriteRenderer no asignado en el Inspector.");
            return;
        }
        // Mirar hacia la dirección de movimiento
        if (moverInput > 0)
        {
            spritePlayer.flipX = false;
        }
        else if (moverInput < 0)
        {
            spritePlayer.flipX = true;
        }
    }

    private void OnEnable()
    {
        moverAccion.Enable();
    }

    private void OnDisable()
    {
        moverAccion.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rigiBody2D.linearVelocity;
        velocity.x = moverInput;
        rigiBody2D.linearVelocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D colisionador)
    {
        Debug.Log("Colisión con: " + colisionador.collider.name);
        // Comprobar que colisionó desde arriba
        foreach (ContactPoint2D contact in colisionador.contacts)
        {
            // El contacto debe estar por debajo del jugador (o lo bastante plano)
            bool colisionPorDebajo = contact.normal.y > 0.01f;

            if (colisionPorDebajo)
            {
                if (colisionador.collider.CompareTag("Platform"))
                {
                    if (bounceSound != null) audioSource.PlayOneShot(bounceSound);
                }
                else if (colisionador.collider.CompareTag("PlatformBoost"))
                {
                    if (boostSound != null) audioSource.PlayOneShot(boostSound);
                }
                else if (colisionador.collider.CompareTag("FinalPlatform"))
                {
                    if (boostSound != null) audioSource.PlayOneShot(boostSound);
                    PlayerPrefs.SetInt("ObjetoInfierno", 1);
                    PlayerPrefs.Save();
                }

                break;
            }
        }

        // Si colisiona con cualquier borde, no avanza
        if (colisionador.collider.CompareTag("Wall"))
        {
            moverInput = -moverInput;
            rigiBody2D.linearVelocityX = 0f;
        }

        if (colisionador.collider.CompareTag("DeadZone"))
        {
            PlayerPrefs.SetInt("VidasRestantes", PlayerPrefs.GetInt("VidasRestantes") - 1);
            PlayerPrefs.Save();
            ResetPlayer();
        }

        if (colisionador.collider.CompareTag("Enemy"))
        {
            Debug.Log("¡Colisión con enemigo!");

            // Restar una vida
            PlayerPrefs.SetInt("VidasRestantes", PlayerPrefs.GetInt("VidasRestantes") - 1);

            // Restar puntos (evitar que sea negativo)
            int puntosActuales = PlayerPrefs.GetInt("PuntuacionPartida");
            int puntosARestar = 50;
            puntosActuales = Mathf.Max(0, puntosActuales - puntosARestar);
            PlayerPrefs.SetInt("PuntuacionPartida", PlayerPrefs.GetInt("PuntuacionPartida"));
            PlayerPrefs.Save();

            // Destruir el enemigo
            Destroy(colisionador.gameObject);

            audioSource.PlayOneShot(colisionEnemigo);

            // Resetear al jugador
            ResetPlayer();
        }
    }

    public void ResetPlayer()
    {
        Vector3 posicionCamara = Camera.main.transform.position;

        // Posición central en X, altura máxima alcanzada en Y, misma Z que antes
        transform.position = new Vector3(posicionCamara.x, alturaMaxima, transform.position.z);

        rigiBody2D.linearVelocity = Vector2.zero;

        audioSource.PlayOneShot(respawn);
    }

}
