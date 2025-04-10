using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinalPlatform : MonoBehaviour
{
    public float delayBeforeVictory = 1f; 

    private bool triggered = false;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!triggered && col.collider.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(LoadVictorySceneAfterDelay());

            // Detener cronómetro si lo estás usando
            TimerManager timer = Object.FindFirstObjectByType<TimerManager>();
            if (timer != null)
                timer.StopTimer();
        }
    }

    IEnumerator LoadVictorySceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeVictory);
        SceneManager.LoadScene("VictoryScene");
    }
}
