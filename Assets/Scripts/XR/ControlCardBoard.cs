//using UnityEngine;
//using Google.XR.Cardboard;


//public class ControlCardBoard : MonoBehaviour
//{
//    [Header("Elementos de XR")]
//    public GestorXR gestorXR;
//    public GameObject spawner;
//    public GameObject interacciones;

//    // Variables
//    private bool recentrado = false;
//    private int sleepInicial;
//    private float brilloInicial;
//
//    public void ActivarCardBoard()
//    {
//          StartCoroutine(ActivarOpenXRCoroutine());
//    }
//    public Ienumerator ActivarCardBoardCoroutine()
//    {
//        // Obtener el gestor de XR
//        if (ControlMenuPrincipal.InstanciaControl == null || ControlMenuPrincipal.InstanciaGestorXR == null)
//        {
//            if (ControlMenuPrincipal.InstanciaControl == null) Debug.LogError("[Control_OpenXR] No se encuentra instancia ControlMenuPrincioal.");
//            Debug.LogError("[Control_CardBoard] GestorXR no encontrado. Buscando en la escena...");
//            gestorXR = FindFirstObjectByType<GestorXR>();
//            if (gestorXR == null)
//            {
//                Debug.LogError("[Control_CardBoard] GestorXR sigue sin encontrarse.");
//                yield break;
//            }
//        }
//        else
//        {
//            gestorXR = ControlMenuPrincipal.Instance;
//        }

//        sleepInicial = Screen.sleepTimeout;
//        brilloInicial = Screen.brightness;

//        // Recentrar vista al iniciar a los 1.5 segundos
//        Invoke(nameof(RecentrarVista), 1.5f);

//        Screen.sleepTimeout = SleepTimeout.NeverSleep;
//        Screen.brightness = 1.0f;

//        // Desactiva todos los componentes de Cardboard
//        if (spawner != null) spawner.SetActive(false);
//        else Debug.LogError("[Control_CardBoard] spawner is null");
//        if (interacciones != null) interacciones.SetActive(false);
//        else Debug.LogError("[Control_CardBoard] interacciones is null");

//        yield return gestorXR.InicializarCardboard();

//        // Activar todos los componentes de Cardboard
//        if (spawner != null) spawner.SetActive(true);
//        else Debug.LogError("[Control_CardBoard] spawner is null");
//        if (interacciones != null) interacciones.SetActive(true);
//        else Debug.LogError("[Control_CardBoard] interacciones is null");
//    }

//    public void DesactivarCardBoard()
//    {
//        StartCoroutine(gestorXR.ApagarModoActual());

//        if (spawner != null) spawner.SetActive(false);
//        else Debug.LogError("[Control_CardBoard] spawner is null");
//        if (interacciones != null) interacciones.SetActive(false);
//        else Debug.LogError("[Control_CardBoard] interacciones is null");

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