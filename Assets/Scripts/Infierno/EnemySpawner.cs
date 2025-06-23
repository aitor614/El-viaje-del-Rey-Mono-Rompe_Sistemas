using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int numberOfEnemies = 4;
    public float minY = 10f;
    public float maxY = 60f;
    public float horizontalMargin = 2f;
    private float levelWidth;
    private List<float> alturasOcupadas = new List<float>();
    public float distanciaEntreEnemigos = 5f;


    void Start()
    {


    }

    public void GenerarEnemigos()
    {
        float camHeight = Camera.main.orthographicSize * 2f;
        levelWidth = camHeight * Camera.main.aspect;

        // Separación del spawn de los enemigos para que no se solapen y sea imposible cruzar.

        int enemigosGenerados = 0;
        int intentosMaximos = 100;

        while (enemigosGenerados < numberOfEnemies && intentosMaximos > 0)
        {
            // Cálculo horizontal (igual que antes)
            float halfWidth = enemyPrefab.GetComponent<SpriteRenderer>().bounds.extents.x;
            float safeLeft = -levelWidth / 2f + halfWidth + horizontalMargin;
            float safeRight = levelWidth / 2f - halfWidth - horizontalMargin;
            float x = Random.Range(safeLeft, safeRight);

            // Cálculo vertical con distancia mínima
            float y = Random.Range(minY, maxY);
            bool esValida = true;

            foreach (float yExistente in alturasOcupadas)
            {
                if (Mathf.Abs(y - yExistente) < distanciaEntreEnemigos)
                {
                    esValida = false;
                    break;
                }
            }

            if (esValida)
            {
                Vector3 spawnPos = new Vector3(x, y, 0f);
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                alturasOcupadas.Add(y);
                enemigosGenerados++;
            }

            intentosMaximos--;
        }
    }

    // Gizmos para ver el rango de aparición en la Scene View
    void OnDrawGizmos()
    {
        if (Camera.main == null) return;

        float camHeight = Camera.main.orthographicSize * 2f;
        float width = camHeight * Camera.main.aspect;

        Vector3 center = new(0f, (minY + maxY) / 2f, 0f);
        Vector3 size = new(width - 2 * horizontalMargin, maxY - minY, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
