using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;

public static class XRControlador
{
    public static IEnumerator IniciarXR()
    {
        Debug.Log("Inicializando XR...");

        if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
            Debug.Log("XR ya estaba inicializado.");
            yield break;
        }

        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Error al inicializar XR.");
        }
        else
        {
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            Debug.Log("XR activo.");
        }
    }

    public static void DetenerXR()
    {
        Debug.Log("Apagando XR...");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
    }
}