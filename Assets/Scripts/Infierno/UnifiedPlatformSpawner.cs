using System.Collections.Generic;
using UnityEngine;

public class UnifiedPlatformSpawner : MonoBehaviour
{
    public GameObject Platform;
    public GameObject PlatformBoost;
    public GameObject FinalPlatform;

    public int totalPlataformas;
    public int cantidadBoosts;

    public float minY = 1f;
    public float maxY = 2f;

    [Range(0f, 1f)]
    public float probabilidadPlataformaMovil;

    private float spawnY = -2f;

    void Start()
    {
        HashSet<int> boostIndices = GenerateUniqueRandomIndices(totalPlataformas, cantidadBoosts);

        for (int i = 0; i < totalPlataformas; i++)
        {
            GameObject prefabToSpawn = boostIndices.Contains(i) ? PlatformBoost : Platform;

            Vector3 spawnPosition = GenerarPlataformaPosicionAleatoria(prefabToSpawn);

            GameObject platform = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

            //if (i == totalPlataformas - 1)
            //{
            //    var trigger = platform.AddComponent<PlatformTriggerFinal>();
            //    trigger.finalPlatform = FinalPlatform;
            //    trigger.offsetY = 2f;
            //}
            spawnY += Random.Range(minY, maxY);

            if (i == totalPlataformas - 1)
            {
                float finalY = spawnY - 2f;

                spawnPosition = GenerarPlataformaPosicionAleatoria(FinalPlatform);

                Instantiate(FinalPlatform, spawnPosition, Quaternion.identity);
            }

            if (Random.value < probabilidadPlataformaMovil)
            {
                PlatformMover mover = platform.GetComponent<PlatformMover>();
                if (mover != null)
                {
                    mover.enabled = true;
                }
            }

        }
    }

    private Vector3 GenerarPlataformaPosicionAleatoria(GameObject plataforma)
    {
        float leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float halfWidth = plataforma.GetComponent<SpriteRenderer>().bounds.extents.x;
        float safeLeft = leftEdge + halfWidth;
        float safeRight = rightEdge - halfWidth;
        float x = Random.Range(safeLeft, safeRight);
        return new Vector3(x, spawnY, 0f);
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
