using UnityEngine;

public class EspirituBase : MonoBehaviour
{
    [SerializeField] protected float velocidadAngular;
    [SerializeField] protected float radio;
    [SerializeField] protected int vida;
    [SerializeField] protected int puntosEspiritu;

    protected Transform objetivoJugador;

    protected float anguloInicial;

    [SerializeField] private float velocidad;
    [SerializeField] private float cambioDireccionCada;
    [SerializeField] private float distanciaMinima;
    [SerializeField] private float distanciaMaxima;
    [SerializeField] private float alturaMinima;
    [SerializeField] private float alturaMaxima;

    private Vector3 objetivoActual;
    private Transform jugador;
    private float tiempoCambio;

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

    protected virtual void NuevoRumbo()
    {
        Vector3 centro = jugador.position;

        // Elegir dirección horizontal aleatoria
        Vector2 plano = Random.insideUnitCircle.normalized;
        float distancia = Random.Range(distanciaMinima, distanciaMaxima);

        Vector3 offset = new Vector3(plano.x, 0, plano.y) * distancia;

        // Añadir altura aleatoria
        float altura = Random.Range(alturaMinima, alturaMaxima);
        offset.y = altura;

        objetivoActual = centro + offset;
        tiempoCambio = Random.Range(cambioDireccionCada * 0.7f, cambioDireccionCada * 1.3f);
    }

    public virtual void RecibirToque()
    {
        vida--;

        // Efecto de golpe
        //Instantiate(efectoParticula, transform.position, Quaternion.identity);

        // Sonido
        //AudioSource.PlayClipAtPoint(clipTocado, transform.position);

        if (vida <= 0)
        {
            Destruir();
        }
    }

    protected virtual void Destruir()
    {
        PlayerPrefs.SetInt("Espiritus", PlayerPrefs.GetInt("Espiritus") + 1);
        PlayerPrefs.SetInt("PuntuacionPartida", PlayerPrefs.GetInt("PuntuacionPartida") + puntosEspiritu);
        PlayerPrefs.Save();
        SpawnerEspiritus.Instancia.EliminarEspiritu(gameObject);
        Destroy(gameObject);
    }
}