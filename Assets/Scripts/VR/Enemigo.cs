using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [Header("Componentes")]
    public Transform player;
    public Animator animacion;

    [Header("Parámetros")]
    public float velocidad;
    public int vida;
    public int poderGolpe;
    public int puntosEliminar;

    // Variables
    private bool ataqueRealizado = false;

    void Start()
    {

    }

    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Proyectil"))
        {
            RestarVida(1);
        }

        if (collision.gameObject.CompareTag("Player") && !ataqueRealizado)
        {
            PlayerPrefs.SetInt("VidasRestantes", PlayerPrefs.GetInt("VidasRestantes") - poderGolpe);
            PlayerPrefs.Save();
            ataqueRealizado = true;
            //animacion.SetBool("isAttacking", true);
            Debug.Log("¡El enemigo ataca!");
            Destroy(gameObject, 1f);
        }

    }

    void RestarVida(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0)
        {
            PlayerPrefs.SetInt("PuntuacionPartida", PlayerPrefs.GetInt("PuntuacionPartida") + puntosEliminar);
            PlayerPrefs.SetInt("EnemigosEliminados", PlayerPrefs.GetInt("EnemigosEliminados") + 1);
            PlayerPrefs.Save();
            Destroy(gameObject);
        }
    }
}
