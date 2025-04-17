using Unity.VisualScripting;
using UnityEngine;

public class ControlEspiritus : MonoBehaviour
{
    [Header("Controles")]
    public ControlAR _controlAR;
    public ControlAR_old controlAR;
    public SpawnerEspiritus spawnerEspiritus;
    public ControlMenuPrincipal controlMenuPrincipal;
    public ControlHud controlHud;

    [Header("Parámetros")]
    public int tiempoDeEspera;
    public int puntuacionVictoria;
    public int espiritusObjeto;
    public float tiempoRestante;

    // Variables
    private int espiritus;
    private int puntuacion;
    private bool arActivado = false;

    void Start()
    {
        controlMenuPrincipal = ControlMenuPrincipal.InstanciaControl;
        controlHud = ControlHud.InstanciaControl;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        //controlAR.ActivarAR();
        arActivado = true;
        PlayerPrefs.SetInt("Espiritus", 0);
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        PlayerPrefs.SetInt("EspiritusObjeto", 0);
        PlayerPrefs.Save();
    }

    private void Update()
    {
        espiritus = PlayerPrefs.GetInt("Espiritus");
        puntuacion = PlayerPrefs.GetInt("PuntuacionPartida");
        ActualizarPuntos();
        ActualizarContador(); 
        RestarTiempo();

    }

    private void ActualizarContador()
    {
        controlHud.ActualizarContador("ESPíRITUS", espiritus);
    }

    private void ActualizarPuntos()
    {
        controlHud.ActualizarPuntos("SCORE", puntuacion);
    }

    private void GuardarPuntos()
    {
        PlayerPrefs.SetInt("Puntuacion", PlayerPrefs.GetInt("Puntuacion") + puntuacion);
        PlayerPrefs.Save();
    }

    // Función para controlar el tiempo
    void RestarTiempo()
    {
        // Si el tiempo es mayor a 0, se resta el tiempo
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante < 0) tiempoRestante = 0;
            controlHud.ActualizarTiempo(tiempoRestante);
        }

        if (tiempoRestante == 0)
        {
            
            if (puntuacion > puntuacionVictoria)
            {
                if (espiritus >= espiritusObjeto) {
                    PlayerPrefs.SetInt("ObjetoEspiritus", 1);
                    PlayerPrefs.Save();
                }
                GuardarPuntos();
                controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Exito);
            }
            else
            {
                GuardarPuntos();
                controlMenuPrincipal.ProcesarResultado(ControlMenuPrincipal.ResultadoMinijuego.Derrota);
            }
        }
    }

    private void OnDestroy()
    {
        _controlAR.DesactivarAR();
        //controlAR.DesactivarAR();
        arActivado = false;
    }

    private void OnDisable()
    {
        _controlAR.DesactivarAR();
        //controlAR.DesactivarAR();
        arActivado = false;
    }

    private void OnEnable()
    {
        if (!arActivado)
        _controlAR.ActivarAR();

        //controlAR.ActivarAR();
        arActivado = true;
    }
}
