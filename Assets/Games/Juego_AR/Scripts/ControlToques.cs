using UnityEngine;
using UnityEngine.InputSystem;

public class ControlToques : MonoBehaviour
{
    private Camera camara;

    private void Start()
    {
        camara = Camera.main;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ProcesarToque(Mouse.current.position.ReadValue());
        }
#endif

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            ProcesarToque(touchPos);
        }
    }

    private void ProcesarToque(Vector2 pantalla)
    {
        Ray ray = camara.ScreenPointToRay(pantalla);
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
