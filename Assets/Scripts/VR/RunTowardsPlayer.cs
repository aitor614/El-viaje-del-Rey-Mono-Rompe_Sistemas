using UnityEngine;

public class RunTowardsPlayer : MonoBehaviour
{
    [Header("Componentes")]
    public Transform objetivo;
    public Animator animador;

    [Header("Parámetros")]
    public float offsetAlturaObjetivo= -5f;
    public float velocidad;
    public float distanciaAtaque;
    public int fuerzaAtaque;
    public float enfriamientoAtaque;

    // Variables
    private float lastAttackTime;
    private bool hasAttacked = false;
    


    void Start()
    {

    }

    void Update()
    {
        if (objetivo == null) return;

        // Calcula la posición corregida del objetivo con altura ajustable
        Vector3 posicionObjetivo = new(
            objetivo.position.x,
            objetivo.position.y + offsetAlturaObjetivo,
            objetivo.position.z
        );

        float distance = Vector3.Distance(transform.position, posicionObjetivo);

        if (distance > distanciaAtaque)
        {
            Vector3 direccion = (posicionObjetivo - transform.position).normalized;

            transform.position += Time.deltaTime * velocidad * direccion;

            // Mira directamente al objetivo con la altura corregida
            transform.LookAt(posicionObjetivo);
        }
        else
        {
            // Atacar si está cerca
            if (!hasAttacked)
            {
                animador?.SetTrigger("AttackTrigger");
                hasAttacked = true;
            }

            if (Time.time - lastAttackTime >= enfriamientoAtaque)
            {
                PlayerHealth playerHealth = objetivo.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(fuerzaAtaque);
                }

                lastAttackTime = Time.time;
            }
        }
    }
}

