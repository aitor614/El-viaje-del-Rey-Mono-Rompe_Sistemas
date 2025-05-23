using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlPremio : MonoBehaviour
{
    public Canvas canvas;
    public Image premio;
    public TextMeshProUGUI TxtPremio;
    public Sprite premioEscape;
    public Sprite premioGolpe;
    public Sprite premioHuida;
    public Sprite premioEspiritus;
    public Sprite premioBatalla;
    public Sprite premioCompleto;

    void Start()
    {
        // Obtener la cámara principal
        Camera camara = Camera.main;
        if (camara != null) Debug.Log("[Premio] Cámara principal encontrada: " + camara.name);
        else Debug.Log("[Premio] Cámara principal no encontrada, buscando en la escena.");

        // Obtener cámara de la escena principal
        if (camara == null)
        {
            Debug.Log("[Premio] Buscar cámara en la escena principal");
            camara = FindAnyObjectByType<Camera>();
        }

        if (camara == null)
        {
            Debug.Log("[Premio] Crear cámara principal");
            // Asignar nueva cámara a la escena
            GameObject camaraGO = new GameObject("CamaraTemporal");
            camara = camaraGO.AddComponent<Camera>();
            camara.tag = "MainCamera";
        }

        canvas.worldCamera = camara;
        camara.enabled = true;

        // Asignar el sprite correspondiente al canvas según la escena cargada
        if (SceneManager.GetSceneByName("Juego2DEscapeInfierno").isLoaded)
        {
            // Asignar sprite a gameObject
            premio.sprite = premioEscape;
            TxtPremio.text = "PLUMA DEL AVE FÉNIX";

        }
        else if (SceneManager.GetSceneByName("Juego2DGolpeBaston").isLoaded)
        {
            premio.sprite = premioGolpe;
            TxtPremio.text = "INSCRIPCIÓN DEL SELLO DE BUDA";
        }
        else if (SceneManager.GetSceneByName("Juego2DHuidaCelestial").isLoaded)
        {
            premio.sprite = premioHuida;
            TxtPremio.text = "MELOCOTÓN DORADO DE LA INMORTALIDAD";
        }
        else if (SceneManager.GetSceneByName("JuegoAREspiritusDesencarnados").isLoaded)
        {
            premio.sprite = premioEspiritus;
            TxtPremio.text = "PAPIRO DEL MÁS ALLÁ";
        }
        else if (SceneManager.GetSceneByName("JuegoVRBatallaCelestial").isLoaded)
        {
            premio.sprite = premioBatalla;
            TxtPremio.text = "CINTA DORADA";
        }
        else if (SceneManager.GetSceneByName("EscenaFinal").isLoaded)
        {
            premio.sprite = premioCompleto;
            TxtPremio.text = "ARMADURA CELESTIAL";
        }
    }

}
