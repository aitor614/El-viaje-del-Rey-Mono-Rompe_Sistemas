using System.Collections.Generic;
using UnityEngine;

public class GeneradorEnemigos : MonoBehaviour
{
    [Header("Componentes")]
    public GameObject enemigoSobreNube; // Prefab que contiene la nube y al enemigo como hijo
    public Transform posicionJugador;

    [Header("Parámetros")]
    public float intervaloAparicion;
    public float distanciaSpawn;
    public float offsetAlturaSpawn;

    [Header("Rangos offsets Aleatorios")]
    public float offsetLateralSpawn;
    public float offsetVerticalSpawn;
    public float offsetProfundidadSpawn;

    public int maximoEnemigos;

    private List<GameObject> enemigosGenerados;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, intervaloAparicion);
    }

    void SpawnEnemy()
    {
        if (enemigosGenerados == null)
        {
            enemigosGenerados = new List<GameObject>();
        }

        enemigosGenerados.RemoveAll(e => e == null);

        if (enemigosGenerados.Count >= maximoEnemigos)
        {
            Debug.Log("Máximo de enemigos alcanzado.");
            return;
        }

        Vector3 offset = new(
            Random.Range(-offsetLateralSpawn, offsetLateralSpawn),
            Random.Range(-offsetVerticalSpawn, offsetVerticalSpawn) + offsetAlturaSpawn,
            Random.Range(-offsetProfundidadSpawn, offsetProfundidadSpawn)
        );

        Vector3 spawnPos = transform.position + offset;
        Quaternion orientacion = Quaternion.LookRotation(posicionJugador.position - spawnPos);

        GameObject instancia = Instantiate(enemigoSobreNube, spawnPos, orientacion);
        enemigosGenerados.Add(instancia);

        // Buscar los scripts en los hijos del prefab
        RunTowardsPlayer runScript = instancia.GetComponentInChildren<RunTowardsPlayer>();
        if (runScript != null) runScript.objetivo = posicionJugador;

        Enemigo enemigoScript = instancia.GetComponentInChildren<Enemigo>();
        if (enemigoScript != null) enemigoScript.player = posicionJugador;
    }
}
/*using System.Collections.Generic;
using UnityEngine;

public class GeneradorEnemigos : MonoBehaviour
{
    [Header("Componentes")]
    public GameObject enemigo;
    public Transform posicionJugador;

    [Header("Parámetros")]
    public float intervaloAparicion;
    public float distanciaSpawn;
    public float offsetAlturaSpawn;

    [Header("Rangos offsets Aleatorios")]
    public float offsetLateralSpawn;
    public float offsetVerticalSpawn;
    public float offsetProfundidadSpawn;

    public int maximoEnemigos;

    // Array enemigos generados
    private List<GameObject> enemigosGenerados;

    void Start()
    {
        // Repetir SpawnEnemy cada cierto intervalo
        InvokeRepeating(nameof(SpawnEnemy), 1f, intervaloAparicion);
    }

    void SpawnEnemy()
    {
        // Si no hay array de enemigos generados, lo inicializamos
        if (enemigosGenerados == null)
        {
            enemigosGenerados = new List<GameObject>();
        }

        // Eliminar enemigos nulos de la lista
        enemigosGenerados.RemoveAll(e => e == null);

        // Si ya hay el máximo de enemigos, no generamos más
        if (enemigosGenerados.Count >= maximoEnemigos)
        {
            Debug.Log("Máximo de enemigos alcanzado.");
            return;
        }

        // Generar offset aleatorio
        Vector3 offset = new(
            Random.Range(-offsetLateralSpawn, offsetLateralSpawn),
            Random.Range(-offsetVerticalSpawn, offsetVerticalSpawn) + offsetAlturaSpawn,
            Random.Range(-offsetProfundidadSpawn, offsetProfundidadSpawn)
        );

        // Definir posición de spawn del enemigo
        Vector3 spawnPos = transform.position + offset;

        // Nueva orientación hacia el jugador
        Quaternion orientacion = Quaternion.LookRotation(posicionJugador.position - spawnPos);

        // Generar instancia del enemigo
        GameObject enemy = Instantiate(enemigo, spawnPos, orientacion);

        // Añadir el enemigo a la lista de enemigos generados
        enemigosGenerados.Add(enemy);

        // Definir la posición del jugador en scripts del enemigo
        if (enemy.TryGetComponent(out RunTowardsPlayer script)) script.objetivo = posicionJugador;
        if (enemy.TryGetComponent(out Enemigo script2)) script2.player = posicionJugador;

    }
}
*/