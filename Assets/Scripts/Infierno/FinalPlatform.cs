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
            PlayerPrefs.SetInt("ObjetoInfierno", 1);
            PlayerPrefs.Save();
        }
    }

}
