using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void PlayAgain()
    {
        GameManager.Instance.RestartGame();
    }
}
