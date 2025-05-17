using UnityEngine;

public class SeguirJugador : MonoBehaviour
{
    public Transform jugador; // arrástralo desde el editor
    public float velocidad = 2f;

    void Update()
    {
        if (jugador == null) return;

        // Mover la nube (y el enemigo con ella) hacia el jugador
        transform.position = Vector3.MoveTowards(
            transform.position,
            jugador.position,
            velocidad * Time.deltaTime
        );
    }
}
