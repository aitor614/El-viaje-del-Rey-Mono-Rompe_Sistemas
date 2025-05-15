using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;
using System.Collections.Generic;
//using Google.XR.Cardboard;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.OpenXR;

public class ControlXR : MonoBehaviour
{
    public enum TipoXR { Ninguno, Cardboard, ARCore, OpenXR}

    private XRManagerSettings _gestorXR;
    //private Google.XR.Cardboard.XRLoader _loaderCardboard;
    private UnityEngine.XR.ARCore.ARCoreLoader _loaderAR;
    private UnityEngine.XR.OpenXR.OpenXRLoader _loaderOpenXR;

    private void Awake()
    {
        _gestorXR = XRGeneralSettings.Instance.Manager;
        if (_gestorXR == null)
        {
            Debug.LogError("XRManagerSettings no disponible.");
            return;
        }
        DetectarLoaders();

        Debug.Log("Loader activo al iniciar: " + _gestorXR.activeLoader?.name);
    }

    // Detectar loaders disponibles
    private void DetectarLoaders()
    {
        // Recorrer los loaders activos
        foreach (var loader in _gestorXR.activeLoaders)
        {
            // Comprobar si el loader es de ARCore o Cardboard
            if (loader is UnityEngine.XR.ARCore.ARCoreLoader)
                _loaderAR = loader as UnityEngine.XR.ARCore.ARCoreLoader;
            //else if (loader is Google.XR.Cardboard.XRLoader)
            //    _loaderCardboard = loader as Google.XR.Cardboard.XRLoader;
            else if (loader is UnityEngine.XR.OpenXR.OpenXRLoader)
                _loaderOpenXR = loader as UnityEngine.XR.OpenXR.OpenXRLoader;

        }

        //if (_loaderAR == null || _loaderCardboard == null)
        //{
        //    Debug.LogError("Faltan loaders AR o Cardboard.");
        //}
        if (_loaderAR == null && _loaderOpenXR == null)
        {
            Debug.LogError("Faltan loaders AR u OpenXR.");
        }
    }

    // Activar XR
    public void ActivarXR(TipoXR tipoXR)
    {
        StartCoroutine(CambiarLoaderXR(tipoXR));
    }

    // Desactivar XR
    public void DesactivarXR()
    {
        StartCoroutine(DesactivarActual());
    }

    private IEnumerator CambiarLoaderXR(ControlXR.TipoXR tipo)
    {
        if (_gestorXR.activeLoader != null)
        {
            string actual = _gestorXR.activeLoader.name;
            if ((tipo == TipoXR.Cardboard && actual.Contains("Cardboard")) ||
                (tipo == TipoXR.ARCore && actual.Contains("ARCore")))
            {
                Debug.Log("El loader ya está activo: " + actual);
                yield break;
            }
        }

        yield return DesactivarActual();

        var loaders = new List<UnityEngine.XR.Management.XRLoader>();

        if (tipo == ControlXR.TipoXR.ARCore)
        {
            if (_loaderAR != null) loaders.Add(_loaderAR);
            //if (_loaderCardboard != null) loaders.Add(_loaderCardboard);
            if (_loaderOpenXR != null) loaders.Add(_loaderOpenXR);
        }
        else if (tipo == ControlXR.TipoXR.OpenXR)
        { 
            if (_loaderOpenXR != null) loaders.Add(_loaderOpenXR);
            //if (_loaderCardboard != null) loaders.Add(_loaderCardboard);
            if (_loaderAR != null) loaders.Add(_loaderAR);
        }
        //else if (tipo == ControlXR.TipoXR.Cardboard)
        //{
        //    if (_loaderCardboard != null) loaders.Add(_loaderCardboard);
        //    if (_loaderAR != null) loaders.Add(_loaderAR);
        //}

        // Intentar establecer los loaders en el nuevo orden
        if (!_gestorXR.TrySetLoaders(new List<UnityEngine.XR.Management.XRLoader>(loaders)))
        {
            Debug.LogError("No se pudo reordenar los loaders.");
            yield break;
        }

        // Inicializar el nuevo loader 
        yield return _gestorXR.InitializeLoader();

        // Comprobar si el loader se ha inicializado correctamente
        if (_gestorXR.activeLoader == null)
        {
            Debug.LogError("Fallo al inicializar el loader.");
            yield break;
        }

        // Iniciar los subsistemas del nuevo loader
        _gestorXR.StartSubsystems();
        Debug.Log("Loader activado: " + _gestorXR.activeLoader.name);
    }

    // Desactivar el loader actual
    private IEnumerator DesactivarActual()
    {
        if (_gestorXR.activeLoader != null)
        {
            Debug.Log("Desactivando loader actual: " + _gestorXR.activeLoader.name);
            _gestorXR.StopSubsystems();
            _gestorXR.DeinitializeLoader();
        }
        yield return null;
    }
}
