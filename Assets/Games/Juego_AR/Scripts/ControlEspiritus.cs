using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Management;
using Unity.XR.CoreUtils;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ControlEspiritus : MonoBehaviour
{
    [SerializeField] private ControlAR controlAR;
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        // Instanciar el objeto ControlAR
        if (controlAR == null)
        {
            controlAR = FindFirstObjectByType<ControlAR>();
            if (controlAR == null)
            {
                Debug.LogError("No se encontró el objeto ControlAR en la escena.");
                return;
            }
        }
        controlAR.ActivarAR();

    }

    private void OnDestroy()
    {
        controlAR.DesactivarAR();
    }

    private void OnDisable()
    {
        controlAR.DesactivarAR();
    }

    private void OnEnable()
    {
        controlAR.ActivarAR();
    }
}
