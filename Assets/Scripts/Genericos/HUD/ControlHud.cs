using TMPro;
using UnityEngine;

public class ControlHud : MonoBehaviour
{
    public static ControlHud InstanciaControl { get; private set; }
    public TextMeshProUGUI TxtScore;
    public TextMeshProUGUI TxtContador;
    public TextMeshProUGUI TxtTime;
    public Emblema[] Emblemas;

    private void Awake()
    {
        InstanciaControl = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActualizarContador(string nombreContador, int contador)
    {
        TxtContador.text = nombreContador + ": " + contador;
    }

    public void ActualizarPuntos(string nombrePuntos, int puntos)
    {
        TxtScore.text = nombrePuntos + ": " + puntos;
    }

    public void ActualizarTiempo(float tiempo)
    {
        TxtTime.text = tiempo.ToString("f0");
    }

    public void ActualizarEmblemas(int cantidadEmblemas)
    {
        for (int i = 0; i < Emblemas.Length; i++)
        {
            if (i < cantidadEmblemas)
            {
                Emblemas[i].gameObject.SetActive(true);
            }
            else
            {
                Emblemas[i].gameObject.SetActive(false);
            }
        }
    }
}
