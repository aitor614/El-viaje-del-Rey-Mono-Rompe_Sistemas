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
        // Asignar automáticamente si no está configurado desde el Inspector
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (player != null)
            maxYReached = player.position.y;
    }

    void Update()
    {
        if (player == null) return;

        if (player.position.y > maxYReached)
        {
            maxYReached = player.position.y;
            score = Mathf.FloorToInt(maxYReached * pointsPerUnit);
            scoreText.text = "Puntos: " + score;
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
        scoreText.text = "Puntos: " + score;
    }
}
