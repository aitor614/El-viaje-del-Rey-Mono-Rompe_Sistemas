//using UnityEngine;
//using UnityEngine.XR.Management;
//using Google.XR.Cardboard;
//using System.Collections;

//public class ControlCardBoard : MonoBehaviour
//{
//    private bool recentrado = false;

//    void Start()
//    {
//        Screen.sleepTimeout = SleepTimeout.NeverSleep;
//        Screen.brightness = 1.0f;

//        Invoke(nameof(RecentrarVista), 1.5f);
//        StartCoroutine(StartXR());
//    }

//    private IEnumerator StartXR()
//    {
//        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
//        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
//        {
//            Debug.LogError("No se pudo iniciar Cardboard XR loader.");
//            yield break;
//        }

//        XRGeneralSettings.Instance.Manager.StartSubsystems();
//        Debug.Log("Cardboard XR iniciado.");
//    }

//    public void DesactivarCardBoard()
//    {
//        StopXR();
//    }
//    public void ActivarCardBoard()
//    {
//        StartCoroutine(StartXR());
//    }

//    private void StopXR()
//    {
//        XRGeneralSettings.Instance.Manager.StopSubsystems();
//        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
//        Debug.Log("Cardboard XR detenido.");
//    }

//    private void RecentrarVista()
//    {
//        if (!recentrado)
//        {
//            Api.Recenter();
//            recentrado = true;
//        }
//    }

//    void Update()
//    {
//#if !UNITY_EDITOR
//        // Si el usuario pulsa el botón de cerrar
//        if (Api.IsCloseButtonPressed)
//        {
//            Application.Quit(); // O vuelve al menú, si prefieres
//        }

//        // Si pulsa el engranaje (aunque no uses QR)
//        if (Api.IsGearButtonPressed)
//        {
//            Api.ScanDeviceParams();
//        }

//        // Recargar parámetros si se ha escaneado un nuevo QR
//        if (Api.HasNewDeviceParams())
//        {
//            Api.ReloadDeviceParams();
//        }

//        // Permitir recentrado manual con el trigger
//        if (Api.IsTriggerHeldPressed)
//        {
//            Api.Recenter();
//        }

//        // Mantener los parámetros de pantalla actualizados
//        Api.UpdateScreenParams();
//#endif
//    }
//}