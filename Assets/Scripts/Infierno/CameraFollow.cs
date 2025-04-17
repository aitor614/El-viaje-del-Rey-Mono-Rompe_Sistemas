using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Transform puertaFinal;
    private Camera camara;
    private bool detenerAlCentrarPuerta = false;

    private void Start()
    {
        camara = Camera.main;
    }

    void LateUpdate()
    {
        if (camara == null || target == null) return;

        // Buscar la puerta si aún no ha sido encontrada
        if (puertaFinal == null)
        {
            GameObject puertaFinal = FindFirstObjectByType<FinalPlatform>().gameObject;
            if (puertaFinal != null)
            {
                this.puertaFinal = puertaFinal.transform;
                detenerAlCentrarPuerta = true;
                Debug.Log("Puerta final detectada por la cámara.");
            }
        }

        float alturaCamara = camara.orthographicSize;
        float centroCamaraY = transform.position.y + alturaCamara;

        if (detenerAlCentrarPuerta && puertaFinal != null)
        {
            float centroPuertaY = puertaFinal.position.y;

            if (target.position.y > transform.position.y && centroCamaraY < centroPuertaY)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    target.position.y,
                    transform.position.z
                );
            }

            if (centroCamaraY >= centroPuertaY)
            {
                detenerAlCentrarPuerta = false;
                Debug.Log("Cámara detenida al centrar la puerta.");
            }
        }
        else
        {
            // Movimiento normal sin puerta
            if (target.position.y > transform.position.y)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    target.position.y,
                    transform.position.z
                );
            }
        }
    }

    public void ResetCameraPosition()
    {
        if (target != null)
        {
            transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
        }
    }

}
