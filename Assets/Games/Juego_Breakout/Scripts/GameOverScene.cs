using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    public void ResetGame()
    {
        SceneManager.LoadScene("MenuPrincipal");


    }
}
