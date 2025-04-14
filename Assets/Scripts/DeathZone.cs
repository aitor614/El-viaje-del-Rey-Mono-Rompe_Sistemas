using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        FindPlayer();
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        if (player.position.y < Camera.main.transform.position.y - 6f)
        {
            GameManager.Instance.PlayerFell();
        }
    }

    private void FindPlayer()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }
}
