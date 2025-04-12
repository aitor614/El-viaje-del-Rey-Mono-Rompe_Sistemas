using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public string gameSceneName = "SampleScene";

    public void PlayAgain()
    {
        // Resetear completamente el GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetLives();
        }

        // Opcional: también puedes reiniciar el tiempo y puntuación aquí si quieres
        // (aunque ya se hace normalmente en OnSceneLoaded)

        // Cargar la escena del juego
        SceneManager.LoadScene(gameSceneName);
    }

}
