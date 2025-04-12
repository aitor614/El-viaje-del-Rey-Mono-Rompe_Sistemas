using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int maxLives = 3;
    private int currentLives;
    public LifeDisplay lifeDisplay;
    private bool initialized = false;
    private bool hasFallenThisLife = false;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SetPlayerStartPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            startPosition = player.transform.position;
            Debug.Log("Start position actualizada: " + startPosition);
        }
    }

    public void PlayerFell()
    {
        if (hasFallenThisLife) return;

        hasFallenThisLife = true;
        currentLives--;
        Debug.Log("Vida perdida. Vidas restantes: " + currentLives);

        if (lifeDisplay != null)
        {
            lifeDisplay.UpdateLives(currentLives);
        }

        if (currentLives > 0)
        {
            StartCoroutine(RespawnAfterDelay(1f)); // espera antes del respawn
        }
        else
        {
            StartCoroutine(LoadDefeatSceneWithDelay(1f)); // espera antes de cambiar de escena
        }
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RespawnPlayer();
    }

    private IEnumerator LoadDefeatSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("DefeatScene");
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

            // Reiniciar cámara si tienes seguimiento
            CameraFollow cam = FindAnyObjectByType<CameraFollow>();
            if (cam != null)
            {
                cam.transform.position = new Vector3(cam.transform.position.x, startPosition.y, cam.transform.position.z);
            }
        }

        hasFallenThisLife = false;
    }

    private void FindAndAssignLifeDisplay()
    {
        if (lifeDisplay == null)
        {
            lifeDisplay = Object.FindFirstObjectByType<LifeDisplay>();
        }
    }

    private void InitializeIfNeeded()
    {
        if (initialized) return;

        FindAndAssignLifeDisplay();

        if (lifeDisplay != null)
            lifeDisplay.UpdateLives(currentLives);

        initialized = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeIfNeeded();
        SetPlayerStartPosition(); // NUEVO: cada vez que se carga la escena

        // Reasignar referencias manualmente después del cambio de escena
        TimerManager timer = GetComponent<TimerManager>();
        if (timer != null)
            timer.Initialize();

        ScoreManagerAltura scoreManager = GetComponent<ScoreManagerAltura>();
        if (scoreManager != null)
            scoreManager.Initialize();
    }
    public void ResetLives()
    {
        currentLives = maxLives;
        hasFallenThisLife = false;

        if (lifeDisplay != null)
            lifeDisplay.UpdateLives(currentLives);
    }

    public string gameSceneName = "SampleScene";
}