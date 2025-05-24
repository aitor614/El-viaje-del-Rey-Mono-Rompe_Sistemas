using System.Collections;
using UnityEngine;

public class GeneradorObstaculos : MonoBehaviour
{
    [System.Serializable]
    public class TipoObstaculo
    {
        public string nombre;
        public GameObject prefabObstaculo;
        public float probabilidad;
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
        while (true)
        {
            // Generar obstáculo
            Spawn(tiposObstaculo[0]);

            // Esperar la mitad del tiempo de spawn del primer obstáculo
            yield return new WaitForSeconds(tiposObstaculo[0].probabilidad / 2f);

            // Generar premio o vida 
            int indiceAleatorio = Random.Range(1, tiposObstaculo.Length);

            // 
            if (Random.value <= tiposObstaculo[indiceAleatorio].probabilidad) 
            {
                Spawn(tiposObstaculo[indiceAleatorio]);
            }

            // Esperar la otra mitad
            yield return new WaitForSeconds(tiposObstaculo[0].probabilidad / 2f);
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
