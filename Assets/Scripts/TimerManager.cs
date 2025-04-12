using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeElapsed = 0f;
    private bool isRunning = true;

    void Start()
    {
        // Si no está asignado, lo busca por nombre
        if (timerText == null)
        {
            GameObject txt = GameObject.Find("TimerText");
            if (txt != null)
                timerText = txt.GetComponent<TextMeshProUGUI>();
        }

        timeElapsed = 0f;
        isRunning = true;

    }

    void Update()
    {
        if (isRunning && timerText != null)
        {
            timeElapsed += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timeElapsed / 60f);
            int seconds = Mathf.FloorToInt(timeElapsed % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void Initialize()
    {
        if (timerText == null)
        {
            GameObject t = GameObject.Find("TimerText");
            if (t != null) timerText = t.GetComponent<TextMeshProUGUI>();
        }

        timeElapsed = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetTime()
    {
        return timeElapsed;
    }
}
