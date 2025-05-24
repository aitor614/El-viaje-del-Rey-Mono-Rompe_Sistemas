using UnityEngine;

public class Premio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colisión de premio con: " + collision.gameObject.name);
        // Si el obstáculo colisiona con un jugador, se destruye
        if (collision.CompareTag("Player"))
        {
            Debug.Log("¡Premio recogido!");
            Destroy(gameObject);
        }
    }
}
