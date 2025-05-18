using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int numberOfEnemies = 3;
    public float minY = 10f;
    public float maxY = 90f;
    public float horizontalMargin = 0.5f;

    private float levelWidth;

    void Start()
    {
        float camHeight = Camera.main.orthographicSize * 2f;
        levelWidth = camHeight * Camera.main.aspect;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Limitar aparición horizontal
            float halfWidth = enemyPrefab.GetComponent<SpriteRenderer>().bounds.extents.x;
            float safeLeft = -levelWidth / 2f + halfWidth + horizontalMargin;
            float safeRight = levelWidth / 2f - halfWidth - horizontalMargin;

            float x = Random.Range(safeLeft, safeRight);
            float y = Random.Range(minY, maxY);

            Vector3 spawnPos = new Vector3(x, y, 0f);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

    // Gizmos para ver el rango de aparición en la Scene View
    void OnDrawGizmos()
    {
        if (Camera.main == null) return;

        float camHeight = Camera.main.orthographicSize * 2f;
        float width = camHeight * Camera.main.aspect;

        Vector3 center = new Vector3(0f, (minY + maxY) / 2f, 0f);
        Vector3 size = new Vector3(width - 2 * horizontalMargin, maxY - minY, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
