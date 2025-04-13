using UnityEngine;

public class RunTowardsPlayer : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float attackDistance = 1.5f;
    public int damage = 10;
    public float attackCooldown = 2f;

    private Vector3 targetOffset;
    private float lastAttackTime;

    private Animator animator;
    private bool hasAttacked = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (target != null)
        {
            float offsetRange = 1.5f;
            Vector2 randomOffset = Random.insideUnitCircle * offsetRange;

            targetOffset = new Vector3(
                target.position.x + randomOffset.x,
                transform.position.y,
                target.position.z + randomOffset.y
            );
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            // Movimiento
            transform.position = Vector3.MoveTowards(transform.position, targetOffset, speed * Time.deltaTime);
            transform.LookAt(new Vector3(targetOffset.x, transform.position.y, targetOffset.z));
        }
        else
        {
            // Atacar si está cerca
            if (!hasAttacked)
            {
                animator.SetTrigger("AttackTrigger");
                hasAttacked = true; // evita que se repita la animación todo el rato
            }

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }

                lastAttackTime = Time.time;
            }
        }
    }
}

