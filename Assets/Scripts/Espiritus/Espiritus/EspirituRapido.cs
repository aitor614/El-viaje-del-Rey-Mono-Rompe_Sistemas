using UnityEngine;

public class EspirituRapido : EspirituBase
{
    protected override void Start()
    {
        velocidadAngular = 60f;
        radio = 2.8f;
        base.Start();
    }
}
