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

        float centroCamaraY = transform.position.y;
        float alturaJugador = target.position.y;

        if (puertaFinal != null)
        {
            float alturaPuerta = puertaFinal.position.y;

            // Solo mover la cámara si el jugador sube Y el centro de la cámara aún no ha alcanzado la puerta
            if (alturaJugador > centroCamaraY && centroCamaraY < alturaPuerta)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    alturaJugador,
                    transform.position.z
                );
            }
        }
        else
        {
            // Comportamiento normal sin puerta
            if (alturaJugador > centroCamaraY)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    alturaJugador,
                    transform.position.z
                );
            }
        }
    }

    private void Update()
    {
        // Buscar la puerta si aún no ha sido encontrada
        if (puertaFinal == null)
        {
            var puertaObj = FindFirstObjectByType<FinalPlatform>();
            if (puertaObj != null)
            {
                puertaFinal = puertaObj.transform;
                detenerAlCentrarPuerta = true;
                Debug.Log("[CameraFollow] Puerta final detectada por la cámara.");
            }
        }
    }


    public void ResetCameraPosition()
    {
        if (target != null)
        {
            transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
            puertaFinal = null;
            detenerAlCentrarPuerta = false;
            Debug.Log("[CameraFollow] Posición de cámara reiniciada.");
        }
    }

}
