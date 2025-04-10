using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        if (player != null && player.position.y < Camera.main.transform.position.y - 6f)
        {
            GameManager.Instance.PlayerFell();
        }
    }
}