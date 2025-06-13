using System.Collections;
using UnityEngine;

public class EspirituDemoniaco : EspirituBase
{
    [Header("Robo de Puntos")]
    public int puntosRobo;
    public float tiempoRobo;

    // Variables internas
    private int puntuacionRobada;
    private bool robado = false;
    protected override void Start()
    {
        base.Start();
        puntuacionRobada = 0;
    }

    protected override void Update()
    {
        base.Update();

        // Rotar el espíritu demoniaco continuamente
        transform.Rotate(Vector3.up, 90 * Time.deltaTime);

        if (PlayerPrefs.GetInt("PuntuacionPartida") >= puntosRobo) StartCoroutine(RobarPuntos());
    }

    // Robo de puntos del jugador
    private IEnumerator RobarPuntos()
    {
        if (robado) yield break;
        Debug.Log($"[Espíritu Demoniaco] Robando {puntosRobo} puntos al jugador.");
        puntuacionRobada += puntosRobo;
        PlayerPrefs.SetInt("PuntuacionPartida", PlayerPrefs.GetInt("PuntuacionPartida") - puntosRobo);
        PlayerPrefs.Save();
        robado = true;
        yield return new WaitForSecondsRealtime(tiempoRobo);
        robado = false;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("PuntuacionPartida", PlayerPrefs.GetInt("PuntuacionPartida") + puntuacionRobada);
        PlayerPrefs.Save();
    }
}
