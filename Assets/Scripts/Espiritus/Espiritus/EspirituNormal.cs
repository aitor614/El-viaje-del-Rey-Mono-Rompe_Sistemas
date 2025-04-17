using UnityEngine;

public class EspirituNormal : EspirituBase
{
    protected override void Start()
    {
        velocidadAngular = 30f;
        radio = 2.5f;
        base.Start();
    }
}