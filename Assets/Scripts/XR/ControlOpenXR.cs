using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ControlOpenXR : MonoBehaviour
{
    [Header("Elementos de XR")]
    public GestorXR gestorXR;
    public XROrigin xrOrigin;
    public GameObject spawner;
    public GameObject interacciones;

    // Variables
    private int sleepInicial;
    private float brilloInicial;

    public void ActivarOpenXR()
    {
        StartCoroutine(ActivarOpenXRCoroutine());
    }

    private IEnumerator ActivarOpenXRCoroutine()
    {
        // Obtener el gestor de XR
        if (ControlMenuPrincipal.InstanciaControl == null || ControlMenuPrincipal.InstanciaGestorXR == null)
        {
            if (ControlMenuPrincipal.InstanciaControl == null) Debug.LogError("[Control_OpenXR] No se encuentra instancia ControlMenuPrincioal.");
            Debug.LogError("[Control_OpenXR] GestorXR no encontrado. Buscando en la escena...");
            gestorXR = FindFirstObjectByType<GestorXR>();
            if (gestorXR == null)
            {
                Debug.LogError("[Control_OpenXR] GestorXR sigue sin encontrarse.");
                yield break;
            }
        }
        else
        {
            gestorXR = ControlMenuPrincipal.InstanciaGestorXR;
        }

        sleepInicial = Screen.sleepTimeout;
        brilloInicial = Screen.brightness;

        // Desactiva todos los componentes de OpenXR
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(false);
        else Debug.LogError("[Control_OpenXR] xrOrigin is null");
        if (spawner != null) spawner.SetActive(false);
        else Debug.LogError("[Control_OpenXR] spawner is null");
        if (interacciones != null) interacciones.SetActive(false);
        else Debug.LogError("[Control_OpenXR] interacciones is null");

        yield return gestorXR.InicializarOpenXR();


        // Activar todos los componentes de OpenXR
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(true);
        else Debug.LogError("[Control_OpenXR] xrOrigin is null");
        if (spawner != null) spawner.SetActive(true);
        else Debug.LogError("[Control_OpenXR] spawner is null");
        if (interacciones != null) interacciones.SetActive(true);
        else Debug.LogError("[Control_OpenXR] interacciones is null");

        Screen.sleepTimeout = sleepInicial;
        Screen.brightness = brilloInicial;
    }

    public void DesactivarOpenXR()
    {
        StartCoroutine(gestorXR.ApagarModoActual());

        // Desactiva todos los componentes de OpenXR
        if (xrOrigin != null) xrOrigin.gameObject.SetActive(false);
        if (spawner != null) spawner.SetActive(false);
        if (interacciones != null) interacciones.SetActive(false);

    }

}
