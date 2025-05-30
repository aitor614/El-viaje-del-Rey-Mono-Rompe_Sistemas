using System.Runtime.InteropServices;
using UnityEngine;

public class EspirituIlusorio : EspirituBase
{

    [Header("Escala dinámica")]
    public float escalaMinima;
    public float escalaMaxima;
    public float velocidadOscilacion;

    protected override void Start()
    {
        base.Start();
        CambiarRadio();
    }

    private void CambiarRadio()
    {
        // Esto hace que la escala oscile entre escalaMinima y escalaMaxima
        // usando Mathf.PingPong para crear un efecto de oscilación suave
        // y Mathf.Lerp para interpolar entre las dos escalas (ir de un valor a otro)

        // Va de 0 a 1 en un ciclo de ping-pong
        float t = Mathf.PingPong(Time.time * velocidadOscilacion, 1f);

        // Interpolación entre la escala mínima y máxima
        float escala = Mathf.Lerp(escalaMinima, escalaMaxima, t);

        // Aplicar la escala al transform del espíritu
        transform.localScale = Vector3.one * escala;
    }
}