using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int lives = 3;

    private Ball ball;

    private Player player;

    public void looseHealth()
    {
        lives--;

        if (lives <= 0)
        {
            SceneManager.LoadScene("GameOver");
         
        }
        else
        {
            ball = FindFirstObjectByType<Ball>();
            player = FindFirstObjectByType<Player>();
            ResetLevel();
        }
    }

    public void ResetLevel()
    {
        ball.ResetBall();
        player.ResetPlayer();
    }
}
