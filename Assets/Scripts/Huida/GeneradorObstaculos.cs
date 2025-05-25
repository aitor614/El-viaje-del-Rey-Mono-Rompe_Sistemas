using System.Collections;
using UnityEngine;

public class GeneradorObstaculos : MonoBehaviour
{
    [System.Serializable]
    public class TipoObstaculo
    {
        public string nombre;
        public GameObject prefabObstaculo;
        public float tiempoSpawn;
        public float probabilidadSpawn;
        public float maxAltura;
        public float minAltura;
    }

    [Header("Obstáculos")]
    public TipoObstaculo[] tiposObstaculo;
    public float velocidadDesplazamiento;

    private Coroutine rutinaGeneracion;


    private void OnEnable()
    {
        // Iniciar la rutina de generación de obstáculos intercalados
        rutinaGeneracion = StartCoroutine(GenerarIntercalados());
    }

    private void OnDisable()
    {
        // Si la rutina de generación está activa, detenerla
        if (rutinaGeneracion != null) StopCoroutine(rutinaGeneracion);
    }

    // Generación de obstáculos intercalados
    IEnumerator GenerarIntercalados()
    {
        float probAcumuladaVida = 0f;
        float probAcumuladaPremio = 0f;

        // Esperar a que el tiempo se reanude si está pausado
        while (Time.timeScale == 0f) yield return null;

        // Esperar un frame para asegurar que el tiempo se ha reanudado
        yield return null;

        // Comenzar la generación de obstáculos
        while (true)
        {
            // Generar obstáculo
            Spawn(tiposObstaculo[0]);

            // Esperar la mitad del tiempo de spawn del primer obstáculo
            yield return new WaitForSeconds(tiposObstaculo[0].tiempoSpawn / 2f);

            probAcumuladaPremio += tiposObstaculo[1].probabilidadSpawn;
            probAcumuladaVida += tiposObstaculo[2].probabilidadSpawn;

            bool generado = false;


            if (probAcumuladaVida >= 1f)
            {
                Spawn(tiposObstaculo[2]);
                probAcumuladaVida -= 1f;
                generado = true;
            }

            if (!generado && probAcumuladaPremio >= 1f)
            {
                Spawn(tiposObstaculo[1]);
                probAcumuladaPremio -= 1f;
            }


            // Esperar la otra mitad
            yield return new WaitForSeconds(tiposObstaculo[0].tiempoSpawn / 2f);
            
        }
    }

    // Spawn de un obstáculo en la posición del generador
    void Spawn(TipoObstaculo tipoObstaculo)
    {
        // Instanciar el prefab del obstáculo en la posición del generador
        GameObject obj = Instantiate(tipoObstaculo.prefabObstaculo, transform.position, Quaternion.identity);
        obj.transform.position += Vector3.up * Random.Range(tipoObstaculo.minAltura, tipoObstaculo.maxAltura);
        // Asignar la velocidad de desplazamiento
        obj.GetComponent<MovimientoObjeto2D>().velocidad = velocidadDesplazamiento;
    }
}
