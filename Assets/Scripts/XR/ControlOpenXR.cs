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

    public void ActivarAR()
    {
        gestorXR = GestorXR.Instance;

        // Desactiva todos los componentes de OpenXR
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(false);
        else Debug.LogError("[Control_AR] xrOrigin is null");
        if (spawner != null) spawner.SetActive(false);
        else Debug.LogError("[Control_AR] spawner is null");
        if (interacciones != null) interacciones.SetActive(false);
        else Debug.LogError("[Control_AR] interacciones is null");

        StartCoroutine(gestorXR.InicializarARCore());


        // Activar todos los componentes de OpenXR
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(true);
        else Debug.LogError("[Control_AR] xrOrigin is null");
        if (spawner != null) spawner.SetActive(true);
        else Debug.LogError("[Control_AR] spawner is null");
        if (interacciones != null) interacciones.SetActive(true);
        else Debug.LogError("[Control_AR] interacciones is null");
    }

    public void DesactivarAR()
    {
        StartCoroutine(gestorXR.ApagarModoActual());

        // Desactiva todos los componentes de OpenXR
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(false);
        if (spawner != null) spawner.SetActive(false);
        if (interacciones != null) interacciones.SetActive(false);

    }

}
