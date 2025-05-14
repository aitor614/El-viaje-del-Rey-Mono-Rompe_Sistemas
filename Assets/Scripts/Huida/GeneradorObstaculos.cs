using UnityEngine;

public class GeneradorObstaculos : MonoBehaviour
{
    public GameObject prefabObstaculo;
    public GameObject premio;
    public float spawnRate = 1.0f;
    public float minHeight = -1.0f;
    public float maxHeight = 1.0f;


    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);

    }
    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }
    private void Spawn()
    {
        // Instantia el obstáculo en una posición aleatoria dentro de los límites especificados
        GameObject obstaculos = Instantiate(prefabObstaculo, transform.position, Quaternion.identity);
        obstaculos.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);

    }

}
