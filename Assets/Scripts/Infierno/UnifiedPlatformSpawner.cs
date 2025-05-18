using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnifiedPlatformSpawner : MonoBehaviour
{
    public GameObject Platform;
    public GameObject PlatformBoost;
    public GameObject FinalPlatform;
    public RectTransform fondo;

    public int totalPlataformas;
    public int cantidadBoosts;

    public float minY = 1f;
    public float maxY = 2f;
    public float minSeparacion = 2f;
    public float maxSeparacion = 4.5f;

    private float minX;
    private float maxX;

    [Range(0f, 1f)]
    public float probabilidadPlataformaMovil;

    private float spawnY = -2f;

    void Start()
    {
        HashSet<int> boostIndices = GenerarIndicesBoostsUnicos(totalPlataformas, cantidadBoosts);

        CalcularLimitesFondo();

        for (int i = 0; i < totalPlataformas; i++)
        {
            bool esUltima = (i == totalPlataformas - 1);
            bool duplicar = (i % 2 == 0 && !esUltima); // Alternancia, pero no para la final

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

            spawnY += Random.Range(minY, maxY);

            if (esUltima)
            {
                Vector3 posicionFinal = GenerarPosicionAleatoria(FinalPlatform);
                Instantiate(FinalPlatform, posicionFinal, Quaternion.identity);
            }
        }
    }

    void CalcularLimitesFondo()
    {
        Vector3[] esquinas = new Vector3[4];
        fondo.GetWorldCorners(esquinas);
        minX = esquinas[0].x;
        maxX = esquinas[2].x;
    }

    Vector3 GenerarPosicionAleatoria(GameObject plataforma)
    {
        float ancho = plataforma.GetComponent<SpriteRenderer>().bounds.size.x;
        float x = Random.Range(minX + ancho / 2f, maxX - ancho / 2f);
        return new Vector3(x, spawnY, 0f);
    }

    void InstanciarDoblePlataforma(GameObject prefab)
    {
        float ancho = prefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float espacio = Random.Range(minSeparacion, maxSeparacion);

        float centro = Random.Range(minX + ancho + espacio / 2f, maxX - ancho - espacio / 2f);

        float x1 = centro - espacio / 2f;
        float x2 = centro + espacio / 2f;

        Vector3 pos1 = new Vector3(x1, spawnY, 0f);
        Vector3 pos2 = new Vector3(x2, spawnY, 0f);

        GameObject p1 = Instantiate(prefab, pos1, Quaternion.identity);
        GameObject p2 = Instantiate(Platform, pos2, Quaternion.identity); // Siempre normal la segunda

        ActivarMovimientoSiCorresponde(p1);
        ActivarMovimientoSiCorresponde(p2);
    }

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

    HashSet<int> GenerarIndicesBoostsUnicos(int max, int cantidad)
    {
        HashSet<int> indices = new HashSet<int>();
        while (indices.Count < cantidad)
        {
            indices.Add(Random.Range(0, max));
        }
        return indices;
    }
}
