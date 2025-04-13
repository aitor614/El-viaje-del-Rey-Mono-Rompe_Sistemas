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
    [Header("Controles")]
    public ControlAR controlAR;
    public SpawnerEspiritus spawnerEspiritus;
    public ControlMenuPrincipal controlMenuPrincipal;
    public ControlHud controlHud;

    [Header("Parámetros")]
    public int tiempoDeEspera;
    public int puntuacionVictoria;
    public int espiritusObjeto;
    public float tiempoRestante = 60f;

    // Variables
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
