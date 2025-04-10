using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Player : MonoBehaviour
{
    public Rigidbody2D rigiBody2D;
    public InputActionAsset inputActions;
    public InputAction moverAccion;
    
    private float moverInput = 0f;
    public float moveSpeed = 2.5f;

    private Vector2 direction;
    private Vector2 startPosition;

    private Vector3 targetPosition;
    private bool hasTarget = false;

    private ControlBreakout control;

    private void Start()
    {
        startPosition = transform.position;
        control = ControlBreakout.InstanciaControl;
    }

    private void OnEnable()
    {
        moverAccion.Enable();
    }

    private void OnDisable()
    {
        moverAccion.Disable();
    }

    // Update is called once per frame
    private void Update()
    {


        moverInput = 0f; // Reiniciar cada frame

        // --- TECLADO ---
        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
                moverInput = -1f;
            else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
                moverInput = 1f;
        }

        // --- TOUCH ---
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();

            // Comparar con la mitad de la pantalla
            if (touchPos.x < Screen.width / 2)
                moverInput = -1f;
            else
                moverInput = 1f;
        }

        // --- MOVER ---
        Vector3 direction = new(moverInput, 0f, 0f);
        transform.position += moveSpeed * Time.deltaTime * direction;
    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el jugador colisiona con la bola no le afecta
        if (collision.gameObject.CompareTag("Ball"))
        {
            //direction = Vector2.zero;
            //rigiBody2D.linearVelocity = Vector2.zero;
            //rigiBody2D.angularVelocity = 0;
            //rigiBody2D.AddForce(100 * moveSpeed * Time.deltaTime * direction);
        }
    }

    public void ResetPlayer()
    {
        transform.position = startPosition;

        rigiBody2D.linearVelocity = Vector2.zero;
    }


}
