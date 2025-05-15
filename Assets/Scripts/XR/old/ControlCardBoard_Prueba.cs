//using UnityEngine;
//using Google.XR.Cardboard;


//public class ControlCardBoard_Prueba : MonoBehaviour
//{
//    [Header("Controlador de XR")]
//    public ControlXR controlXR;
//    public GestorXR gestorXR;
//    [Header("Objetos a gestionar")]
//    public GameObject spawner;
//    public GameObject interacciones;

//    // Variables
//    private bool recentrado = false;
//    private int sleepInicial;
//    private float brilloInicial;

//    void Start()
//    {
//        gestorXR = GestorXR.Instance;
//        sleepInicial = Screen.sleepTimeout;
//        brilloInicial = Screen.brightness;
//        Invoke(nameof(RecentrarVista), 1.5f);
//    }

//    public void ActivarCardBoard()
//    {
//        Screen.sleepTimeout = SleepTimeout.NeverSleep;
//        Screen.brightness = 1.0f;

//        if (spawner != null) spawner.SetActive(false);
//        if (interacciones != null) interacciones.SetActive(false);

//        gestorXR.ActivarPlugin(GestorXR.XRMode.Cardboard);

//        if (spawner != null) spawner.SetActive(true);
//        if (interacciones != null) interacciones.SetActive(true);
//    }

//    public void DesactivarCardBoard()
//    {
//        gestorXR.ActivarPlugin(GestorXR.XRMode.None);

//        if (spawner != null) spawner.SetActive(false);
//        if (interacciones != null) interacciones.SetActive(false);

//        Screen.sleepTimeout = sleepInicial;
//        Screen.brightness = brilloInicial;

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

//        // Si pulsa el engranaje
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