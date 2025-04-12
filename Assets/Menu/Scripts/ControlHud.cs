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
}
