using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InteraccionVista : MonoBehaviour
{
    public LayerMask capaInteractiva;
    public float distanciaMaxima = 20f;
    public InputAction accionGatillo;

    void Update()
    {
#if UNITY_EDITOR
        // Para pruebas en editor
        if (Mouse.current.leftButton.wasPressedThisFrame)
            LanzarRaycast();
#else
        // En móvil con OpenXR
        if (accionGatillo.WasPressedThisFrame())
        {
            LanzarRaycast();
        }
#endif
    }

    void OnEnable() => accionGatillo.Enable();
    void OnDisable() => accionGatillo.Disable();

    private void LanzarRaycast()
    {
        Ray rayo = new(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(rayo, out RaycastHit hit, distanciaMaxima, capaInteractiva))
        {
            GameObject objetivo = hit.collider.gameObject;

            // Intenta ejecutar un clic en UI o GameObject
            ExecuteEvents.Execute(objetivo, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            Debug.Log($"Objeto pulsado: {objetivo.name}");
        }
        // En móvil con OpenXR
        if (accionGatillo.WasPressedThisFrame())
        {
            LanzarRaycast();
        }
    }
}
