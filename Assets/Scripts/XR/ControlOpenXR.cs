using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Management;

public class ControlOpenXR : MonoBehaviour
{
    public XROrigin xrOrigin;
    public GameObject spawner;
    public GameObject interacciones;
    public GestorXR gestorXR;

    public void ActivarOpenXR()
    {
        gestorXR = GestorXR.Instance;

        // Desactiva todos los componentes de OpenXR
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(false);
        else Debug.LogError("[Control_OpenXR] xrOrigin is null");
        if (spawner != null) spawner.SetActive(false);
        else Debug.LogError("[Control_OpenXR] spawner is null");
        if (interacciones != null) interacciones.SetActive(false);
        else Debug.LogError("[Control_OpenXR] interacciones is null");

        StartCoroutine(gestorXR.InicializarOpenXR());


        // Activar todos los componentes de OpenXR
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(true);
        else Debug.LogError("[Control_OpenXR] xrOrigin is null");
        if (spawner != null) spawner.SetActive(true);
        else Debug.LogError("[Control_OpenXR] spawner is null");
        if (interacciones != null) interacciones.SetActive(true);
        else Debug.LogError("[Control_OpenXR] interacciones is null");
    }

    public void DesactivarOpenXR()
    {
        StartCoroutine(gestorXR.ApagarModoActual());

        // Desactiva todos los componentes de OpenXR
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(false);
        if (spawner != null) spawner.SetActive(false);
        if (interacciones != null) interacciones.SetActive(false);

    }

}
