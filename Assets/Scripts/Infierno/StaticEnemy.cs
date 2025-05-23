using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticEnemy : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("¡Tocado por enemigo!");
        }
    }
}
