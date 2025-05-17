using UnityEngine;

public class NubeFlotante : MonoBehaviour
{
    public float alturaFlotacion = 0.5f;
    public float velocidadFlotacion = 1.5f;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.localPosition;
    }

    void Update()
    {
        float nuevaAltura = Mathf.Sin(Time.time * velocidadFlotacion) * alturaFlotacion;
        transform.localPosition = new Vector3(posicionInicial.x, posicionInicial.y + nuevaAltura, posicionInicial.z);
    }
}

