using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public Transform player;
    public float speed = 2.5f;
    public int health = 3;
    public int damage = 1;

    private Animator animator;
    private bool hasAttacked = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null && !hasAttacked && health > 0)
        {
            Vector3 direction = (player.position - transform.position);
            direction.y = 0;
            direction.Normalize();

            transform.position += direction * speed * Time.deltaTime;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasAttacked)
        {
            hasAttacked = true;
            animator.SetBool("isAttacking", true);
            Debug.Log("¡El enemigo ataca!");

            // Aquí iría el daño real al jugador
            // other.GetComponent<PlayerHealth>().TakeDamage(damage);

            Destroy(gameObject, 1.5f);
        }

        if (other.CompareTag("PlayerAttack") && !hasAttacked)
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            animator.SetBool("isDead", true);
            Destroy(gameObject, 2f);
        }
    }
}
