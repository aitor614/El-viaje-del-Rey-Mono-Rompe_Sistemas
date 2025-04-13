using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Disparo : MonoBehaviour
{
    [Header("Componentes")]
    public GameObject proyectilPrefab; 
    public Transform puntoDisparo;
    public InputActionReference disparoAction;

    [Header("Parámetros")]
    public float fuerzaProyectil = 1000f;
    public int maximoProyectiles = 10;

    // Variables
    private List<GameObject> proyectiles; 


    void Update()
    {
#if UNITY_EDITOR
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RealizarDisparo();
        }
#endif

    }

    private void OnEnable()
    {
        if (disparoAction != null)
            disparoAction.action.performed += OnDisparo;
    }

    private void OnDisable()
    {
        if (disparoAction != null)
            disparoAction.action.performed -= OnDisparo;
    }

    private void OnDisparo(InputAction.CallbackContext ctx)
    {
        RealizarDisparo();
    }

    private void RealizarDisparo()
    {
        // Crear array de proyectiles si no existe
        proyectiles ??= new List<GameObject>();
        // Comprobar si el prefab y el punto de disparo están asignados
        if (proyectilPrefab == null || puntoDisparo == null)
        {
            Debug.LogWarning("Prefab o punto de disparo no asignados.");
            return;
        }
        // Comprobar si el número máximo de proyectiles ha sido alcanzado
        if (proyectiles.Count >= maximoProyectiles)
        {
            Debug.LogWarning("Destruyendo proyectil más antiguo.");
            Destroy(proyectiles[0]); // Destruir el proyectil más antiguo
        }
        // Crea un proyectil en el punto de disparo con la misma rotación que el punto
        GameObject disparo = Instantiate(proyectilPrefab, puntoDisparo.position, puntoDisparo.rotation);

        // Añade una fuerza al proyectil para que se mueva
        disparo.GetComponent<Rigidbody>().AddForce(puntoDisparo.forward * fuerzaProyectil);

        // Añade el proyectil al array
        proyectiles.Add(disparo);
    }
}
