using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnifiedPlatformSpawner : MonoBehaviour
{
    [Header("Prefabs de plataformas")]
    public GameObject Platform;
    public GameObject PlatformBoost;
    public GameObject FinalPlatform;
    public RectTransform fondo;

    [Header("Cantidad de plataformas")]
    public int totalPlataformas;
    public int cantidadBoosts;

    [Header("Parámetros de altura y separación")]
    public float minAlturaY;
    public float maxAlturaY;
    public float minSeparacion;
    public float maxSeparacion;

    [Range(0f, 1f)]
    public float probabilidadPlataformaMovil;

    private float spawnY = -2f;
    private HashSet<int> boostIndices;

    // Variables para calcular los límites del fondo
    private float minX;
    private float maxX;

    // Pool de plataformas
    private Queue<GameObject> poolNormal = new();
    private Queue<GameObject> poolBoost = new();
    private Queue<GameObject> poolFinal = new();

    [Header("Control manual de generación")]
    public bool autoGenerarAlIniciar = true;
    private bool plataformasGeneradas = false;

    void Start()
    {
        // Inicializar las pools de plataformas
        InicializarPools();

        // Generar plataformas al iniciar si está habilitado
        if (autoGenerarAlIniciar)
        {
            GenerarPlataformas();
        }
    }

    // Inicializar las pools de plataformas
    private void InicializarPools()
    {
        // Calcular total de plataformas normales más dobles posibles
        int plataformasDobles = totalPlataformas / 2;
        int plataformasNormales = totalPlataformas - cantidadBoosts - 1 + plataformasDobles;

        // Generar las plataformas normales, de boost y la final

        for (int i = 0; i < plataformasNormales; i++)
        {
            CrearYGuardarEnPool(Platform, poolNormal);
        }

        for (int i = 0; i < cantidadBoosts; i++)
        {
            CrearYGuardarEnPool(PlatformBoost, poolBoost);
        }

        // Solo una final
        CrearYGuardarEnPool(FinalPlatform, poolFinal);
    }

    // Crear y guardar una plataforma en el pool correspondiente
    private void CrearYGuardarEnPool(GameObject prefab, Queue<GameObject> pool)
    {
        GameObject plataformaNueva = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
        plataformaNueva.SetActive(false);
        pool.Enqueue(plataformaNueva);
    }

    // Obtener una plataforma del pool correspondiente y posicionarla
    private GameObject ObtenerDePool(GameObject prefab, Vector3 posicion)
    {
        Queue<GameObject> poolPlataformaDeseada = ObtenerPoolCorrespondiente(prefab);
        GameObject plataformaPool;

        if (poolPlataformaDeseada.Count > 0)
        {
            plataformaPool = poolPlataformaDeseada.Dequeue();
        }
        else
        {
            plataformaPool = Instantiate(prefab, transform);
        }

        plataformaPool.transform.SetPositionAndRotation(posicion, Quaternion.identity);
        plataformaPool.SetActive(true);
        return plataformaPool;
    }

    // Discriminación de pools según el prefab
    private Queue<GameObject> ObtenerPoolCorrespondiente(GameObject prefab)
    {
        if (prefab == PlatformBoost) return poolBoost;
        if (prefab == FinalPlatform) return poolFinal;
        return poolNormal;
    }

    // Genera las plataformas y los boosts únicos,si no se han generado aún
    public void GenerarPlataformas()
    {
        if (plataformasGeneradas) return;

        boostIndices = GenerarIndicesBoostsUnicos(totalPlataformas, cantidadBoosts);
        StartCoroutine(GenerarPlataformasTrasLayout());
        plataformasGeneradas = true;
    }

    // Reiniciar el estado de las plataformas y pools
    public void ResetearPlataformas()
    {
        // Desactivar todas las plataformas de la escena
        foreach (Transform hijo in transform)
        {
            if (hijo.gameObject.activeSelf)
            {
                DevolverAlPool(hijo.gameObject);
            }
        }

        // Definir altura de spawn por debajo del jugador
        spawnY = -2f;
        plataformasGeneradas = false;
    }

    // Devolver una plataforma al pool correspondiente y desactivarla
    public void DevolverAlPool(GameObject plataforma)
    {
        plataforma.SetActive(false);

        if (plataforma.name.Contains(FinalPlatform.name)) poolFinal.Enqueue(plataforma);
        else if (plataforma.name.Contains(PlatformBoost.name)) poolBoost.Enqueue(plataforma);
        else poolNormal.Enqueue(plataforma);
    }

    // Coroutine para generar plataformas después de aplicar el layout
    IEnumerator GenerarPlataformasTrasLayout()
    {
        // Espera 1 frame para asegurar que el layout esté aplicado
        yield return null;

        CalcularLimitesFondo();

        for (int i = 0; i < totalPlataformas; i++)
        {
            // Determinar si es la última plataforma
            bool esUltima = (i == totalPlataformas - 1);

            // Alternar entre plataformas dobles y simples
            bool duplicar = (i % 2 == 0 && !esUltima);

            // Si el índice actual está en los índices de boost, usar la plataforma de boost
            GameObject prefab = boostIndices.Contains(i) ? PlatformBoost : Platform;

            if (duplicar)
            {
                InstanciarDoblePlataforma(prefab);
            }
            else
            {
                Vector3 posicion = GenerarPosicionAleatoria(prefab);
                GameObject plataforma = ObtenerDePool(prefab, posicion);
                ActivarMovimientoSiCorresponde(plataforma);
            }

            spawnY += Random.Range(minAlturaY, maxAlturaY);

            if (esUltima)
            {
                Vector3 posicionFinal = GenerarPosicionAleatoria(FinalPlatform);
                ObtenerDePool(FinalPlatform, posicionFinal);
            }
        }
    }

    // Calcular limites fondo para evitar que las plataformas se generen fuera de la pantalla
    void CalcularLimitesFondo()
    {
        Vector3[] esquinas = new Vector3[4];
        fondo.GetWorldCorners(esquinas);
        minX = esquinas[0].x;
        maxX = esquinas[2].x;
    }

    // Generar una posición aleatoria dentro de los límites del fondo, considerando el ancho de la plataforma
    Vector3 GenerarPosicionAleatoria(GameObject plataforma)
    {
        float ancho = plataforma.GetComponent<SpriteRenderer>().bounds.size.x;
        float x = Random.Range(minX + ancho / 2f, maxX - ancho / 2f);
        return new Vector3(x, spawnY, 0f);
    }

    // Instanciar una plataforma doble, una móvil y otra normal
    void InstanciarDoblePlataforma(GameObject prefab)
    {
        float ancho = prefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float espacio = Random.Range(minSeparacion, maxSeparacion);

        float centro = Random.Range(minX + ancho + espacio / 2f, maxX - ancho - espacio / 2f);

        float x1 = centro - espacio / 2f;
        float x2 = centro + espacio / 2f;

        Vector3 pos1 = new(x1, spawnY, 0f);
        Vector3 pos2 = new(x2, spawnY, 0f);

        GameObject p1 = ObtenerDePool(prefab, pos1);
        GameObject p2 = ObtenerDePool(Platform, pos2); // Siempre normal la segunda

        ActivarMovimientoSiCorresponde(p1);
        ActivarMovimientoSiCorresponde(p2);
    }

    // Activar el movimiento de la plataforma si corresponde según la probabilidad
    void ActivarMovimientoSiCorresponde(GameObject plataforma)
    {
        if (Random.value < probabilidadPlataformaMovil)
        {
            if (plataforma.TryGetComponent<PlatformMover>(out var mover))
            {
                mover.fondo = fondo;
                mover.enabled = true;
            }
        }
    }

    // Generar un conjunto de índices únicos para los boosts
    HashSet<int> GenerarIndicesBoostsUnicos(int max, int cantidad)
    {
        cantidad = Mathf.Clamp(cantidad, 0, max); // Seguridad: evitar overflow

        List<int> indices = new();
        for (int i = 0; i < max; i++) indices.Add(i);

        // Mezclar la lista
        for (int i = 0; i < indices.Count; i++)
        {
            int temp = indices[i];
            int randomIndex = Random.Range(i, indices.Count);
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        return new HashSet<int>(indices.GetRange(0, cantidad));
    }
}
