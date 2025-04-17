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
        public int peso = 1;
    }
    [Header("Tipos de espíritus")]
    [SerializeField] private TipoEspiritu[] tiposEspiritus;

    [Header("Configuración del spawner")]
    [SerializeField] private Camera camaraAR;
    [SerializeField] private int cantidad;
    [SerializeField] private float distanciaMinima;
    [SerializeField] private float distanciaMaxima;
    [SerializeField] private float primerSpawnDelay;
    [SerializeField] private float tiempoSpawn;
    [SerializeField] private int maxEspiritus;
    private List<GameObject> espiritusActivos = new();
    private bool corutinaLanzada = false;

    public static SpawnerEspiritus Instancia { get; private set; }

    private void Awake()
    {
        Instancia = this;
    }

    private void Start()
    {
        if (camaraAR == null) camaraAR = Camera.main;

        Debug.Log("Spawn iniciado");
        InvokeRepeating(nameof(SpawnEspiritus), primerSpawnDelay, tiempoSpawn);
    }

        private void Update()
    {
        if (!corutinaLanzada)
        {
            StartCoroutine(SpawnearCadaTiempo());
            corutinaLanzada = true;
        }
    }

    private IEnumerator SpawnearCadaTiempo()
    {
        Debug.Log("Esperando para primer spawn...");
        yield return new WaitForSeconds(primerSpawnDelay);

        while (true)
        {
            Debug.Log("Ejecutando SpawnEspiritus");
            SpawnEspiritus();
            yield return new WaitForSeconds(tiempoSpawn);
        }
    }


    private void OnEnable()
    {
        Debug.Log("Spawner ejecutando spawn.");
        SpawnEspiritus();
    }

    public void SpawnEspiritus()
    {
        if (camaraAR == null || tiposEspiritus.Length == 0) return;
        if (espiritusActivos.Count >= maxEspiritus) return;

        int espacioDisponible = maxEspiritus - espiritusActivos.Count;
        int aGenerar = Mathf.Min(cantidad, espacioDisponible);

        for (int i = 0; i < aGenerar; i++)
        {
            Vector3 posicionSpawn = GenerarPosicionAleatoria();

            TipoEspiritu tipo = CalcTipoEspiritu();
            if (tipo == null || tipo.prefab == null) continue;

            GameObject espiritu = Instantiate(tipo.prefab, posicionSpawn, Quaternion.identity);
            espiritusActivos.Add(espiritu);

        }
        Debug.Log($"[Spawner] Ejecutando SpawnEspiritus. Activos: {espiritusActivos.Count}/{maxEspiritus}");
    }

    private TipoEspiritu CalcTipoEspiritu()
    {
        int totalPeso = 0;
        foreach (var tipo in tiposEspiritus)
            totalPeso += tipo.peso;

        int valor = Random.Range(0, totalPeso);
        int acumulador = 0;

        foreach (var tipo in tiposEspiritus)
        {
            acumulador += tipo.peso;
            if (valor < acumulador)
                return tipo;
        }

        return tiposEspiritus[0];
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