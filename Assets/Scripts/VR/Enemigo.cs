using System.Collections;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [Header("Componentes")]
    public Animator animacion;
    public Camera objetivo;

    [Header("Parámetros")]
    public int vida;
    public int poderGolpe;
    public int puntosEliminar;

    public float velocidad;
    public float offsetAlturaObjetivo;
    public float distanciaAtaque;


    [Header("Sonido")]
    public AudioClip sonidoAtaque;
    public AudioClip sonidoDamage;

    // Variables
    private bool ataqueRealizado = false;
    public GameObject efectoExplosion;
    private ControlBatalla controlBatalla;

    void Start()
    {
        controlBatalla = FindFirstObjectByType<ControlBatalla>();
        if(controlBatalla == null)
        {
            Debug.LogError("No se encontró el ControlBatalla en la escena.");
        }

    }

    void Update()
    {
        if (objetivo == null) return;

        // Calcula la posición corregida del objetivo con altura ajustable
        Vector3 posicionObjetivo = new(
            objetivo.transform.position.x,
            objetivo.transform.position.y + offsetAlturaObjetivo,
            objetivo.transform.position.z
        );

        float distance = Vector3.Distance(transform.position, posicionObjetivo);

        if (distance > distanciaAtaque)
        {
            Vector3 direccion = (posicionObjetivo - transform.position).normalized;

            transform.position += Time.deltaTime * velocidad * direccion;

            // Mira directamente al objetivo con la altura corregida
            transform.LookAt(posicionObjetivo);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Proyectil"))
        {
            Debug.Log("[Enemigo] Golpe proyectil.");
            // Instanciar la explosión en la posición del enemigo
            if (efectoExplosion != null)
            {
                var efecto = Instantiate(efectoExplosion, transform.position, Quaternion.identity);
                Destroy(efecto, 2f);

            }
            StartCoroutine(RecibirDamage());
            Destroy(collision.gameObject, 0.25f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !ataqueRealizado)
        {
            Debug.Log("[Enemigo] Colisión con el jugador, iniciando ataque...");
            ataqueRealizado = true;
            StartCoroutine(AtaqueEnemigo());
        }
    }

    IEnumerator AtaqueEnemigo()
    {
        animacion.Play("attack");
        yield return new WaitForSeconds(1f);
        if (sonidoAtaque != null)
        {
            AudioSource.PlayClipAtPoint(sonidoAtaque, transform.position);
        }
        animacion.Play("running");
        Debug.Log("[Enemigo] Ataque realizado.");
        Destroy(gameObject, 1f);
        controlBatalla.PerderVida(poderGolpe);
    }

    IEnumerator RecibirDamage()
    {
        var velocidadOriginal = velocidad;
        velocidad = 0; // Detener al enemigo al recibir daño

        Debug.Log("[Enemigo] Recibiendo daño.");
        animacion.Play("damage");
        // Esperar un breve momento para que la animación se reproduzca
        if (sonidoDamage != null)
        {
            AudioSource.PlayClipAtPoint(sonidoDamage, transform.position);
        }
        yield return new WaitForSeconds(0.5f);
        animacion.Play("running");
        RestarVida(1);
    }

    void RestarVida(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0)
        {


            // Actualizar puntuación y eliminar al enemigo
            PlayerPrefs.SetInt("PuntuacionPartida", PlayerPrefs.GetInt("PuntuacionPartida") + puntosEliminar);
            PlayerPrefs.SetInt("EnemigosEliminados", PlayerPrefs.GetInt("EnemigosEliminados") + 1);
            PlayerPrefs.Save();
            Destroy(gameObject);
        }
    }
}
