using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("¡Has recibido daño! Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log(" Has muerto.");
            // Aquí podrías reiniciar la escena, mostrar pantalla de Game Over, etc.
        }
    }
}
