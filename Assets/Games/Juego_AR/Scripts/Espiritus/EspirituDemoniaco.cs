using UnityEngine;

public class EspirituDemoniaco : EspirituBase
{
    protected override void Start()
    {
        velocidadAngular = 15f;
        radio = 1.8f;
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        transform.Rotate(Vector3.up, 90 * Time.deltaTime); // efecto demoníaco
    }
}
