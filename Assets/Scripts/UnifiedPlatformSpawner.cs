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

    private float spawnY = -2f;

    void Start()
    {
        HashSet<int> boostIndices = GenerateUniqueRandomIndices(totalPlatforms, numberOfBoosts);

        for (int i = 0; i < totalPlatforms; i++)
        {
            GameObject prefabToSpawn = boostIndices.Contains(i) ? PlatformBoost : Platform;

            float leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
            float rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

            float halfWidth = prefabToSpawn.GetComponent<SpriteRenderer>().bounds.extents.x;
            float safeLeft = leftEdge + halfWidth;
            float safeRight = rightEdge - halfWidth;

            float x = Random.Range(safeLeft, safeRight);
            Vector3 spawnPosition = new Vector3(x, spawnY, 0f);

            GameObject platform = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

            if (i == totalPlatforms - 1)
            {
                var trigger = platform.AddComponent<PlatformTriggerFinal>();
                trigger.finalPlatform = FinalPlatform;
                trigger.offsetY = 2f;
            }

            if (Random.value < movingPlatformChance)
            {
                PlatformMover mover = platform.GetComponent<PlatformMover>();
                if (mover != null)
                {
                    mover.enabled = true;
                }
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
