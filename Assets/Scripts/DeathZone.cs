using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Transform player;
    private float checkDelay = 0.5f;
    private float timeSinceStart = 0f;

    void Start()
    {
        InvokeRepeating(nameof(FindPlayer), 0.5f, 1f);
    }

    void Update()
    {
        timeSinceStart += Time.deltaTime;

        if (player == null || timeSinceStart < checkDelay) return;

        if (player.position.y < Camera.main.transform.position.y - 6f)
        {
            GameManager.Instance.PlayerFell();
        }
    }

    private void FindPlayer()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p.transform;
                Debug.Log(" DeathZone reconectó al Player");
            }
        }
    }
}
