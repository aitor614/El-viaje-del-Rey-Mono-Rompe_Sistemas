using System.Collections;
using UnityEngine;

public class EspirituBase : MonoBehaviour
{
    [Header("Atributos")]
    public float velocidadAngular;
    public float radio;
    public int vida;
    public int puntosEspiritu;

    [Header("Movimiento & Posición")]
    public float velocidad;
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
    private Vector3 objetivoActual;
    private Transform jugador;
    private float tiempoCambio;
    private Transform objetivoJugador;
    private float anguloInicial;

    protected virtual void Start()
    {
        jugador = Camera.main.transform;
        NuevoRumbo();
    }

    protected virtual void Update()
    {
        if (jugador == null) return;

        // Moverse suavemente hacia el punto objetivo
        transform.position = Vector3.Lerp(transform.position, objetivoActual, velocidad * Time.deltaTime);

        // Si está cerca o ha pasado un tiempo, cambiar objetivo
        tiempoCambio -= Time.deltaTime;
        if (Vector3.Distance(transform.position, objetivoActual) < 0.2f || tiempoCambio <= 0f)
        {
            NuevoRumbo();
        }
    }

    //protected virtual void NuevoRumbo()
    //{
    //    Vector3 centro = jugador.position;

    //    // Elegir dirección horizontal aleatoria
    //    Vector2 plano = Random.insideUnitCircle.normalized;
    //    float distancia = Random.Range(distanciaMinima, distanciaMaxima);

    //    Vector3 offset = new Vector3(plano.x, 0, plano.y) * distancia;

    //    // Añadir altura aleatoria
    //    float altura = Random.Range(alturaMinima, alturaMaxima);
    //    offset.y = altura;

    //    objetivoActual = centro + offset;
    //    tiempoCambio = Random.Range(cambioDireccionCada * 0.7f, cambioDireccionCada * 1.3f);
    //}

    protected virtual void NuevoRumbo()
    {
        if (jugador == null) return;

        Vector3 centro = jugador.position;

        // Elegir dirección horizontal aleatoria normalizada
        Vector2 plano = Random.insideUnitCircle.normalized;

        // Elegir distancia garantizando un anillo entre mínima y máxima
        float distancia = Random.Range(distanciaMinima, distanciaMaxima);
        Vector3 offset = new Vector3(plano.x, 0, plano.y) * distancia;

        // Altura aleatoria
        float altura = Random.Range(alturaMinima, alturaMaxima);
        offset.y = altura;

        // Posición final: centro + offset
        objetivoActual = centro + offset;

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
        vida--;

        // Sonido
        audioSource.PlayOneShot(clipTocado);

        // Cambiar color
        StartCoroutine(FlashParticulas(Color.red, 0.05f));

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