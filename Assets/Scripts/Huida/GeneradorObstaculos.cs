using UnityEngine;

public class GeneradorObstaculos : MonoBehaviour
{
    public GameObject prefab;
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
        GameObject pipes = Instantiate(prefab, transform.position, Quaternion.identity);
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
    }

}
