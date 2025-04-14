using UnityEngine;
using TMPro;

public class ScoreManagerAltura : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI scoreText;

    public int pointsPerUnit = 10;
    private float maxYReached;
    private int score;

    void Start()
    {
        ResetScore();
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

    public void ResetScore()
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

        maxYReached = player != null ? player.position.y : 0f;
        score = 0;

        if (scoreText != null)
            scoreText.text = "Puntos: 0";
    }

    public int GetScore() => score;
}
