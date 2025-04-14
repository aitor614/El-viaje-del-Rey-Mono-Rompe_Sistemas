using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void PlayerFell()
    {
        SceneManager.LoadScene("DefeatScene");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene")
        {
            TimerManager timer = FindFirstObjectByType<TimerManager>();
            if (timer != null)
            {
                Debug.Log("Reiniciando cronómetro");
                timer.StartCountdown();
            }
            else
            {
                Debug.LogWarning("No se encontró TimerManager en escena.");
            }

            ScoreManagerAltura scoreManager = FindFirstObjectByType<ScoreManagerAltura>();
            if (scoreManager != null)
                scoreManager.ResetScore();
        }
    }

}
