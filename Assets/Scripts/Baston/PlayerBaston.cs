using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerBaston : MonoBehaviour
{
    [Header("Controles")]
    public InputActionAsset inputActions;
    public InputAction moverAccion;

    [Header("Sonidos")]
    public AudioClip lanzarBola;
    public AudioClip respawn;

    [Header("Componentes")]
    public SpriteRenderer spritePlayer;
    public Rigidbody2D rigiBody2D;

    [Header("Parámetros")]
    public float velocidadMovimiento;

    // Variables
    private Vector2 posUltimoToque = Vector2.zero;
    private Vector2 direction;
    private Vector2 posicionInicial;
    private float moverInput;
    private AudioSource audioSource;

    private void Start()
    {
        posicionInicial = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        moverAccion.Enable();
    }

    private void OnDisable()
    {
        moverAccion.Disable();
    }

    private void Update()
    {
        moverInput = 0f; // Reiniciar cada frame
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
        transform.position += velocidadMovimiento * Time.deltaTime * direction;
    }

    // Invertir la imagen del jugador según la dirección de movimiento
    private void InvertirImagenX()
    {
        if (spritePlayer == null)
        {
            Debug.LogError("SpriteRenderer no asignado en el Inspector.");
            return;
        }

        // Mirar hacia la dirección de movimiento
        if ( moverInput > 0)
        {
            spritePlayer.flipX = false;
        }
        else if (moverInput < 0)
        {
            spritePlayer.flipX = true;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        
        if (collision.gameObject.CompareTag("Ball"))
        {
            audioSource.PlayOneShot(lanzarBola);
        }
        

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colisión de " + name + "con: " + other.gameObject.name);
        // Si el jugador colisiona con una ventaja de vida
        if (other.gameObject.CompareTag("VentajaVidaCaida"))
        {
            Debug.Log("¡Vida recogida!");
            ControlGolpe.InstanciaControl.ColisionVida(other);
        }

    }


    // Reinicia la posición del jugador
    public void ResetPlayer()
    {
        transform.position = posicionInicial;
        rigiBody2D.linearVelocity = Vector2.zero;
        audioSource.PlayOneShot(respawn);
    }


}
