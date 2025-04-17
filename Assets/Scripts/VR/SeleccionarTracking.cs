using UnityEngine;

public class SeleccionarTracking : MonoBehaviour
{
    public Behaviour poseEditor;
    public Behaviour poseCardboard;

    void Awake()
    {
#if UNITY_EDITOR
        Debug.Log("Modo Editor");
        if (poseEditor != null) poseEditor.enabled = true;
        if (poseCardboard != null) poseCardboard.enabled = false;
#else
        Debug.Log("Modo Cardboard");
        if (poseEditor != null) poseEditor.enabled = false;
        if (poseCardboard != null) poseCardboard.enabled = true;
#endif
    }
}
