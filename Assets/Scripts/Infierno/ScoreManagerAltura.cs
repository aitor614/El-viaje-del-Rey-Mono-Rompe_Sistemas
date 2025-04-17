using UnityEngine;
using TMPro;

public class ScoreManagerAltura : MonoBehaviour
{
    [Header("Componentes")]
    public PlayerInfierno player;
    public Transform posicionPlayer;

    [Header("Parámetros")]
    public int pointsPerUnit = 10;

    private float maxYReached = 0f;
    private int score = 0;
    int scorePrevio;
    float alturaInicial;


    void Start()
    {
        if (player != null)
        {
            posicionPlayer = player.transform;
            alturaInicial = posicionPlayer.position.y;
            maxYReached = alturaInicial;
        }
    }

    void Update()
    {
        if (player == null) return;

        float yActual = posicionPlayer.position.y;

        if (yActual > maxYReached)
        {
            maxYReached = yActual;

            float diferenciaAltura = maxYReached - alturaInicial;
            score = Mathf.FloorToInt(diferenciaAltura * pointsPerUnit);
        }

        if (score > scorePrevio + 10)
        {
            PlayerPrefs.SetInt("PuntuacionPartida", score);
            PlayerPrefs.Save();
            scorePrevio = score;
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void SubtractPoints(int amount)
    {
        score -= amount;
        if (score < 0) score = 0;
    }
}
