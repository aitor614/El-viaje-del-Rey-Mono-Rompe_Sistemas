using UnityEngine;
using TMPro;

public class ScoreManagerAltura : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI scoreText;

    public int pointsPerUnit = 10;
    private float maxYReached = 0f;
    private int score = 0;

    void Start()
    {
        // Buscar automáticamente el Player si no está asignado
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        // Buscar automáticamente el ScoreText si no está asignado
        if (scoreText == null)
        {
            GameObject txt = GameObject.Find("ScoreText");
            if (txt != null) scoreText = txt.GetComponent<TextMeshProUGUI>();
        }

        if (player != null)
            maxYReached = player.position.y;
    }

    void Update()
    {
        if (player == null || scoreText == null) return;

        if (player.position.y > maxYReached)
        {
            maxYReached = player.position.y;
            score = Mathf.FloorToInt(maxYReached * pointsPerUnit);
            scoreText.text = "Puntos: " + score;
        }
    }

    public void Initialize()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (scoreText == null)
        {
            GameObject txt = GameObject.Find("ScoreText");
            if (txt != null) scoreText = txt.GetComponent<TextMeshProUGUI>();
        }

        if (player != null)
            maxYReached = player.position.y;

        score = 0;
        if (scoreText != null)
            scoreText.text = "Puntos: 0";
    }

    public int GetScore()
    {
        return score;
    }

    public void SubtractPoints(int amount)
    {
        score -= amount;
        if (score < 0) score = 0;

        if (scoreText != null)
            scoreText.text = "Puntos: " + score;
    }
}
