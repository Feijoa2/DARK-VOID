using UnityEngine;
using System.Collections.Generic;

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

    [Header("Достижения")]
    [SerializeField] private List<Achievement> achievements = new List<Achievement>();

    private const string AchievementsKey = "Achievements";

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

        LoadAchievements();
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
        bool changed = false;

        foreach (var ach in achievements)
        {
            if (ach.type == type && !ach.isCompleted)
            {
                ach.currentAmount += amount;
                changed = true;

                if (ach.currentAmount >= ach.targetAmount)
                {
                    ach.isCompleted = true;
                    Debug.Log($"[AchievementManager] Достижение выполнено: {ach.title}");
                }
            }
        }

        if (changed) SaveAchievements();
    }

    private void SaveAchievements()
    {
        var wrapper = new AchievementListWrapper { achievements = achievements };
        PlayerPrefs.SetString(AchievementsKey, JsonUtility.ToJson(wrapper));
        PlayerPrefs.Save();
    }

    private void LoadAchievements()
    {
        string json = PlayerPrefs.GetString(AchievementsKey, "");
        if (!string.IsNullOrEmpty(json))
        {
            var wrapper = JsonUtility.FromJson<AchievementListWrapper>(json);
            if (wrapper.achievements.Count == achievements.Count)
                achievements = wrapper.achievements;
        }
    }

    public List<Achievement> GetAchievements() => achievements;

    [System.Serializable]
    private class AchievementListWrapper
    {
        public List<Achievement> achievements;
    }
}
