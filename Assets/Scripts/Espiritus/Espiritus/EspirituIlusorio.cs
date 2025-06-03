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
        // No va..
    }
}