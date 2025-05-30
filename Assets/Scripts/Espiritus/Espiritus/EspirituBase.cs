using System.Collections;
using UnityEngine;

public class EspirituBase : MonoBehaviour
{
    [Header("Atributos")]
    public int vida;
    public int puntosEspiritu;

    [Header("Movimiento & Posición")]
    public float velocidadOrbital;
    public float velocidadRotacionVisual;
    public float cambioDireccionCada;
    public float distanciaMinima;
    public float distanciaMaxima;
    public float alturaMinima;
    public float alturaMaxima;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip clipTocado;
    public AudioClip clipDesvanecer;

    [Header("Partículas")]
    public ParticleSystem sistemaParticulas;

    // Variables
    private Transform jugador;
    private Vector3 direccionActual;
    private float alturaActual;
    private float tiempoCambio;

    protected virtual void Start()
    {
        jugador = Camera.main.transform;
        NuevoRumbo();
    }

    protected virtual void Update()
    {
        // Movimiento constante en el espacio
        transform.position += Time.deltaTime * velocidadOrbital * direccionActual;

        // Mantener altura limitada
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, alturaMinima, alturaMaxima);
        pos.x = Mathf.Clamp(pos.x, -distanciaMaxima, distanciaMaxima);
        transform.position = pos;

        // Rotación visual sobre sí mismo
        transform.Rotate(Vector3.up, velocidadRotacionVisual * Time.deltaTime);

        // Si ha pasado un tiempo, cambiar objetivo
        tiempoCambio -= Time.deltaTime;
        if (tiempoCambio <= 0f)
        {
            NuevoRumbo();
        }
    }

    // private void void NuevoRumbo()
    //{
    //    if (jugador == null) return;

    //    Vector3 centro = jugador.position;

    //    // Elegir dirección horizontal aleatoria normalizada
    //    Vector2 plano = Random.insideUnitCircle.normalized;

    //    // Elegir distancia garantizando un anillo entre mínima y máxima
    //    float distancia = Random.Range(distanciaMinima, distanciaMaxima);
    //    Vector3 offset = new Vector3(plano.x, 0, plano.y) * distancia;

    //    // Altura aleatoria
    //    float altura = Random.Range(alturaMinima, alturaMaxima);
    //    offset.y = altura;

    //    // Posición final: centro + offset
    //    objetivoActual = centro + offset;

    //    tiempoCambio = Random.Range(cambioDireccionCada * 0.7f, cambioDireccionCada * 1.3f);
    //}



    private void NuevoRumbo()
    {
        // Dirección 3D aleatoria y normalizada
        direccionActual = Random.onUnitSphere;

        // Evita direcciones demasiado verticales
        direccionActual.y = Mathf.Clamp(direccionActual.y, -0.2f, 0.4f); 

        // Altura objetivo
        alturaActual = Random.Range(alturaMinima, alturaMaxima);

        // Tiempo para el próximo cambio
        tiempoCambio = Random.Range(cambioDireccionCada * 0.7f, cambioDireccionCada * 1.3f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("[Espíritu] Colisión con cámara principal: " + other.name);
            // Si el espíritu entra en la zona segura, empujarlo hacia afuera
            Vector3 direccionSegura = (transform.position - other.transform.position).normalized;
            // Calcular el radio de la zona segura (extensión del collider + un margen)
            float radioZona = other.bounds.extents.magnitude + 0.5f;

            // Empuja al espíritu fuera de la zona segura
            transform.position = other.transform.position + direccionSegura * radioZona;

            // Forzar nuevo rumbo
            NuevoRumbo();
        }
    }


    public virtual void RecibirToque()
    {
        // Restar vida
        vida--;

        // Sonido
        AudioSource.PlayClipAtPoint(clipTocado, transform.position);

        // Cambiar color
        StartCoroutine(FlashParticulas(Color.red, 0.05f));

        // Si la vida llega a 0, destruir el espíritu
        if (vida <= 0)
        {
            Destruir();
        }
    }

    IEnumerator FlashParticulas(Color colorTemporal, float duracion)
    {
        var main = sistemaParticulas.main;

        // Guardar color original
        Color colorOriginal = main.startColor.color;

        // Aplicar color temporal
        main.startColor = colorTemporal;
        sistemaParticulas.Clear();
        sistemaParticulas.Play();

        yield return new WaitForSeconds(duracion);

        // Restaurar color original
        main.startColor = colorOriginal;
        sistemaParticulas.Clear();
        sistemaParticulas.Play();
    }

    protected virtual void Destruir()
    {
        PlayerPrefs.SetInt("Espiritus", PlayerPrefs.GetInt("Espiritus") + 1);
        PlayerPrefs.SetInt("PuntuacionPartida", PlayerPrefs.GetInt("PuntuacionPartida") + puntosEspiritu);
        PlayerPrefs.Save();
        SpawnerEspiritus.Instancia.EliminarEspiritu(gameObject);

        // Sonido de desvanecimiento
        AudioSource.PlayClipAtPoint(clipDesvanecer, transform.position);

        Destroy(gameObject);
    }
}