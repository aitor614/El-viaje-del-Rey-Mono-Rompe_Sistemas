using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float startTimeInSeconds = 90f; // 1 minuto y medio
    private float timeRemaining;
    private bool isRunning;

    void Start()
    {
        StartCountdown();
    }

    void Update()
    {
        if (!isRunning || timerText == null) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;
            timerText.text = "00:00";

            // Cambio a la escena de derrota
            SceneManager.LoadScene("DefeatScene");
        }
        else
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void StartCountdown()
    {
        timeRemaining = startTimeInSeconds;
        isRunning = true;

        if (timerText == null)
        {
            GameObject txt = GameObject.Find("TimerText");
            if (txt != null)
            {
                timerText = txt.GetComponent<TextMeshProUGUI>();
                Debug.Log("TimerText asignado automáticamente.");
            }
        }

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
