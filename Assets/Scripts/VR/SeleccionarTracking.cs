using UnityEngine;

public class SeleccionarTracking : MonoBehaviour
{
    public Behaviour poseEditor;
    public Behaviour poseVR;

    void Awake()
    {
#if UNITY_EDITOR
        Debug.Log("[Tracking (Camara)] Modo Editor");
        if (poseEditor != null) poseEditor.enabled = true;
        if (poseVR != null) poseVR.enabled = false;
#else
        Debug.Log("[Tracking (Camara)] Modo VR");
        if (poseEditor != null) poseEditor.enabled = false;
        if (poseVR != null) poseVR.enabled = true;
#endif
    }
}
