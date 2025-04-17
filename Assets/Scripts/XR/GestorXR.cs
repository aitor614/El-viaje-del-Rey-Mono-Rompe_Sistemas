using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;
using Google.XR.Cardboard;

public class GestorXR : MonoBehaviour
{
    public static GestorXR Instance { get; private set; }

    // Enum para los modos XR
    public enum XRMode { None, Cardboard, ARCore }

    [Header("Configuración inicial XR")]
    public XRMode initialMode = XRMode.None;

    // Cardboard
    private const float _campoVisionDefecto = 60f;

    // Cámaras
    private Camera _mainCamera;
    private Camera _xrCamera;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Verificar si XRGeneralSettings existe
            if (XRGeneralSettings.Instance == null)
            {
                Debug.LogError("[GESTOR XR] XRGeneralSettings no encontrado.");
                return;
            }

            StartCoroutine(ApagarModoActual());
            InicializarEntorno2D();

            //SceneManager.sceneLoaded += AlCargarEscena;
            //_mainCamera = Camera.main;
            //SetupXRInput();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(InicializarCorrutinaXR(initialMode));
    }

    private void AlCargarEscena(Scene scene, LoadSceneMode mode)
    {
        DetermineInitialMode(scene);
        StartCoroutine(InicializarCorrutinaXR(initialMode));
    }

    public void ActivarPlugin(XRMode xRMode)
    {
        initialMode = xRMode;
        StartCoroutine(InicializarCorrutinaXR(initialMode));
    }

    private void DetermineInitialMode(Scene scene)
    {
        if (scene.name.Contains("AR")) initialMode = XRMode.ARCore;
        else if (scene.name.Contains("VR")) initialMode = XRMode.Cardboard;
        else initialMode = XRMode.None;
    }

    public void CambiarModoXR(XRMode nuevoModo)
    {
        StartCoroutine(SwitchModeCoroutine(nuevoModo));
    }

    private IEnumerator SwitchModeCoroutine(XRMode nuevoModo)
    {
        yield return ApagarModoActual();
        initialMode = nuevoModo;
        yield return InicializarCorrutinaXR(nuevoModo);
    }

    private IEnumerator InicializarCorrutinaXR(XRMode mode)
    {
        switch (mode)
        {
            case XRMode.Cardboard:
                yield return InicializarCardboard();
                break;

            case XRMode.ARCore:
                yield return InicializarARCore();
                break;

            default:
                InicializarEntorno2D();
                break;
        }
    }

    public IEnumerator InicializarCardboard()
    {
        // Obtener lista completa de loaders
        var loaders = XRGeneralSettings.Instance.Manager.activeLoaders;

        // Reordenar la lista de loaders para que Cardboard sea el primero
        var loadersReordenados = new List<UnityEngine.XR.Management.XRLoader>();
        foreach ( var loader in loaders)
        {
            if (loader is Google.XR.Cardboard.XRLoader)
            {
                loadersReordenados.Insert(0, loader);

                break;
            }
            else if (loader is UnityEngine.XR.ARCore.ARCoreLoader)
            {
                loadersReordenados.Insert(loadersReordenados.Count, loader);
            }
        }
        // Mostrar loaders cargados
        Debug.Log("[GESTOR XR] Cargando " + loadersReordenados.Count + " loaders: ");
        for (int i = 0; i < loadersReordenados.Count; i++)
        {
            Debug.Log("[GESTOR XR] Loader " + i + ": " + loadersReordenados[i].name);
        }

        yield return StartCoroutine(XRGeneralSettings.Instance.Manager.InitializeLoader());
        XRGeneralSettings.Instance.Manager.StartSubsystems();

        // Configurar parámetros y cámara
        if (!Api.HasDeviceParams()) Api.ScanDeviceParams();
        DeshabilitarCamaraExistente();
        ConfigurarInputCardboard();
    }

    private void ConfigurarInputCardboard()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.brightness = 1.0f;
    }

    private void Update()
    {
        if (XRGeneralSettings.Instance.Manager.activeLoader is Google.XR.Cardboard.XRLoader)
        {
            ControlInputCardboard();
        }
    }

    private void ControlInputCardboard()
    {
        if (Api.IsCloseButtonPressed) SalirVR();
        if (Api.IsGearButtonPressed) Api.ScanDeviceParams();
        if (Api.IsTriggerHeldPressed) Api.Recenter();
        Api.UpdateScreenParams();
    }

    public IEnumerator InicializarARCore()
    {
        // Desactivar subsistemas de ARCore
        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            Debug.Log("[GESTOR XR] Desactivando ActiveLoader.");
            yield return ApagarModoActual();
        }
        // Obtener lista completa de loaders
        var loaders = XRGeneralSettings.Instance.Manager.activeLoaders;

        // Reordenar la lista de loaders para que ARCore sea el primero
        var loadersReordenados = new List<UnityEngine.XR.Management.XRLoader>();
        foreach (var loader in loaders)
        {
            if (loader is UnityEngine.XR.ARCore.ARCoreLoader)
            {
                loadersReordenados.Insert(0, loader);
            }
            else if (loader is Google.XR.Cardboard.XRLoader)
            {
                loadersReordenados.Insert(loadersReordenados.Count, loader);
            }
        }
        Debug.Log("[GESTOR XR] Cargando " + loadersReordenados.Count + " loaders: ");
        for (int i = 0; i < loadersReordenados.Count; i++)
        {
            Debug.Log("[GESTOR XR] Loader " + i + ": " + loadersReordenados[i].name);
        }

        // Inicializar loader y subsistemas
        yield return StartCoroutine(XRGeneralSettings.Instance.Manager.InitializeLoader());
        XRGeneralSettings.Instance.Manager.StartSubsystems();
        
    }

    public IEnumerator ApagarModoActual()
    {
        var manager = XRGeneralSettings.Instance.Manager;

        // Si no hay XRGeneralSettings, salir
        if (manager == null)
        {
            Debug.LogError("[GESTOR XR] XRGeneralSettings no encontrado.");
            yield break;
        }

        if (manager.activeLoader != null)
        {
            Debug.Log("[GESTOR XR] Apagando loader activo: " + manager.activeLoader.GetType().Name);

            // Detener subsistemas 
            if (manager.isInitializationComplete)
                manager.StopSubsystems();

            // Ajustes visuales específicos según tipo de loader
            if (manager.activeLoader is Google.XR.Cardboard.XRLoader)
            {
                Debug.Log("[GESTOR XR] Ajustes post-Cardboard.");
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
                Screen.brightness = 0.5f;
            }
            else if (manager.activeLoader is UnityEngine.XR.ARCore.ARCoreLoader)
            {
                Debug.Log("[GESTOR XR] Ajustes post-ARCore.");
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
                Screen.brightness = 0.5f;
            }


            manager.DeinitializeLoader();
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            Debug.Log("[GESTOR XR] No hay loader activo.");

        }
    }

    private void DeshabilitarCamaraExistente()
    {
        // Obtener la cámara XR directamente desde el Camera.main
        if (Camera.main != null) _xrCamera = Camera.main;

        if (_xrCamera != null)
        {
            _xrCamera.tag = "MainCamera";
            _mainCamera.enabled = false;
        }
        else
        {
            Debug.LogError("[GESTOR XR] No se encontró la cámara de Cardboard.");
        }
    }

    private void InicializarEntorno2D()
    {
        if (_mainCamera != null) _mainCamera.enabled = true;
        if (_xrCamera != null) _xrCamera.enabled = false;
    }

    private T GetLoader<T>() where T : class
    {
        return XRGeneralSettings.Instance.Manager.activeLoaders
            .FirstOrDefault(loader => loader is T) as T;
    }

    private List<UnityEngine.XR.Management.XRLoader> GetLoaders()
    {
        return XRGeneralSettings.Instance.Manager.activeLoaders.ToList();
    }

    public void SalirVR()
    {
        StartCoroutine(ApagarModoActual());
        InicializarEntorno2D();
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= AlCargarEscena;
            StartCoroutine(ApagarModoActual());
        }
    }
}
