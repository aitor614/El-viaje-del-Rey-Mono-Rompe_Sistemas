using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Efectos")]
    public GameObject efectoExplosion; // Prefab del efecto de explosi�n

    [Header("Par�metros")]
    public float fuerzaProyectil = 10f;
    public float tiempoVida = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * fuerzaProyectil;
        Destroy(gameObject, tiempoVida);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Instanciar el efecto de explosi�n en la posici�n del impacto
        if (efectoExplosion != null)
        {
            Instantiate(efectoExplosion, transform.position, Quaternion.identity);
        }

        // Destruir el proyectil
        Destroy(gameObject);
    }
}
