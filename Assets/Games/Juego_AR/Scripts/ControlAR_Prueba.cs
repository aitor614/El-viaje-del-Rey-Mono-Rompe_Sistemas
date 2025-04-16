using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Management;

public class ControlAR_Prueba : MonoBehaviour
{
    public ARSession arSession;
    public XROrigin xrOrigin;
    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager;
    public ARCameraBackground arCameraBackground;
    public GameObject spawner;
    public GameObject interacciones;
    public GestorXR gestorXR;

    public void ActivarAR()
    {
        gestorXR = GestorXR.Instance;
        // Desactiva todos los componentes de AR mientras esperamos
        if (arSession != null) arSession.gameObject.SetActive(false);
        else Debug.LogError("[Control_AR] arSession is null");
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(false);
        else Debug.LogError("[Control_AR] xrOrigin is null");
        if (planeManager != null) planeManager.enabled = false;
        else Debug.LogError("[Control_AR] planeManager is null");
        if (raycastManager != null) raycastManager.enabled = false;
        else Debug.LogError("[Control_AR] raycastManager is null");
        if (arCameraBackground != null) arCameraBackground.enabled = false;
        else Debug.LogError("[Control_AR] arCameraBackground is null");
        if (spawner != null) spawner.SetActive(false);
        else Debug.LogError("[Control_AR] spawner is null");
        if (interacciones != null) interacciones.SetActive(false);
        else Debug.LogError("[Control_AR] interacciones is null");

        StartCoroutine(gestorXR.InitializeARCore());


        // Activar todos los componentes de AR
        if (arSession != null) arSession.gameObject.SetActive(true);
        else Debug.LogError("[Control_AR] arSession is null");
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(true);
        else Debug.LogError("[Control_AR] xrOrigin is null");
        if (planeManager != null) planeManager.enabled = true;
        else Debug.LogError("[Control_AR] planeManager is null");
        if (raycastManager != null) raycastManager.enabled = true;
        else Debug.LogError("[Control_AR] raycastManager is null");
        if (arCameraBackground != null) arCameraBackground.enabled = true;
        else Debug.LogError("[Control_AR] arCameraBackground is null");
        if (spawner != null) spawner.SetActive(true);
        else Debug.LogError("[Control_AR] spawner is null");
        if (interacciones != null) interacciones.SetActive(true);
        else Debug.LogError("[Control_AR] interacciones is null");
    }

    public void DesactivarAR()
    {
        StartCoroutine(gestorXR.ShutdownCurrentMode());

        // Desactiva todos los componentes de AR
        if (arSession != null) arSession.gameObject.SetActive(false);
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(false);
        if (planeManager != null) planeManager.enabled = false;
        if (raycastManager != null) raycastManager.enabled = false;
        if (arCameraBackground != null) arCameraBackground.enabled = false;
        if (spawner != null) spawner.SetActive(false);
        if (interacciones != null) interacciones.SetActive(false);

    }

}
