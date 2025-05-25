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

    // Variables para calcular los límites del fondo
    private float minX;
    private float maxX;

    [Range(0f, 1f)]
    public float probabilidadPlataformaMovil;

    private float spawnY = -2f;

    void Start()
    {
        // Generar un número único de índices para los boosts
        HashSet<int> boostIndices = GenerarIndicesBoostsUnicos(totalPlataformas, cantidadBoosts);

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
                GameObject plataforma = Instantiate(prefab, posicion, Quaternion.identity);
                ActivarMovimientoSiCorresponde(plataforma);
            }

            spawnY += Random.Range(minAlturaY, maxAlturaY);

            if (esUltima)
            {
                Vector3 posicionFinal = GenerarPosicionAleatoria(FinalPlatform);
                Instantiate(FinalPlatform, posicionFinal, Quaternion.identity);
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

        GameObject p1 = Instantiate(prefab, pos1, Quaternion.identity);
        GameObject p2 = Instantiate(Platform, pos2, Quaternion.identity); // Siempre normal la segunda

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
        HashSet<int> indices = new();

        // Repetir hasta que tengamos la cantidad deseada de índices de boost únicos
        while (indices.Count < cantidad)
        {
            indices.Add(Random.Range(0, max));
        }
        return indices;
    }
}
