using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Management;
using Google.XR.Cardboard;
using Unity.XR.CoreUtils;
using UnityEngine.XR;
using UnityEngine.EventSystems;

public class GestorXR : MonoBehaviour
{
    public static GestorXR Instance { get; private set; }

    public enum XRMode { None, Cardboard, ARCore }
    public XRMode initialMode = XRMode.None;

    // Cardboard
    private const float _defaultFieldOfView = 60f;

    // ARCore
    private ARSession _arSession;
    private XROrigin _arOrigin;
    private ARPlaneManager _arPlaneManager;
    private ARRaycastManager _arRaycastManager;
    private ARCameraBackground _arCameraBackground;
    private GameObject _spawner;
    private GameObject _interacciones;

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

            StartCoroutine(ShutdownCurrentMode());
            Setup2DEnvironment();

            //SceneManager.sceneLoaded += OnSceneLoaded;
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
        StartCoroutine(InitializeXRCoroutine(initialMode));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DetermineInitialMode(scene);
        StartCoroutine(InitializeXRCoroutine(initialMode));
    }

    public void ActivarPlugin(XRMode xRMode)
    {
        initialMode = xRMode;
        StartCoroutine(InitializeXRCoroutine(initialMode));
    }

    private void DetermineInitialMode(Scene scene)
    {
        if (scene.name.Contains("AR")) initialMode = XRMode.ARCore;
        else if (scene.name.Contains("VR")) initialMode = XRMode.Cardboard;
        else initialMode = XRMode.None;
    }

    public void SwitchMode(XRMode newMode)
    {
        StartCoroutine(SwitchModeCoroutine(newMode));
    }

    private IEnumerator SwitchModeCoroutine(XRMode newMode)
    {
        yield return ShutdownCurrentMode();
        initialMode = newMode;
        yield return InitializeXRCoroutine(newMode);
    }

    private IEnumerator InitializeXRCoroutine(XRMode mode)
    {
        switch (mode)
        {
            case XRMode.Cardboard:
                yield return InitializeCardboard();
                break;

            case XRMode.ARCore:
                yield return InitializeARCore();
                break;

            default:
                Setup2DEnvironment();
                break;
        }
    }

    #region Cardboard Implementation
    public IEnumerator InitializeCardboard()
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

        Debug.Log("[GESTOR XR] Cargando " + loadersReordenados.Count + " loaders: ");
        for (int i = 0; i < loadersReordenados.Count; i++)
        {
            Debug.Log("[GESTOR XR] Loader " + i + ": " + loadersReordenados[i].name);
        }

        // Inicializar Cardboard
        yield return ReinitializeXRManager(loadersReordenados);

        // Configurar parámetros y cámara
        if (!Api.HasDeviceParams()) Api.ScanDeviceParams();
        DeshabilitarCamaraExistente();
        SetupCardboardInput();
    }



    private void SetupCardboardInput()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.brightness = 1.0f;
    }

    private void Update()
    {
        if (XRGeneralSettings.Instance.Manager.activeLoader is Google.XR.Cardboard.XRLoader)
        {
            HandleCardboardInput();
        }
    }

    private void HandleCardboardInput()
    {
        if (Api.IsCloseButtonPressed) ExitVR();
        if (Api.IsGearButtonPressed) Api.ScanDeviceParams();
        if (Api.IsTriggerHeldPressed) Api.Recenter();
        Api.UpdateScreenParams();
    }
    #endregion

    #region ARCore Implementation
    public IEnumerator InitializeARCore()
    {
        // Desactivar subsistemas de ARCore
        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            Debug.Log("[GESTOR XR] Desactivando ActiveLoader.");
            yield return ShutdownCurrentMode();
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

        yield return StartCoroutine(XRGeneralSettings.Instance.Manager.InitializeLoader());
        XRGeneralSettings.Instance.Manager.StartSubsystems();

        // Inicializar ARCore
        //yield return ReinitializeXRManager(loadersReordenados);
        
        SetupARComponents();
    }

    private void SetupARComponents()
    {

        //// Desactiva todo antes de iniciar XR
        //if (_arSession != null) _arSession.gameObject.SetActive(false);
        //if (_arOrigin != null) _arOrigin.gameObject.SetActive(false);
        //if (_arPlaneManager != null) _arPlaneManager.enabled = false;
        //if (_arRaycastManager != null) _arRaycastManager.enabled = false;
        //if (_arCameraBackground != null) _arCameraBackground.enabled = false;
        //if (_spawner != null) _spawner.SetActive(false);
        //if (_interacciones != null) _interacciones.SetActive(false);

        //// Obtener componentes de la escena
        //_arOrigin = FindFirstObjectByType<XROrigin>();
        //_arSession = FindFirstObjectByType<ARSession>();
        //_arPlaneManager = FindFirstObjectByType<ARPlaneManager>();
        //_arRaycastManager = FindFirstObjectByType<ARRaycastManager>();
        //_arCameraBackground = FindFirstObjectByType<ARCameraBackground>();
        //_spawner = GameObject.Find("Spawner");
        //_interacciones = GameObject.Find("Interacciones");

        //// Activa todo tras iniciar XR correctamente
        //if (_arSession != null) _arSession.gameObject.SetActive(true);
        //if (_arOrigin != null) _arOrigin.gameObject.SetActive(true);
        //if (_arPlaneManager != null) _arPlaneManager.enabled = true;
        //if (_arRaycastManager != null) _arRaycastManager.enabled = true;
        //if (_arCameraBackground != null) _arCameraBackground.enabled = true;
        //if (_spawner != null) _spawner.SetActive(true);
        //if (_interacciones != null) _interacciones.SetActive(true);
    }
    private IEnumerator ReinitializeXRManager(List<UnityEngine.XR.Management.XRLoader> targetLoaders)
    {
        // Verificar existencia de XRGeneralSettings
        var manager = XRGeneralSettings.Instance.Manager;
        if (manager == null)
        {
            Debug.LogError("[GESTOR XR] XRGeneralSettings no encontrado.");
            yield break;
        }

        // Desactivar subsistemas actuales
        yield return ShutdownCurrentMode();
        while (manager.activeLoader != null)
        {
            Debug.Log("[GESTOR XR] Esperando a que se libere el loader...");
            yield return null;
        }
        Debug.Log("[GESTOR XR] Loader activo tras shutdown: " + XRGeneralSettings.Instance.Manager.activeLoader);

        // Definir loaders
        targetLoaders = targetLoaders.Where(loader => loader != null).ToList();
        if (targetLoaders.Count == 0)
        {
            Debug.LogError("[GESTOR XR] No hay loaders válidos");
            yield break;
        }

        // Establecer loaders
        if (!manager.TrySetLoaders(targetLoaders))
        {
            Debug.LogError("[GESTOR XR] Error al asignar loaders");
            yield break;
        }

        var loadersNuevos = GetLoaders();
        Debug.Log("[GESTOR XR] Cargando " + loadersNuevos.Count + " loaders: ");
        for (int i = 0; i < loadersNuevos.Count; i++)
        {
            Debug.Log("[GESTOR XR] Loader " + i + ": " + loadersNuevos[i].name + " | " + loadersNuevos[i].GetType().Name + " | " + loadersNuevos[i].ToString());
        }

        var loader = loadersNuevos.First();
        string loaderName = loader.GetType().Name;
        Debug.Log("[GESTOR XR] Primer loader: " + loaderName);

        // Inicializar Cardboard
        if (loaderName == "XRLoader")
        {
            Debug.Log("[GESTOR XR] Inicializando CardboardXRLoader");
            var cardboardLoader = loader as Google.XR.Cardboard.XRLoader;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Screen.brightness = 1.0f;

            // Inicializar Cardboard
            yield return cardboardLoader.Initialize();
            // Esperar un frame para asegurar que la inicialización se complete
            yield return null;

            // Iniciar Cardboard
            if (cardboardLoader != null)
            {
                DeshabilitarCamaraExistente();
                cardboardLoader.Start();
            }

        }
        // Inicializar ARCore
        else if (loaderName == "ARCoreLoader")
        {
            Debug.Log("[GESTOR XR] Inicializando ARCoreLoader");
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Screen.brightness = 1.0f;
            // Inicializar ARCore
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
            while (!XRGeneralSettings.Instance.Manager.isInitializationComplete)
                yield return null;
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            // Esperar un frame para asegurar que la inicialización se complete
            yield return null;

            SetupARComponents();

        }
        else
        {
            Debug.LogError("[GESTOR XR] Loader no soportado: " + loaderName);
            yield break;
        }

    }

    public IEnumerator ShutdownCurrentMode()
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


    #endregion

    #region Camera Management
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

    private void Setup2DEnvironment()
    {
        if (_mainCamera != null) _mainCamera.enabled = true;
        if (_xrCamera != null) _xrCamera.enabled = false;
    }
    #endregion

    #region Utilities
    private T GetLoader<T>() where T : class
    {
        return XRGeneralSettings.Instance.Manager.activeLoaders
            .FirstOrDefault(loader => loader is T) as T;
    }

    private List<UnityEngine.XR.Management.XRLoader> GetLoaders()
    {
        return XRGeneralSettings.Instance.Manager.activeLoaders.ToList();
    }

    private void SetupXRInput()
    {
        //InputSystem.EnableDevice(UnityEngine.InputSystem.Pointer.current);
        //InputSystem.EnableDevice(Keyboard.current);
    }

    public void ExitVR()
    {
        StartCoroutine(ShutdownCurrentMode());
        Setup2DEnvironment();
    }
    #endregion



    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            StartCoroutine(ShutdownCurrentMode());
        }
    }
}
