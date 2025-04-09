using UnityEngine;

public class Brick : MonoBehaviour
{
    private ControlBreakout control;

    public void Inicializar(ControlBreakout controlBreakout)
    {
        control = controlBreakout;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Destroy(gameObject);
            control.SumarPuntuacion(10); // Incrementa la puntuación en 10 al destruir un ladrillo
        }


    }

}
