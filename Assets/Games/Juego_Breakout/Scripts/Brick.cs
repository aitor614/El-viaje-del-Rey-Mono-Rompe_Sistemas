using UnityEngine;

public class Brick : MonoBehaviour
{
    private ControlBreakout control;

    // Start is called before the first frame update
    private void Start()
    {
        control = ControlBreakout.InstanciaControl;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Destroy(gameObject);
            // Incrementa la puntuación en 10 al destruir un ladrillo
            control.SumarPuntuacion(10); 
        }


    }

}
