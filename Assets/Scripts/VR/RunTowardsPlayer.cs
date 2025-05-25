using UnityEngine;

public class RunTowardsPlayer : MonoBehaviour
{
    [Header("Componentes")]
    public Camera objetivo;
    public Animator animador;

    [Header("Parámetros")]
    public float offsetAlturaObjetivo;
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
        else
        {
            // Atacar si está cerca
            if (!hasAttacked)
            {
                if (animador == null)
                {
                    Debug.LogWarning("Animator no asignado. No se puede activar el ataque.");
                    return;
                }
                animador.SetTrigger("AttackTrigger");
                hasAttacked = true;
            }

            if (Time.time - lastAttackTime >= enfriamientoAtaque)
            {
                if (objetivo.TryGetComponent<PlayerHealth>(out var playerHealth))
                {
                    playerHealth.TakeDamage(fuerzaAtaque);
                }

                lastAttackTime = Time.time;
            }
        }
    }
}

