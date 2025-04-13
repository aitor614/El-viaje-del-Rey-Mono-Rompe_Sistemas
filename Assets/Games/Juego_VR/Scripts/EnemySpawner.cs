using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public Transform target;       // El jugador
    public float spawnInterval = 2f; // Intervalo de aparición
    public float spawnDistance = 20f; // Distancia en el horizonte donde aparecerán los enemigos
    public float spawnHeight = 1.5f;  // Altura en la que aparecerán (para evitar que vengan demasiado arriba o abajo)
    public int maxEnemies = 5; // Máximo número de enemigos en la escena

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval); // Repetir SpawnEnemy cada cierto intervalo
    }

    void SpawnEnemy()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= maxEnemies)
        {
            return; // Si ya hay el máximo de enemigos, no hacemos nada
        }
        // Genera una posición aleatoria en el horizonte (enfrente del jugador)
        Vector3 spawnPos = target.position + (target.forward * spawnDistance);
        spawnPos.y = spawnHeight; // Evita que el enemigo aparezca muy arriba o abajo

        // Opcional: un poco de aleatoriedad para que aparezcan un poco a los lados
        float randomOffset = Random.Range(-5f, 5f); // Aleatoriedad para que no aparezcan en una línea recta
        spawnPos.x += randomOffset;

        // Instancia el enemigo en la posición generada
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        // Le asignamos al enemigo el objetivo (el jugador)
        enemy.GetComponent<RunTowardsPlayer>().target = target;
    }
}
