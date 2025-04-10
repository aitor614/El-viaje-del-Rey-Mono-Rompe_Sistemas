using UnityEngine;

public class PlatformSpawnerBoost : MonoBehaviour
{
    public GameObject platformPrefab;
    public int numberOfPlatforms = 10;
    public float minY = 10f;
    public float maxY = 100f;

    private float spawnY = -2f;

    void Start()
    {
        // Bordes visibles en coordenadas del mundo
        float leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        // Ancho del sprite de la plataforma
        float halfWidth = platformPrefab.GetComponent<SpriteRenderer>().bounds.extents.x;

        // Ajustar los límites seguros
        float safeLeft = leftEdge + halfWidth;
        float safeRight = rightEdge - halfWidth;

        for (int i = 0; i < numberOfPlatforms; i++)
        {
            float x = Random.Range(safeLeft, safeRight);
            Vector3 spawnPosition = new Vector3(x, spawnY, 0f);
            Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            spawnY += Random.Range(minY, maxY);
        }
    }
}
