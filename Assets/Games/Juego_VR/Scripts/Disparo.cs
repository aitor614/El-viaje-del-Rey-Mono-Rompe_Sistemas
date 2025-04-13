using UnityEngine;

public class Disparo : MonoBehaviour
{
    public GameObject proyectilPrefab;  // Prefab del proyectil (esfera)
    public Transform puntoDisparo;      // Punto desde donde se disparará el proyectil
    public float fuerza = 1000f;         // Fuerza con la que se disparará el proyectil

    void Update()
    {
        // Detecta si el jugador presiona el botón de disparo
        if (Input.GetButtonDown("Fire1"))
        {
            // Crea un proyectil en el punto de disparo con la misma rotación que el punto
            GameObject disparo = Instantiate(proyectilPrefab, puntoDisparo.position, puntoDisparo.rotation);

            // Añade una fuerza al proyectil para que se mueva
            disparo.GetComponent<Rigidbody>().AddForce(puntoDisparo.forward * fuerza);
        }
    }
}
