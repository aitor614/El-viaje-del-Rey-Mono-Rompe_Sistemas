using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Management;
public class ControlVR : MonoBehaviour
{
    [Header("Componentes")]
    private XROrigin xrOrigin;
    private GameObject HUD;
    private GameObject spawner;
    private GameObject interacciones;


    public void ActivarVR()
    {
        // Desactiva todos los componentes de VR
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(false);
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
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(true);
        if (spawner != null) spawner.SetActive(true);
        if (interacciones != null) interacciones.SetActive(true);

        Debug.Log("AR activado correctamente.");
    }

    public void DesactivarVR()
    {
        // Desactiva todos los componentes de VR
        if(xrOrigin != null) xrOrigin.gameObject.SetActive(false);
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
