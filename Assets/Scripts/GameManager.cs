using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int maxLives = 3;
    private int currentLives;
    public LifeDisplay lifeDisplay;

    private Vector3 startPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentLives = maxLives;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            startPosition = player.transform.position;
        }
    }

    public void PlayerFell()
    {
        currentLives--;
        Debug.Log("Vida perdida. Vidas restantes: " + currentLives);

        if (lifeDisplay != null)
        {
            lifeDisplay.UpdateLives(currentLives);
        }

        if (currentLives > 0)
        {
            RespawnPlayer();
        }
        else
        {
            SceneManager.LoadScene("DefeatScene");
        }


    }

    private void RespawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = startPosition;
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    public void ResetLives()
    {
        currentLives = maxLives;
    }

    public string gameSceneName = "SampleScene";

    public void PlayAgain()
    {
        // Reiniciar vidas si quieres empezar de cero
        GameManager.Instance.ResetLives();

        // Cargar la escena del juego
        SceneManager.LoadScene(gameSceneName);
    }
}