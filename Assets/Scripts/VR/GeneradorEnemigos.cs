using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorEnemigos : MonoBehaviour
{
    [Header("Componentes")]
    public GameObject enemigo;
    public Camera camaraJugador;

    [Header("Parámetros")]
    public float intervaloAparicion;
    public float distanciaAJugador;
    public float offsetAlturaSpawn;

    [Header("Rangos offsets Aleatorios")]
    public float offsetXSpawn;
    public float offsetYSpawn;
    public float offsetZSpawn;

    public int maximoEnemigos;
    private bool generandoEnemigos = false;

    // Array enemigos generados
    private List<GameObject> enemigosGenerados;

    void Start()
    {
        Debug.Log("[GeneradorEnemigos] Iniciando generador de enemigos.");
        // Obtener main camera si no se ha asignado
        if (camaraJugador == null)
        {
            camaraJugador = Camera.main;
            if (camaraJugador == null)
            {
                Debug.LogError("[GeneradorEnemigos] No se ha asignado una cámara principal o no se ha encontrado una cámara en la escena.");
                return;
            }
        }


        // Repetir SpawnEnemy cada cierto intervalo parámetoros: nombre, inicio e intervalo
        if (intervaloAparicion <= 0)
        {
            Debug.LogError("[GeneradorEnemigos] El intervalo de aparición debe ser mayor que 0.");
            return;
        }

        generandoEnemigos = true;
        SpawnEnemy();

        StartCoroutine(GenerarEnemigos());

    }

    private void Update()
    {
        if (generandoEnemigos) return;

        // Obtener main camera si no se ha asignado
        if (camaraJugador == null)
        {
            camaraJugador = Camera.main;
            if (camaraJugador == null)
            {
                Debug.LogError("[GeneradorEnemigos] No se ha asignado una cámara principal o no se ha encontrado una cámara en la escena.");
                return;
            }
        }
        generandoEnemigos = true;
        SpawnEnemy();

        StartCoroutine(GenerarEnemigos());
    }

    // Coroutine para generar enemigos cada cierto intervalo
    private IEnumerator GenerarEnemigos()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervaloAparicion);

            Debug.Log("[GeneradorEnemigos] Generando nuevo enemigo.");
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        // Si no hay array de enemigos generados, lo inicializamos
        if (enemigosGenerados == null)
        {
            enemigosGenerados = new List<GameObject>();
            Debug.Log("[GeneradorEnemigos] Lista de enemigos generados inicializada.");
        }

        // Eliminar enemigos nulos de la lista
        enemigosGenerados.RemoveAll(e => e == null);

        // Si ya hay el máximo de enemigos, no generamos más
        if (enemigosGenerados.Count >= maximoEnemigos)
        {
            Debug.Log("[GeneradorEnemigos] Máximo de enemigos alcanzado.");
            return;
        }

        // Generar una posición aleatoria dentro de los rangos definidos
        Vector3 offset = new(
            Random.Range(-offsetXSpawn, offsetXSpawn),
            Random.Range(-offsetYSpawn, offsetYSpawn) + offsetAlturaSpawn,
            Random.Range(-offsetZSpawn, offsetZSpawn)
        );

        Vector3 spawnPos = transform.position + offset;

        // Calcular la rotación del enemigo mirando hacia el jugador
        Quaternion orientacion = Quaternion.LookRotation(camaraJugador.transform.position - spawnPos);

        // Retroceder desde esa posición
        spawnPos -= orientacion * Vector3.forward * distanciaAJugador;


        // Generar instancia del enemigo
        GameObject enemy = Instantiate(enemigo, spawnPos, orientacion);

        // Añadir el enemigo a la lista de enemigos generados
        enemigosGenerados.Add(enemy);
        Debug.Log($"[GeneradorEnemigos] Enemigo generado: {enemy.name} en la posición {spawnPos}");

        // Configurar el objetivo del enemigo para que sea la cámara del jugador
        if (enemy.TryGetComponent(out RunTowardsPlayer script))
        {
            script.objetivo = camaraJugador;
            Debug.Log($"[GeneradorEnemigos] Enemigo {enemy.name} configurado para seguir a la cámara del jugador.");
        }
        else
        {
            Debug.LogWarning($"[GeneradorEnemigos] El enemigo {enemy.name} no tiene el script RunTowardsPlayer asignado.");
        }


    }
}
