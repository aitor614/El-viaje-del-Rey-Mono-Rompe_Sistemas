using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int lives = 3;

    private Ball ball;

    private Player player;

    public Temp tempScript;

    public float leftTime = 30f;



    void FinishTime()
    {
        if (leftTime > 0)
        {
            leftTime -= Time.deltaTime;
            if (leftTime < 0)
                leftTime = 0;

            tempScript.refreshText(leftTime); // Mostramos el tiempo
        }

        if (leftTime == 0)
        {
            GameOver();
        }

    }

    public void looseHealth()
    {
        lives--;



        if (lives <= 0)
        {
            GameOver();

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

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    void Update()
    {
        FinishTime();
    }

}
