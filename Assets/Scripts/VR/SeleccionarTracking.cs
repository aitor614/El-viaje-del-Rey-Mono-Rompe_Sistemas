using UnityEngine;

public class SeleccionarTracking : MonoBehaviour
{
    public Behaviour poseEditor;
    public Behaviour poseCardboard;
    public Behaviour poseOpenXR;

    private GestorXR gestorXR;

    void Awake()
    {
        // Obtener el gestor de XR
        if (GestorXR.InstanciaGestorXR == null)
        {
            Debug.LogError("[Tracking (Camara)] GestorXR no encontrado. Buscando en la escena...");
            gestorXR = FindFirstObjectByType<GestorXR>();
            if (gestorXR == null)
            {
                Debug.LogError("[Tracking (Camara)] GestorXR sigue sin encontrarse.");
                return;
            }
        }
        else
        {
            gestorXR = GestorXR.InstanciaGestorXR;
        }

        // Desactivar todos los componentes de XR
        if (poseEditor != null) poseEditor.enabled = false;
        if (poseCardboard != null) poseCardboard.enabled = false;
        if (poseOpenXR != null) poseOpenXR.enabled = false;

        if (gestorXR.modoInicial == GestorXR.XRMode.None)
        {
            if (poseEditor != null)
            {
                Debug.Log("[Tracking (Camara)] Modo Editor");
                poseEditor.enabled = true;
            }
            else Debug.LogError("[Tracking (Camara)] poseEditor is null");
        }
        else if (gestorXR.modoInicial == GestorXR.XRMode.Cardboard)
        {
            if (poseCardboard != null)
            {
                Debug.Log("[Tracking (Camara)] Modo CardBoard");
                poseCardboard.enabled = false;
            }
            else Debug.LogError("[Tracking (Camara)] poseCardboard is null");
        }
        else if (gestorXR.modoInicial == GestorXR.XRMode.OpenXR)
        {
            if (poseOpenXR != null)
            {
                Debug.Log("[Tracking (Camara)] Modo OpenXR");
                poseOpenXR.enabled = true;
            }
            else Debug.LogError("[Tracking (Camara)] poseOpenXR is null");
        }
        else
        {
            Debug.LogError("[Tracking (Camara)] Modo no soportado.");
        }
    }
}
