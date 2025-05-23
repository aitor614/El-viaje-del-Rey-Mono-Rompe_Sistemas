using UnityEditor;
using UnityEngine;

public class AsignadorMainCamera : MonoBehaviour
{
    [Header("Camaras principales")]
    public Camera camaraCardboard;
    public Camera camaraOpenXR;
    public Camera camaraAR;

    [Header("Canvas")]
    public Canvas[] canvasCamara;

    private ControlBatalla controlBatalla;
    private Camera camaraSeleccionada;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controlBatalla = ControlBatalla.Instancia;
        if (controlBatalla == null)
        {
            Debug.LogError("No se ha encontrado el controlador de la escena o no tiene instancia de GestorXR.");
        }
        else if (controlBatalla.modoSeleccionado == ControlBatalla.ModoXR.OpenXR) camaraSeleccionada = camaraOpenXR;
        else if (controlBatalla.modoSeleccionado == ControlBatalla.ModoXR.ARCore) camaraSeleccionada = camaraAR;
        else if (controlBatalla.modoSeleccionado == ControlBatalla.ModoXR.Cardboard) camaraSeleccionada = camaraCardboard;
        else Debug.LogError("No se ha encontrado el modo de XR seleccionado.");

        if (camaraSeleccionada != null)
        {
            for (int i = 0; i < canvasCamara.Length; i++)
            {
                if (canvasCamara[i] != null)
                {
                    canvasCamara[i].worldCamera = camaraSeleccionada;
                    Debug.Log("Canvas " + i + " asignado a la cámara seleccionada.");
                }
                else
                {
                    Debug.LogError("No se ha encontrado el canvas " + i + ".");
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
