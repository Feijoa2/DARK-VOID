using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Экран смерти")]
    [SerializeField] private GameObject deathScreen;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (deathScreen != null)
            deathScreen.SetActive(false);
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        GameEvents.PlayerDied(); // ← новое событие

        Time.timeScale = 0f;
        if (deathScreen != null)
            deathScreen.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}