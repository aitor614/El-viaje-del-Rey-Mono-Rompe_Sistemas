using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEspiritus : MonoBehaviour
{
    [System.Serializable]
    public class TipoEspiritu
    {
        public string nombre;
        public GameObject prefab;
        public int peso;
        public int cantidadMaxima;
    }
    [Header("Tipos de espíritus")]
    public TipoEspiritu[] tiposEspiritus;

    [Header("Configuración del spawner")]
    public Camera camaraAR;
    public float distanciaMinima;
    public float distanciaMaxima;
    public float primerSpawnDelay;
    public float tiempoSpawn;
    public int maxEspiritus;

    /// Variables
    private List<GameObject> espiritusActivos = new();
    private bool corutinaLanzada = false;
    private bool todosGenerados = false;

    public static SpawnerEspiritus Instancia { get; private set; }

    private void Awake()
    {
        Instancia = this;
    }

    private void Start()
    {
        // Obtener la cámara AR si no se ha asignado
        if (camaraAR == null) camaraAR = Camera.main;
        Debug.Log("Spawn iniciado");
    }

    private void Update()
    {
        if (!corutinaLanzada)
        {
            StartCoroutine(SpawnearCadaTiempo());
            corutinaLanzada = true;
        }
    }

    // Spawnear espíritus cada cierto tiempo
    private IEnumerator SpawnearCadaTiempo()
    {
        Debug.Log("Esperando para primer spawn...");
        yield return new WaitForSeconds(primerSpawnDelay);

        while (!todosGenerados)
        {
            Debug.Log("Ejecutando SpawnEspiritus");
            SpawnEspiritus();
            // Esperar el tiempo de spawn
            yield return new WaitForSeconds(tiempoSpawn);
        }
        Debug.Log("[Spawner] Todos los espíritus generados. Corutina detenida.");
    }

    // Ejecutar el spawn al activar el objeto
    private void OnEnable()
    {
        Debug.Log("Spawner ejecutando spawn.");
        SpawnEspiritus();
    }

    // Generar espíritus en la escena
    public void SpawnEspiritus()
    {
        if (camaraAR == null || tiposEspiritus.Length == 0) return;
        if (espiritusActivos.Count >= maxEspiritus) return;

        // Ordenar por peso descendente
        List<TipoEspiritu> tiposOrdenados = new(tiposEspiritus);
        tiposOrdenados.Sort((a, b) => b.peso.CompareTo(a.peso));

        // Calcular el espacio disponible
        int espacioDisponible = maxEspiritus - espiritusActivos.Count;
        if (espacioDisponible <= 0) return;

        int cantidad = 0;
        foreach (var tipo in tiposOrdenados)
        {
            cantidad++;
            if (tipo.prefab == null || espacioDisponible <= 0) continue;

            // Contar cuántos espíritus de este tipo ya están activos
            int activosDeEsteTipo = espiritusActivos.FindAll(e => e != null && e.name.StartsWith(tipo.prefab.name)).Count;
            if (activosDeEsteTipo >= tipo.cantidadMaxima) continue;

            // Solo generar 1 por llamada para hacerlo gradual
            Vector3 posicionSpawn = GenerarPosicionAleatoria();
            GameObject espiritu = Instantiate(tipo.prefab, posicionSpawn, Quaternion.identity);
            espiritu.name = tipo.prefab.name + "_" + Random.Range(0, 10000);
            espiritusActivos.Add(espiritu);

            Debug.Log($"[Spawner] Generado {tipo.nombre}. Total: {espiritusActivos.Count}/{maxEspiritus}");
            // Generar solo uno por ciclo
            if (cantidad == 2) break; 
        }

        // Comprobar si ya no queda ninguno por generar
        bool todosCompletos = true;
        foreach (var tipo in tiposEspiritus)
        {
            int activos = espiritusActivos.FindAll(e => e != null && e.name.StartsWith(tipo.prefab.name)).Count;
            if (activos < tipo.cantidadMaxima)
            {
                todosCompletos = false;
                break;
            }
        }

        todosGenerados = todosCompletos;
    }

    public void EliminarEspiritu(GameObject espiritu)
    {
        espiritusActivos.Remove(espiritu);
        Debug.Log($"[Spawner] Eliminando espíritu. Activos: {espiritusActivos.Count}/{maxEspiritus}");
    }

    private Vector3 GenerarPosicionAleatoria()
    {
        Vector3 origen = camaraAR.transform.position;

        // Dirección aleatoria en plano horizontal
        Vector2 direccionPlanar = Random.insideUnitCircle.normalized;

        // Distancia aleatoria
        float distancia = Random.Range(distanciaMinima, distanciaMaxima);

        // Posición en el mundo real (mismo plano que el jugador)
        Vector3 offset = new Vector3(direccionPlanar.x, 0, direccionPlanar.y) * distancia;

        return origen + offset;
    }
}