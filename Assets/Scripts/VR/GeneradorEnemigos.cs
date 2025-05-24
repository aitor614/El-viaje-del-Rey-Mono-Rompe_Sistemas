using System.Collections.Generic;
using UnityEngine;

public class GeneradorEnemigos : MonoBehaviour
{
    [Header("Componentes")]
    public GameObject enemigo;
    public Camera camaraJugador;

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
        // Obtener main camera si no se ha asignado
        if (camaraJugador == null)
        {
            camaraJugador = Camera.main;
            if (camaraJugador == null)
            {
                Debug.LogError("No se ha asignado una cámara principal o no se ha encontrado una cámara en la escena.");
                return;
            }
        }
        // Repetir SpawnEnemy cada cierto intervalo parámetoros: nombre, inicio e intervalo
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
        Quaternion orientacion = Quaternion.LookRotation(camaraJugador.transform.position - spawnPos);

        // Generar instancia del enemigo
        GameObject enemy = Instantiate(enemigo, spawnPos, orientacion);

        // Añadir el enemigo a la lista de enemigos generados
        enemigosGenerados.Add(enemy);

        // Configurar el objetivo del enemigo para que sea la cámara del jugador
        if (enemy.TryGetComponent(out RunTowardsPlayer script)) script.objetivo = camaraJugador;

    }
}
