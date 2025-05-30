using UnityEngine;

public class DestroyBelowCamera : MonoBehaviour
{
    public float offsetY = 6f;

    void Update()
    {
        if (transform.position.y < Camera.main.transform.position.y - offsetY)
        {
            UnifiedPlatformSpawner spawner = FindFirstObjectByType<UnifiedPlatformSpawner>();
            if (spawner != null)
            {
                // Si hay un spawner, devolver el objeto al pool
                spawner.DevolverAlPool(gameObject);
            }
            else
            {
                // Si no hay un spawner, simplemente desactivar el objeto
                Debug.LogWarning("No se encontró UnifiedPlatformSpawner. Desactivando el objeto directamente.");
                gameObject.SetActive(false);
            }
        }
    }
}
