using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;
//using Google.XR.Cardboard;
using UnityEngine.XR.OpenXR;

public class GestorXR : MonoBehaviour
{
    public static GestorXR Instance { get; private set; }

    // Enum para los modos XR
    public enum XRMode { None, Cardboard, ARCore, OpenXR }

    [Header("Configuración inicial XR")]
    public XRMode modoInicial = XRMode.None;

    // Cardboard
    private const float _campoVisionDefecto = 60f;

    // Cámaras
    private Camera _mainCamera;
    private Camera _xrCamera;

    void Awake()
    {
        // Si no hay instancia, inicializar
        if (Instance == null)
        {
            Instance = this;
            // No destruir el objeto al cargar nuevas escenas
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
        // Inicializar el modo XR al iniciar
        StartCoroutine(InicializarCorrutinaXR(modoInicial));
    }

    private void Update()
    {
        //// Control de inputs si Cardboard está activo
        //if (XRGeneralSettings.Instance.Manager.activeLoader is Google.XR.Cardboard.XRLoader)
        //{
        //    ControlInputCardboard();
        //}
    }

    // Al cargar una escena, definir el modo XR e inicializarlo
    private void AlCargarEscena(Scene scene, LoadSceneMode mode)
    {
        DefinirModoEscena(scene);
        StartCoroutine(InicializarCorrutinaXR(modoInicial));
    }

    // Activar el plugin XR según el modo pasado como parámetro
    public void ActivarPlugin(XRMode xRMode)
    {
        modoInicial = xRMode;
        StartCoroutine(InicializarCorrutinaXR(modoInicial));
    }

    // Definir el modo XR según la escena cargada
    private void DefinirModoEscena(Scene scene)
    {
        if (scene.name.Contains("AR")) modoInicial = XRMode.ARCore;
        else if (scene.name.Contains("VR")) modoInicial = XRMode.OpenXR;
        else if (scene.name.Contains("Cardboard")) modoInicial = XRMode.Cardboard;
        else modoInicial = XRMode.None;
    }

    // Cambiar modo XR
    public void CambiarModoXR(XRMode nuevoModo)
    {
        StartCoroutine(CambiarModoCorrutina(nuevoModo));
    }

    // Corrutina para cambiar el modo XR
    private IEnumerator CambiarModoCorrutina(XRMode nuevoModo)
    {
        yield return ApagarModoActual();
        modoInicial = nuevoModo;
        yield return InicializarCorrutinaXR(nuevoModo);
    }

    // Inicializar el modo XR según el tipo pasado como parámetro
    private IEnumerator InicializarCorrutinaXR(XRMode mode)
    {
        switch (mode)
        {
            //case XRMode.Cardboard:
            //    yield return InicializarCardboard();
            //    break;

            case XRMode.ARCore:
                yield return InicializarARCore();
                break;

            case XRMode.OpenXR:
                yield return InicializarOpenXR();
                break;

            default:
                InicializarEntorno2D();
                break;
        }
    }

    // Inicializar VR
    public IEnumerator InicializarOpenXR()
    {
        // Obtener lista completa de loaders
        var loaders = XRGeneralSettings.Instance.Manager.activeLoaders;

        // Reordenar la lista de loaders para que OpenXR sea el primero
        var loadersReordenados = new List<UnityEngine.XR.Management.XRLoader>();
        foreach (var loader in loaders)
        {
            if (loader is UnityEngine.XR.OpenXR.OpenXRLoader)
            {
                loadersReordenados.Insert(0, loader);
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

        XRGeneralSettings.Instance.Manager.TrySetLoaders(loadersReordenados);

        // Inicializar loader y subsistemas
        yield return StartCoroutine(XRGeneralSettings.Instance.Manager.InitializeLoader());
        XRGeneralSettings.Instance.Manager.StartSubsystems();

        // Configurar parámetros y cámara
        DeshabilitarCamaraExistente();
        ConfigurarPantallaOpenXR();
    }

    // Configurar pantalla para OpenXR
    private void ConfigurarPantallaOpenXR()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.brightness = 1.0f;
    }

    //// Inicializar Cardboard
    //public IEnumerator InicializarCardboard()
    //{
    //    // Obtener lista completa de loaders
    //    var loaders = XRGeneralSettings.Instance.Manager.activeLoaders;

    //    // Reordenar la lista de loaders para que Cardboard sea el primero
    //    var loadersReordenados = new List<UnityEngine.XR.Management.XRLoader>();
    //    foreach ( var loader in loaders)
    //    {
    //        if (loader is Google.XR.Cardboard.XRLoader)
    //        {
    //            loadersReordenados.Insert(0, loader);

    //            break;
    //        }
    //        else if (loader is UnityEngine.XR.ARCore.ARCoreLoader)
    //        {
    //            loadersReordenados.Insert(loadersReordenados.Count, loader);
    //        }
    //    }
    //    // Mostrar loaders cargados
    //    Debug.Log("[GESTOR XR] Cargando " + loadersReordenados.Count + " loaders: ");
    //    for (int i = 0; i < loadersReordenados.Count; i++)
    //    {
    //        Debug.Log("[GESTOR XR] Loader " + i + ": " + loadersReordenados[i].name);
    //    }

    //    yield return StartCoroutine(XRGeneralSettings.Instance.Manager.InitializeLoader());
    //    XRGeneralSettings.Instance.Manager.StartSubsystems();

    //    // Configurar parámetros y cámara
    //    if (!Api.HasDeviceParams()) Api.ScanDeviceParams();
    //    DeshabilitarCamaraExistente();
    //    ConfigurarPantallaCardboard();
    //}

    //// Configurar pantalla para Cardboard
    //private void ConfigurarPantallaCardboard()
    //{
    //    Screen.sleepTimeout = SleepTimeout.NeverSleep;
    //    Screen.brightness = 1.0f;
    //}


    //// Controlar entradas de Cardboard
    //private void ControlInputCardboard()
    //{
    //    // Comprobar si los botones de Cardboard están presionados
    //    if (Api.IsCloseButtonPressed) SalirVR();
    //    if (Api.IsGearButtonPressed) Api.ScanDeviceParams();
    //    if (Api.IsTriggerHeldPressed) Api.Recenter();
    //    // Actualizar parámetros de pantalla
    //    Api.UpdateScreenParams();
    //}

    // Inicializar ARCore
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
            else if (loader is UnityEngine.XR.OpenXR.OpenXRLoader)
            {
                loadersReordenados.Insert(loadersReordenados.Count, loader);
            }
            //else if (loader is Google.XR.Cardboard.XRLoader)
            //{
            //    loadersReordenados.Insert(loadersReordenados.Count, loader);
            //}
        }
        Debug.Log("[GESTOR XR] Cargando " + loadersReordenados.Count + " loaders: ");
        for (int i = 0; i < loadersReordenados.Count; i++)
        {
            Debug.Log("[GESTOR XR] Loader " + i + ": " + loadersReordenados[i].name);
        }

        XRGeneralSettings.Instance.Manager.TrySetLoaders(loadersReordenados);

        // Inicializar loader y subsistemas
        yield return StartCoroutine(XRGeneralSettings.Instance.Manager.InitializeLoader());
        XRGeneralSettings.Instance.Manager.StartSubsystems();
    }

    // Apagar modo actual
    public IEnumerator ApagarModoActual()
    {
        // Obtener el XRManagerSettings
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

            // Devolver ajustes de pantalla a la configuración por defecto
            Debug.Log("[GESTOR XR] Ajustes post-Loader genérico.");
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            Screen.brightness = 0.5f;

            // Desactivar subsistemas y loader
            manager.DeinitializeLoader();
            // Esperar para asegurar que el loader se apague correctamente
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            Debug.Log("[GESTOR XR] No hay loader activo.");

        }
    }

    // Deshabilitar cámara existente
    private void DeshabilitarCamaraExistente()
    {
        // Obtener la cámara XR directamente desde el Camera.main
        if (Camera.main != null) _xrCamera = Camera.main;

        // Si se ha encontrado la cámara XR
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

    // Inicializar entorno 2D 
    private void InicializarEntorno2D()
    {
        // Activar cámara principal y desactivar cámara XR
        if (_mainCamera != null) _mainCamera.enabled = true;
        if (_xrCamera != null) _xrCamera.enabled = false;
    }

    // Obtener el loaders del XRManagerSettings
    private T GetLoader<T>() where T : class
    {
        return XRGeneralSettings.Instance.Manager.activeLoaders
            .FirstOrDefault(loader => loader is T) as T;
    }

    private List<UnityEngine.XR.Management.XRLoader> GetLoaders()
    {
        return XRGeneralSettings.Instance.Manager.activeLoaders.ToList();
    }

    // Salir de VR
    public void SalirVR()
    {
        // Desactivar subsistemas de XR
        StartCoroutine(ApagarModoActual());
        // Desactivar cámara XR y activar cámara principal
        InicializarEntorno2D();
    }

    // Al destruir el objeto
    void OnDestroy()
    {
        if (Instance == this)
        {
            // Desuscribirse del evento de carga de escena
            SceneManager.sceneLoaded -= AlCargarEscena;
            // Apagar modo actual
            StartCoroutine(ApagarModoActual());
        }
    }
}
