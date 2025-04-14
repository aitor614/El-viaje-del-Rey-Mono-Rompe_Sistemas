using System.Collections.Generic;
using UnityEngine;

public class UnifiedPlatformSpawner : MonoBehaviour
{
    public GameObject Platform;
    public GameObject PlatformBoost;
    public GameObject FinalPlatform;

    public int totalPlatforms = 99;
    public int numberOfBoosts = 10;

    public float minY = 1f;
    public float maxY = 2f;

    [Range(0f, 1f)]
    public float movingPlatformChance = 0.05f;

    public float minSpacing = 2f;
    public float maxSpacing = 5f;
    public float margin = 0.5f;

    private float spawnY = -2f;

    void Start()
    {
        HashSet<int> boostIndices = GenerateUniqueRandomIndices(totalPlatforms, numberOfBoosts);

        float camHeight = Camera.main.orthographicSize * 2f;
        float levelWidth = camHeight * Camera.main.aspect;
        float halfWidth = Platform.GetComponent<SpriteRenderer>().bounds.extents.x;
        float usableWidth = levelWidth - 2 * (margin + halfWidth);

        for (int i = 0; i < totalPlatforms; i++)
        {
            bool spawnDouble = (i % 2 == 0) && (i != totalPlatforms - 1); // no duplicar la última

            float xSingle = Random.Range(-usableWidth / 2f, usableWidth / 2f);
            Vector3 posSingle = new Vector3(xSingle, spawnY, 0f);

            GameObject platform1;
            GameObject platform2 = null;

            bool isBoost = boostIndices.Contains(i);
            GameObject prefabToSpawn = isBoost ? PlatformBoost : Platform;

            if (spawnDouble)
            {
                // Calcular espaciado aleatorio
                float spacing = Random.Range(minSpacing, maxSpacing);
                float centerX = Random.Range(-usableWidth / 2f, usableWidth / 2f);
                float offsetX = spacing / 2f;

                float x1 = Mathf.Clamp(centerX - offsetX, -usableWidth / 2f, usableWidth / 2f);
                float x2 = Mathf.Clamp(centerX + offsetX, -usableWidth / 2f, usableWidth / 2f);

                Vector3 pos1 = new Vector3(x1, spawnY, 0f);
                Vector3 pos2 = new Vector3(x2, spawnY, 0f);

                platform1 = Instantiate(prefabToSpawn, pos1, Quaternion.identity);
                platform2 = Instantiate(Platform, pos2, Quaternion.identity);

                // Movimiento sincronizado (si toca)
                bool makeMovable = Random.value < movingPlatformChance;
                if (makeMovable)
                {
                    PlatformMover m1 = platform1.GetComponent<PlatformMover>();
                    PlatformMover m2 = platform2.GetComponent<PlatformMover>();
                    if (m1 != null) m1.enabled = true;
                    if (m2 != null) m2.enabled = true;
                }
            }
            else
            {
                // Plataforma única
                platform1 = Instantiate(prefabToSpawn, posSingle, Quaternion.identity);

                if (Random.value < movingPlatformChance)
                {
                    PlatformMover mover = platform1.GetComponent<PlatformMover>();
                    if (mover != null) mover.enabled = true;
                }
            }

            // Última plataforma: le colocamos el trigger
            if (i == totalPlatforms - 1)
            {
                var trigger = platform1.AddComponent<PlatformTriggerFinal>();
                trigger.finalPlatform = FinalPlatform;
                trigger.offsetY = 2f;
            }

            spawnY += Random.Range(minY, maxY);
        }
    }

    HashSet<int> GenerateUniqueRandomIndices(int max, int count)
    {
        HashSet<int> indices = new HashSet<int>();
        while (indices.Count < count)
        {
            indices.Add(Random.Range(0, max));
        }
        return indices;
    }
}
