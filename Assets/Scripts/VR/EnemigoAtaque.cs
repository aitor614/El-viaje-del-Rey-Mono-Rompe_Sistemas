using System.Collections;
using UnityEngine;

public class EnemigoAtaque : MonoBehaviour
{
    [Header("Componentes")]
    public Transform player;             // Referencia al jugador
    public Animator animacion;           // Animator del enemigo

    [Header("Parámetros")]
    public float distanciaAtaque = 1.5f;  // Distancia para iniciar ataque
    public int vida = 3;                 // Vida inicial del enemigo
    public int poderGolpe = 1;           // Daño que hace al jugador
    public int puntosEliminar = 100;     // Puntos al eliminar enemigo

    private bool ataqueRealizado = false;
    private bool atacando = false;

    void Update()
    {
        if (player == null) return;

        float distancia = Vector3.Distance(transform.position, player.position);
        Debug.Log($"Distancia al jugador: {distancia}");

        if (distancia <= distanciaAtaque && !ataqueRealizado)
        {
            if (!atacando)
            {
                animacion.SetBool("isAttacking", true);
                atacando = true;
            }
        }
        else
        {
            if (atacando)
            {
                animacion.SetBool("isAttacking", false);
                atacando = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Proyectil"))
        {
            RestarVida(1);
        }

        if (collision.gameObject.CompareTag("Player") && !ataqueRealizado)
        {
            ataqueRealizado = true;
            animacion.SetBool("isAttacking", true);
            Debug.Log("¡El enemigo ataca!");

            StartCoroutine(RealizarAtaqueConRetraso());
        }
    }

    private IEnumerator RealizarAtaqueConRetraso()
    {
        yield return new WaitForSeconds(0.5f);

        int vidas = PlayerPrefs.GetInt("VidasRestantes", 3); // Valor por defecto
        vidas -= poderGolpe;
        PlayerPrefs.SetInt("VidasRestantes", vidas);
        PlayerPrefs.Save();

        Destroy(gameObject, 1f);
    }

    void RestarVida(int cantidad)
    {
        vida -= cantidad;

        if (vida <= 0)
        {
            int puntos = PlayerPrefs.GetInt("PuntuacionPartida", 0);
            int eliminados = PlayerPrefs.GetInt("EnemigosEliminados", 0);

            PlayerPrefs.SetInt("PuntuacionPartida", puntos + puntosEliminar);
            PlayerPrefs.SetInt("EnemigosEliminados", eliminados + 1);
            PlayerPrefs.Save();

            Destroy(gameObject);
        }
    }
}

/*
using System.Collections;
using UnityEngine;

public class EnemigoAtaque: MonoBehaviour
{
    [Header("Componentes")]
    public Transform player;             // Referencia al jugador
    public Animator animacion;           // Animator del enemigo

    [Header("Parámetros")]
    public float velocidad;              // No se usa aquí, pero útil si luego se mueve
    public int vida = 3;                 // Vida inicial del enemigo
    public int poderGolpe = 1;           // Cuánto daño hace al jugador
    public int puntosEliminar = 100;     // Puntos al eliminar al enemigo

    // Variables internas
    private bool ataqueRealizado = false;

    void Update()
    {
        if (player == null) return;

        float distancia = Vector3.Distance(transform.position, player.position);

        // Si está cerca del jugador, reproducir animación de ataque
        if (distancia <= 2f && !ataqueRealizado)
        {
            animacion.SetBool("isAttacking", true);
        }
        else
        {
            animacion.SetBool("isAttacking", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Proyectil"))
        {
            RestarVida(1);
        }

        if (collision.gameObject.CompareTag("Player") && !ataqueRealizado)
        {
            ataqueRealizado = true;
            animacion.SetBool("isAttacking", true);
            Debug.Log("¡El enemigo ataca!");

            // Esperar medio segundo para causar daño y permitir ver la animación
            StartCoroutine(RealizarAtaqueConRetraso());
        }
    }

    private IEnumerator RealizarAtaqueConRetraso()
    {
        yield return new WaitForSeconds(0.5f);

        int vidas = PlayerPrefs.GetInt("VidasRestantes", 3); // Valor por defecto
        vidas -= poderGolpe;
        PlayerPrefs.SetInt("VidasRestantes", vidas);
        PlayerPrefs.Save();

        Destroy(gameObject, 1f);
    }

    void RestarVida(int cantidad)
    {
        vida -= cantidad;

        if (vida <= 0)
        {
            int puntos = PlayerPrefs.GetInt("PuntuacionPartida", 0);
            int eliminados = PlayerPrefs.GetInt("EnemigosEliminados", 0);

            PlayerPrefs.SetInt("PuntuacionPartida", puntos + puntosEliminar);
            PlayerPrefs.SetInt("EnemigosEliminados", eliminados + 1);
            PlayerPrefs.Save();

            Destroy(gameObject);
        }
    }
}
*/
