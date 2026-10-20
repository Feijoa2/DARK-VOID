using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;

    // Счётчики
    private int enemiesRepelled = 0;
    private int enemiesKilled = 0;
    private int levelsCompleted = 0;
    private int coinsCollected = 0;
    private int deathsCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // не уничтожать при смене сцены
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadProgress();
    }

    void OnEnable()
    {
        GameEvents.OnEnemyRepelled += HandleEnemyRepelled;
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnPlayerDied += HandlePlayerDied;
        GameEvents.OnLevelCompleted += HandleLevelCompleted;
        GameEvents.OnCoinsCollected += HandleCoinsCollected;
    }

    void OnDisable()
    {
        GameEvents.OnEnemyRepelled -= HandleEnemyRepelled;
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnPlayerDied -= HandlePlayerDied;
        GameEvents.OnLevelCompleted -= HandleLevelCompleted;
        GameEvents.OnCoinsCollected -= HandleCoinsCollected;
    }

    // === Обработчики ===
    private void HandleEnemyRepelled()
    {
        enemiesRepelled++;
        SaveProgress();
    }

    private void HandleEnemyKilled()
    {
        enemiesKilled++;
        SaveProgress();
    }

    private void HandlePlayerDied()
    {
        deathsCount++;
        SaveProgress();
    }

    private void HandleLevelCompleted(int levelId)
    {
        levelsCompleted++;
        SaveProgress();
    }

    private void HandleCoinsCollected(int amount)
    {
        coinsCollected += amount;
        SaveProgress();
    }

    // === Сохранение / загрузка ===
    private void SaveProgress()
    {
        PlayerPrefs.SetInt("EnemiesRepelled", enemiesRepelled);
        PlayerPrefs.SetInt("EnemiesKilled", enemiesKilled);
        PlayerPrefs.SetInt("LevelsCompleted", levelsCompleted);
        PlayerPrefs.SetInt("CoinsCollected", coinsCollected);
        PlayerPrefs.SetInt("DeathsCount", deathsCount);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        enemiesRepelled = PlayerPrefs.GetInt("EnemiesRepelled", 0);
        enemiesKilled = PlayerPrefs.GetInt("EnemiesKilled", 0);
        levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted", 0);
        coinsCollected = PlayerPrefs.GetInt("CoinsCollected", 0);
        deathsCount = PlayerPrefs.GetInt("DeathsCount", 0);
    }

    // === Геттеры ===
    public int EnemiesRepelled => enemiesRepelled;
    public int EnemiesKilled => enemiesKilled;
    public int LevelsCompleted => levelsCompleted;
    public int CoinsCollected => coinsCollected;
    public int DeathsCount => deathsCount;

    // === Сброс (для отладки) ===
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        enemiesRepelled = 0;
        enemiesKilled = 0;
        levelsCompleted = 0;
        coinsCollected = 0;
        deathsCount = 0;
    }

}
