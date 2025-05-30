using UnityEngine;

public class EspirituDemoniaco : EspirituBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        // Rotar el espíritu demoniaco continuamente
        transform.Rotate(Vector3.up, 90 * Time.deltaTime);
    }
}
