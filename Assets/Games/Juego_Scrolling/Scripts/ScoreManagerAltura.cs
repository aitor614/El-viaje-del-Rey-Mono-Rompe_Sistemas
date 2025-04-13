using UnityEngine;
using TMPro;

public class ScoreManagerAltura : MonoBehaviour
{
    public PlayerInfierno player;
    public int pointsPerUnit = 10;
    private float maxYReached = 0f;
    private int score = 0;
    public Transform posicionPlayer;
    int scorePrevio;

    void Start()
    {
        // Asignar automáticamente si no está configurado desde el Inspector
        if (player == null)
        {
            if (player != null) posicionPlayer = player.transform;
        }

        if (player != null)
            maxYReached = posicionPlayer.position.y;
    }

    void Update()
    {
        if (player == null) return;
        if (posicionPlayer.position.y > maxYReached)
        {
            maxYReached = posicionPlayer.position.y;
            score = Mathf.FloorToInt(maxYReached * pointsPerUnit);
        }
        if (score > scorePrevio + 10)
        {
            PlayerPrefs.SetInt("PuntuacionPartida", score);
            scorePrevio = score;
        }

        scorePrevio = score;
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
