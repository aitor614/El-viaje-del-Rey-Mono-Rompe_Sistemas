using UnityEngine;

public class NubeSeguir : MonoBehaviour
{
    public Transform jugador;
    public float velocidadSeguir = 5f;

    public float alturaFlotacion = 0.3f;
    public float velocidadFlotacion = 1.5f;

    public float distanciaMinima = 1.5f; // Distancia mínima a la que se detiene la nube

    private Vector3 posicionBase;

    void Start()
    {
        posicionBase = transform.position;
    }

    void Update()
    {
        if (jugador == null) return;

        // Posición objetivo con offset hacia abajo
        Vector3 objetivoConOffset = jugador.position;
        objetivoConOffset.y -= 2f;

        // Calculamos la distancia desde la posición base actual al objetivo
        Vector3 direccion = objetivoConOffset - posicionBase;
        float distancia = direccion.magnitude;

        if (distancia > distanciaMinima)
        {
            // La nube se mueve hacia el jugador pero solo hasta 3 metros de distancia
            Vector3 movimiento = direccion.normalized * velocidadSeguir * Time.deltaTime;

            // No pasar la distancia mínima
            if (movimiento.magnitude > distancia - distanciaMinima)
            {
                movimiento = direccion.normalized * (distancia - distanciaMinima);
            }

            posicionBase += movimiento;
        }

        // Altura flotante
        float altura = Mathf.Sin(Time.time * velocidadFlotacion) * alturaFlotacion;

        // Aplicar posición combinada
        transform.position = new Vector3(
            posicionBase.x,
            posicionBase.y + altura,
            posicionBase.z
        );
    }
}
/*using UnityEngine;

public class NubeSeguir : MonoBehaviour
{
    public Transform jugador;
    public float velocidadSeguir = 2f;
    public float distanciaMinima = 1f;  // Distancia mínima a la que se detiene

    public bool EstaCercaDelJugador { get; private set; } = false;

    void Update()
    {
        if (jugador == null) return;

        Vector3 direccion = jugador.position - transform.position;
        float distancia = direccion.magnitude;

        if (distancia > distanciaMinima)
        {
            EstaCercaDelJugador = false;
            Vector3 movimiento = direccion.normalized * velocidadSeguir * Time.deltaTime;
            if (movimiento.magnitude > distancia - distanciaMinima)
                movimiento = direccion.normalized * (distancia - distanciaMinima);

            transform.position += movimiento;
        }
        else
        {
            // Estamos dentro de la distancia mínima
            EstaCercaDelJugador = true;
        }
    }
}*/
