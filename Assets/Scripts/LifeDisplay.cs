using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour
{
    public Image[] lifeIcons; // Asigna las 3 imágenes en el Inspector

    public void UpdateLives(int currentLives)
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].enabled = i < currentLives; // Activa solo las vidas disponibles
        }
    }
}