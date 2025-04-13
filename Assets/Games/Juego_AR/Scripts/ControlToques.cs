using UnityEngine;
using UnityEngine.InputSystem;

public class ControlToques : MonoBehaviour
{
    // Variables
    private Camera camara;

    private void Start()
    {
        // Obtener la cámara principal
        camara = Camera.main;
    }

    private void Update()
    {
        // Click izquierdo del ratón en editor
#if UNITY_EDITOR
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ProcesarToque(Mouse.current.position.ReadValue());
        }
#endif
        // Tocar la pantalla en móvil
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            ProcesarToque(touchPos);
        }
    }

    // Procesar el toque en la pantalla
    private void ProcesarToque(Vector2 posicionPantalla)
    {
        Ray ray = camara.ScreenPointToRay(posicionPantalla);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var espiritu = hit.collider.GetComponent<EspirituBase>();
            if (espiritu != null)
            {
                Debug.Log("Espíritu tocado: " + espiritu.name);
                espiritu.RecibirToque();
            }
        }
    }
}
