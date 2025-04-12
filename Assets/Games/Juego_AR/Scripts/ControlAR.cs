using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Management;

public class ControlAR : MonoBehaviour
{
    [SerializeField] private ARSession arSession;
    [SerializeField] private XROrigin xrOrigin;
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARCameraBackground arCameraBackground;

    public void ActivarAR()
    {
        // Desactiva todos los componentes antes de iniciar XR
        arSession.gameObject.SetActive(false);
        xrOrigin.gameObject.SetActive(false);
        planeManager.enabled = false;
        raycastManager.enabled = false;
        arCameraBackground.enabled = false;

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
        arSession.gameObject.SetActive(true);
        xrOrigin.gameObject.SetActive(true);
        planeManager.enabled = true;
        raycastManager.enabled = true;
        arCameraBackground.enabled = true;

        Debug.Log("AR activado correctamente.");
    }

    public void DesactivarAR()
    {         
        // Desactiva todos los componentes de AR
        arSession.gameObject.SetActive(false);
        xrOrigin.gameObject.SetActive(false);
        planeManager.enabled = false;
        raycastManager.enabled = false;
        arCameraBackground.enabled = false;

        DesactivarPluginXR();
    }

    private void DesactivarPluginXR()
    {
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("Plugin XR desactivado.");
    }
}
