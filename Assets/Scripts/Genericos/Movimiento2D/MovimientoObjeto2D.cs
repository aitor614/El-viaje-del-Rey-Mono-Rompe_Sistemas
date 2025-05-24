using UnityEngine;

public class MovimientoObjeto2D : MonoBehaviour
{
    [Header("Velocidad movimiento")]
    public float velocidad = 5f;
    [Header("Direccion movimiento")]
    public DireccionMovimiento direccionMovimiento;
    public enum DireccionMovimiento
    {
        Izquierda, Derecha, Arriba, Abajo
    }


    private float bordeIzquierdo;
    private float bordeDerecho;
    private float bordeSuperior;
    private float bordeInferior;

    private void Start()
    {
        // Calcula el borde izquierdo de la pantalla en coordenadas del mundo
        if (direccionMovimiento == DireccionMovimiento.Izquierda) bordeIzquierdo = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 11f;
        // Calcula el borde derecho de la pantalla en coordenadas del mundo
        if (direccionMovimiento == DireccionMovimiento.Derecha) bordeDerecho = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + 11f;
        // Calcula el borde superior de la pantalla en coordenadas del mundo
        if (direccionMovimiento == DireccionMovimiento.Arriba) bordeSuperior = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y + 11f;
        // Calcula el borde inferior de la pantalla en coordenadas del mundo
        if (direccionMovimiento == DireccionMovimiento.Abajo) bordeInferior = Camera.main.ScreenToWorldPoint(Vector3.zero).y - 11f;

    }

    private void Update()
    {
        Movimiento();

        DestruirFueraPantalla();
    }

    private void Movimiento()
    {
        // Mueve el objeto en la dirección especificada
        switch (direccionMovimiento)
        {
            case DireccionMovimiento.Izquierda:
                transform.position += Time.deltaTime * velocidad * Vector3.left;
                break;
            case DireccionMovimiento.Derecha:
                transform.position += Time.deltaTime * velocidad * Vector3.right;
                break;
            case DireccionMovimiento.Arriba:
                transform.position += Time.deltaTime * velocidad * Vector3.up;
                break;
            case DireccionMovimiento.Abajo:
                transform.position += Time.deltaTime * velocidad * Vector3.down;
                break;
        }
    }

    private void DestruirFueraPantalla()
    {
        // Si el objeto sale del borde de la pantalla al que se mueve, se destruye
        if (direccionMovimiento == DireccionMovimiento.Izquierda && transform.position.x < bordeIzquierdo) Destroy(gameObject);
        if (direccionMovimiento == DireccionMovimiento.Derecha && transform.position.x > bordeDerecho) Destroy(gameObject);
        if (direccionMovimiento == DireccionMovimiento.Arriba && transform.position.y > bordeSuperior) Destroy(gameObject);
        if (direccionMovimiento == DireccionMovimiento.Abajo && transform.position.y < bordeInferior) Destroy(gameObject);

    }
}
