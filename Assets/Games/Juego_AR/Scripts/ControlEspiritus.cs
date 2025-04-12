using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Management;
using Unity.XR.CoreUtils;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using TMPro;
using System;

public class ControlEspiritus : MonoBehaviour
{
    [SerializeField] private ControlAR controlAR;
    [SerializeField] private int tiempoDeEspera;
    [SerializeField] private SpawnerEspiritus spawnerEspiritus;
    [SerializeField] private ControlMenuPrincipal controlMenuPrincipal;
    [SerializeField] private ControlHud controlHud;
    [SerializeField] private int puntuacionVictoria;

    public float tiempoRestante = 60f;
    private int espiritus;
    private int puntuacion;

    void Start()
    {
        controlMenuPrincipal = ControlMenuPrincipal.InstanciaControl;
        controlHud = ControlHud.InstanciaControl;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        controlAR.ActivarAR();

        PlayerPrefs.SetInt("Espiritus", 0);
        PlayerPrefs.SetInt("PuntuacionPartida", 0);
        PlayerPrefs.Save();


    }

    private void Update()
    {
        espiritus = PlayerPrefs.GetInt("Espiritus");
        puntuacion = PlayerPrefs.GetInt("PuntuacionPartida");
        ActualizarPuntos();
        ActualizarEspiritus(); 
        RestarTiempo();

    }

    private void ActualizarEspiritus()
    {
        controlHud.TxtContador.text = "ESPíRITUS: " + espiritus;
    }

    private void ActualizarPuntos()
    {
        controlHud.TxtScore.text = "SCORE: " + puntuacion;
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
            controlHud.TxtTime.text = tiempoRestante.ToString("f0");
        }

        if (tiempoRestante == 0)
        {
            
            if (puntuacion > puntuacionVictoria)
            {
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
