using UnityEngine;
using System.Collections.Generic;
using YG;

[System.Serializable]
public class Achievement
{
    public string id;
    public string title;
    public string description;
    public int targetAmount;
    public int currentAmount;
    public bool isCompleted;
    public int rewardCoins;
    public AchievementType type;
}

public enum AchievementType
{
    FirstKill,
    KillEnemies20,
    FirstLevel,
    CompleteLevels10,
    FirstDeath
}

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    [SerializeField] private List<Achievement> achievements = new List<Achievement>();

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }

        LoadAchievementsFromSaves();
    }

    void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnLevelCompleted += HandleLevelCompleted;
        GameEvents.OnPlayerDied += HandlePlayerDied;
    }

    void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnLevelCompleted -= HandleLevelCompleted;
        GameEvents.OnPlayerDied -= HandlePlayerDied;
    }

    private void HandleEnemyKilled()
    {
        UpdateProgress(AchievementType.FirstKill, 1);
        UpdateProgress(AchievementType.KillEnemies20, 1);
    }

    private void HandleLevelCompleted(int levelId)
    {
        UpdateProgress(AchievementType.FirstLevel, 1);
        UpdateProgress(AchievementType.CompleteLevels10, 1);
    }

    private void HandlePlayerDied()
    {
        UpdateProgress(AchievementType.FirstDeath, 1);
    }

    private void UpdateProgress(AchievementType type, int amount)
    {
        foreach (var ach in achievements)
        {
            if (ach.type == type && !ach.isCompleted)
            {
                ach.currentAmount += amount;

                if (ach.currentAmount >= ach.targetAmount)
                {
                    ach.isCompleted = true;
                    Debug.Log($"[AchievementManager] ✅ Достижение выполнено: {ach.title}");
                    
                    // ОТПРАВЛЯЕМ В ЛИДЕРБОРД ЯНДЕКС ИГР
                    SendToLeaderboard(ach);
                    
                    // Сохраняем прогресс
                    UpdateAchievementsInSaves();
                    YG2.SaveProgress();
                }
            }
        }
    }

    // Метод отправки в лидерборд
    private void SendToLeaderboard(Achievement ach)
    {
        if (!YandexInitializer.IsSDKReady)
        {
            Debug.LogWarning("[AchievementManager] SDK не готов, лидерборд не отправлен");
            return;
        }

        // ach.id должен совпадать с техническим именем лидерборда в консоли Яндекса
        YG2.SetLeaderboard(ach.id, ach.currentAmount);
        Debug.Log($"[AchievementManager] 📤 Отправлено в лидерборд: {ach.id} = {ach.currentAmount}");
    }

    private void UpdateAchievementsInSaves()
    {
        YG2.saves.achievementIds.Clear();
        YG2.saves.achievementProgress.Clear();
        
        foreach (var ach in achievements)
        {
            YG2.saves.achievementIds.Add(ach.id);
            YG2.saves.achievementProgress.Add(ach.currentAmount);
        }
    }

    private void LoadAchievementsFromSaves()
    {
        for (int i = 0; i < YG2.saves.achievementIds.Count; i++)
        {
            string id = YG2.saves.achievementIds[i];
            int progress = YG2.saves.achievementProgress[i];
            
            foreach (var ach in achievements)
            {
                if (ach.id == id)
                {
                    ach.currentAmount = progress;
                    ach.isCompleted = ach.currentAmount >= ach.targetAmount;
                    break;
                }
            }
        }
    }

    public List<Achievement> GetAchievements() => achievements;
}