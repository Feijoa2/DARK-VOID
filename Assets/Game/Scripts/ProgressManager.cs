using UnityEngine;
using YG;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
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

    private void HandleEnemyRepelled() => YG2.saves.enemiesRepelled++;
    private void HandleEnemyKilled() => YG2.saves.enemiesKilled++;
    private void HandlePlayerDied() => YG2.saves.deathsCount++;
    
    private void HandleLevelCompleted(int levelId)
    {
        YG2.saves.levelsCompleted++;
        YG2.SaveProgress(); 
    }

    private void HandleCoinsCollected(int amount)
    {
        YG2.saves.coinsCollected += amount;
    }

    public int EnemiesRepelled => YG2.saves.enemiesRepelled;
    public int EnemiesKilled => YG2.saves.enemiesKilled;
    public int LevelsCompleted => YG2.saves.levelsCompleted;
    public int CoinsCollected => YG2.saves.coinsCollected;
    public int DeathsCount => YG2.saves.deathsCount;

    public void ResetProgress()
    {
        YG2.saves.enemiesRepelled = 0;
        YG2.saves.enemiesKilled = 0;
        YG2.saves.levelsCompleted = 0;
        YG2.saves.coinsCollected = 0;
        YG2.saves.deathsCount = 0;
        YG2.SaveProgress(); // При сбросе сохраняем обязательно
    }
}