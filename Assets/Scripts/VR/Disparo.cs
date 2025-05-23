using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;

public class Disparo : MonoBehaviour
{
    [Header("Componentes")]
    public GameObject proyectilPrefab;

    [Header("Punto de disparo")]
    public Transform puntoDisparoXR;
    public Transform puntoDisparoCardboard;
    public Transform puntoDisparoAR;

    [Header("Sonido")]
    public AudioSource sonidoDisparo; // ← Nuevo campo para el sonido

    [Header("Parámetros")]
    public float fuerzaProyectil;
    public int maximoProyectiles;

    // Variables
    private List<GameObject> proyectiles;
    private Transform puntoDisparo;
    UnityEngine.XR.InputDevice dispositivoManoDerecha;

    void Start()
    {
        dispositivoManoDerecha = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (GestorXR.InstanciaGestorXR.modoInicial == GestorXR.XRMode.Cardboard)
        {
            puntoDisparo = puntoDisparoCardboard;
            Debug.Log("[Disparo] Modo Cardboard");
        }
        else if (GestorXR.InstanciaGestorXR.modoInicial == GestorXR.XRMode.OpenXR)
        {
            puntoDisparo = puntoDisparoXR;
            Debug.Log("[Disparo] Modo OpenXR");
        }
        else if (GestorXR.InstanciaGestorXR.modoInicial == GestorXR.XRMode.ARCore)
        {
            puntoDisparo = puntoDisparoAR;
            Debug.Log("[Disparo] Modo ARCore");
        }
        else
        {
            puntoDisparo = puntoDisparoAR;
            Debug.Log("[Disparo] Modo Editor");
        }
    }

    void Update()
    {
        // Modo Editor (Mouse izquierdo)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            RealizarDisparo();
        }
        // Modo OpenXR (gatillo derecho)
        else if (dispositivoManoDerecha.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool presionado) && presionado)
        {
            RealizarDisparo();
        }
        // Modo ARCore (toque en pantalla)
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            RealizarDisparo();
        }

        //// En Cardboard (gatillo)
        //if (Google.XR.Cardboard.Api.IsTriggerPressed)
        //{
        //    RealizarDisparo();
        //}
    }

    private void RealizarDisparo()
    {
        proyectiles ??= new List<GameObject>();

        if (proyectilPrefab == null || puntoDisparo == null)
        {
            Debug.LogWarning("Prefab o punto de disparo no asignados.");
            return;
        }

        if (proyectiles.Count >= maximoProyectiles)
        {
            Debug.LogWarning("Destruyendo proyectil más antiguo.");
            Destroy(proyectiles[0]);
            proyectiles.RemoveAt(0); // También hay que quitarlo de la lista
        }

        GameObject disparo = Instantiate(proyectilPrefab, puntoDisparo.position, puntoDisparo.rotation);
        disparo.GetComponent<Rigidbody>().AddForce(puntoDisparo.forward * fuerzaProyectil);
        proyectiles.Add(disparo);

        // 🔊 Reproducir sonido de disparo
        if (sonidoDisparo != null)
        {
            sonidoDisparo.Play();
        }
    }

}