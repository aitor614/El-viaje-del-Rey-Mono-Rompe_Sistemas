using UnityEngine;

public class EspirituIlusorio : EspirituBase
{
    protected override void Start()
    {
        velocidadAngular = 25f;
        base.Start();
        InvokeRepeating(nameof(CambiarRadio), 2f, 3f);
    }

    private void CambiarRadio()
    {
        radio = Random.Range(1.5f, 3.5f);
    }
}