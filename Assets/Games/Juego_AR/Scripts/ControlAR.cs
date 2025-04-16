using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Management;
using UnityEngine.Events;
public class ControlAR : MonoBehaviour
{
    public ARSession arSession;
    public XROrigin xrOrigin;
    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager;
    public ARCameraBackground arCameraBackground;
    public GameObject spawner;
    public GameObject interacciones;


    public void ActivarAR()
    {
        // Desactiva todos los componentes de AR
        if (arSession != null) arSession.gameObject.SetActive(false);
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(false);
        if (planeManager != null) planeManager.enabled = false;
        if (raycastManager != null) raycastManager.enabled = false;
        if (arCameraBackground != null) arCameraBackground.enabled = false;
        if (spawner != null) spawner.SetActive(false);
        if (interacciones != null) interacciones.SetActive(false);

        StartCoroutine(ActivarPluginXR());

    }

    private IEnumerator ActivarPluginXR()
    {
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("No se pudo inicializar XR.");
            yield break;
        }

        XRGeneralSettings.Instance.Manager.StartSubsystems();

        // Activa todo tras iniciar XR correctamente
        if (arSession != null) arSession.gameObject.SetActive(true);
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(true);
        if (planeManager != null) planeManager.enabled = true;
        if (raycastManager != null) raycastManager.enabled = true;
        if (arCameraBackground != null) arCameraBackground.enabled = true;
        if (spawner != null) spawner.SetActive(true);
        if (interacciones != null) interacciones.SetActive(true);

        Debug.Log("AR activado correctamente.");
    }

    public void DesactivarAR()
    {
        // Desactiva todos los componentes de AR
        if (arSession != null) arSession.gameObject.SetActive(false);
        if(xrOrigin != null) xrOrigin.gameObject.SetActive(false);
        if (planeManager != null) planeManager.enabled = false;
        if (raycastManager != null) raycastManager.enabled = false;
        if (arCameraBackground != null) arCameraBackground.enabled = false;
        if (spawner != null) spawner.SetActive(false);
        if (interacciones != null) interacciones.SetActive(false);

        DesactivarPluginXR();

    }

    private void DesactivarPluginXR()
    {
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("Plugin XR desactivado.");
    }
}
