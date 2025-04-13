using TMPro;
using UnityEngine;

public class ControlMenuPausa : MonoBehaviour
{
    [Header("Controles")]
    public ControlPausa controlPausa;
    [Header("Componentes")]
    public TextMeshProUGUI textoPausa;
    [Header("Colores")]
    public int r = 255, g = 0, b = 0;
    public bool rojo = true, verde = false, azul = false;

    // Variables
    private float hue = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        controlPausa = ControlPausa.InstanciaControl;
        // Inicializa el texto de pausa
        textoPausa = FindFirstObjectByType<TextMeshProUGUI>();
    }

    // Función para reanudar el juego
    public void Reanudar()
    {
        controlPausa.Reanudar();
    }

    // Función para reiniciar el juego
    public void Reiniciar()
    {
        controlPausa.Reiniciar();
    }

    // Función para salir al menú principal
    public void MenuPrincipal()
    {
        controlPausa.MenuPrincipal();
    }

    // Update is called once per frame
    void Update()
    {
        CambiarColorTexto();
    }

    // Función para cambiar el color del texto de pausa
    public void CambiarColorTexto()
    {
        hue += Time.deltaTime * 0.1f; // velocidad de cambio
        if (hue > 1f) hue = 0f;

        Color nuevoColor = Color.HSVToRGB(hue, 1f, 1f);
        textoPausa.color = nuevoColor;
    }

}
