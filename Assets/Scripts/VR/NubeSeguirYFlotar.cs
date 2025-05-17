using UnityEngine;

public class NubeSeguirYFlotar : MonoBehaviour
{
    public Transform jugador;
    public float velocidadSeguir = 2f;

    public float alturaFlotacion = 0.5f;
    public float velocidadFlotacion = 1.5f;

    private Vector3 posicionBase;

    void Start()
    {
        posicionBase = transform.position;
    }

    void Update()
    {
        if (jugador == null) return;

        // Posición objetivo 0.5 metros más abajo
        Vector3 objetivoConOffset = jugador.position;
        objetivoConOffset.y -= 1.5f;

        // Mover hacia el objetivo con offset
        posicionBase = Vector3.MoveTowards(posicionBase, objetivoConOffset, velocidadSeguir * Time.deltaTime);

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
