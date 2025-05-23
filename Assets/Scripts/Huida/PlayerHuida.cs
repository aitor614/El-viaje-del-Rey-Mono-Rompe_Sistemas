using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerHuida : MonoBehaviour
{
    public Sprite[] sprites;
    public float strength = 5f;
    public float gravity = -9.81f;
    public float tilt = 5f;



    [Header("Sonidos")]
    public AudioClip salto;
    public AudioClip colision;

    [Header("Componentes")]
    public AudioSource audioSource;
    public Rigidbody2D colisionPlayer;
    public SpriteRenderer imagenPlayer;
    private Vector3 direction;
    private Vector2 posicionInicial;

    private void Start()
    {
        posicionInicial = transform.position;
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }

    private void Update()
    {

        // Click izquierdo del ratón en editor
#if UNITY_EDITOR
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Salto();
        }
#endif
        // Tocar la pantalla en móvil
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Salto();
        }

        ControlMovimiento();

    }

    // Salto del sprite
    public void Salto()
    {
        direction = Vector3.up * strength;
        audioSource.PlayOneShot(salto);
    }

    // Controla el movimiento del jugador
    private void ControlMovimiento()
    {
        // Aplicar gravedad y actualizar la posición
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;

        // Aplicar la rotación en función de la dirección
        Vector3 rotation = transform.eulerAngles;
        rotation.z = direction.y * tilt;
        transform.eulerAngles = rotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el jugador colisiona con un obstáculo
        if (other.gameObject.CompareTag("Obstacle"))
        {
            ControlHuida.InstanciaControl.Colision();
            audioSource.PlayOneShot(colision);
        }
        // Si el jugador supera el obstáculo
        else if (other.gameObject.CompareTag("Scoring"))
        {
            PlayerPrefs.SetInt("PuntuacionPartida", PlayerPrefs.GetInt("PuntuacionPartida") + 5);
            PlayerPrefs.SetInt("ObstaculosSalvados", PlayerPrefs.GetInt("ObstaculosSalvados") + 1);
            PlayerPrefs.Save();
        }
        else if (other.gameObject.CompareTag("VidaExxtra"))
        {
            ControlHuida.InstanciaControl.ColisionVida();
        }
    }

    // Reinicia la posición del jugador
    public void ResetPlayer()
    {
        transform.position = posicionInicial;
        colisionPlayer.linearVelocity = Vector2.zero;
    }
}
   