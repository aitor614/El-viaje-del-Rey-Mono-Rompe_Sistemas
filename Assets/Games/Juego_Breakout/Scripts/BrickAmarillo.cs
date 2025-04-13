using UnityEngine;

public class BrickAmarillo : BrickBase
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
            PlayerPrefs.SetInt("Ladrillos", PlayerPrefs.GetInt("Ladrillos") + 1);
            PlayerPrefs.SetInt("PuntuacionPartida", PlayerPrefs.GetInt("PuntuacionPartida") + 10);
            PlayerPrefs.Save();
        }
    }
}
