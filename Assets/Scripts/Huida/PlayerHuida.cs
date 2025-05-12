using System;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerHuida : MonoBehaviour
{
    public Sprite[] sprites;
    public float strength = 5f;
    public float gravity = -9.81f;
    public float tilt = 5f;

    [Header("Componentes")]
    public Rigidbody2D rigiBody2D;

    [Header("Sonidos")]
    public AudioClip salto;
    public AudioClip respawn;

    private SpriteRenderer spriteRenderer;
    private Vector3 direction;
    private Vector2 posicionInicial;
    private int spriteIndex;
    private AudioSource audioSource;

    public float fuerzaSalto = 7f; //añadido aitor


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        posicionInicial = transform.position;
        rigiBody2D = GetComponent<Rigidbody2D>();
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }


    //modificado aitor
    private void Update()
    {
        bool salto = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);

     #if UNITY_ANDROID || UNITY_IOS
        salto = salto || Input.touchCount > 0;
     #endif

        if (salto)
        {
            Debug.Log("Salto pulsado");

            // Reinicia velocidad para evitar acumulación de fuerza
            rigiBody2D.linearVelocity = Vector2.zero;

            // Aplica el salto
            rigiBody2D.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);

            // Reproduce sonido si está asignado
            if (this.salto != null) audioSource.PlayOneShot(this.salto);
        }

        //ControlMovimiento();

    }
    /*
     
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

     
     */

    private void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }

        if (spriteIndex < sprites.Length && spriteIndex >= 0)
        {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {

             // Resta una vida
            int vidasActuales = PlayerPrefs.GetInt("VidasRestantes");
            vidasActuales = Mathf.Max(vidasActuales - 1, 0); // Evita negativos
            PlayerPrefs.SetInt("VidasRestantes", vidasActuales);
            PlayerPrefs.Save();


            ControlHuida.InstanciaControl.GameOver();
        }
        else if (other.gameObject.CompareTag("Scoring"))
        {
            PlayerPrefs.SetInt("PuntuacionActual", PlayerPrefs.GetInt("PuntuacionActual") + 5);
            PlayerPrefs.SetInt("ObstaculosSalvados", PlayerPrefs.GetInt("ObstaculosSalvados") + 1);
            PlayerPrefs.Save();
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