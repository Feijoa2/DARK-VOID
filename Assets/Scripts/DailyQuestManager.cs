using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class DailyQuest
{
    public string id;
    public string description;
    public int targetAmount;
    public int currentAmount;
    public bool isCompleted;
    public int rewardCoins;
    public QuestType type;
}

public enum QuestType
{
    KillEnemies,
    CompleteLevels,
    RepelEnemies,
    CollectCoins
}

public class DailyQuestManager : MonoBehaviour
{
    public static DailyQuestManager Instance;

    [Header("Шаблоны заданий")]
    [SerializeField] private List<DailyQuest> questTemplates = new List<DailyQuest>();

    private List<DailyQuest> activeQuests = new List<DailyQuest>();

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

        GenerateNewQuests();
    }

    void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnPlayerDied += HandlePlayerDied;
        GameEvents.OnLevelCompleted += HandleLevelCompleted;
        GameEvents.OnEnemyRepelled += HandleEnemyRepelled;
    }

    void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnPlayerDied -= HandlePlayerDied;
        GameEvents.OnLevelCompleted -= HandleLevelCompleted;
        GameEvents.OnEnemyRepelled -= HandleEnemyRepelled;
    }

    private void HandleEnemyKilled() => UpdateProgress(QuestType.KillEnemies, 1);
    private void HandlePlayerDied() { /* пока ничего */ }
    private void HandleLevelCompleted(int levelId) => UpdateProgress(QuestType.CompleteLevels, 1);
    private void HandleEnemyRepelled() => UpdateProgress(QuestType.RepelEnemies, 1);

    private void UpdateProgress(QuestType type, int amount)
    {
        bool changed = false;

        foreach (var quest in activeQuests)
        {
            if (quest.type == type && !quest.isCompleted)
            {
                quest.currentAmount += amount;
                changed = true;

                if (quest.currentAmount >= quest.targetAmount)
                    quest.isCompleted = true;
            }
        }

        if (changed)
            Debug.Log($"[DailyQuestManager] Прогресс: {type} +{amount}");
    }

    private void GenerateNewQuests()
    {
        activeQuests.Clear();

        int count = Mathf.Min(3, questTemplates.Count);

        for (int i = 0; i < count; i++)
        {
            var quest = new DailyQuest
            {
                id = questTemplates[i].id,
                description = questTemplates[i].description,
                targetAmount = questTemplates[i].targetAmount,
                currentAmount = 0,
                isCompleted = false,
                rewardCoins = questTemplates[i].rewardCoins,
                type = questTemplates[i].type
            };
            activeQuests.Add(quest);
        }

        Debug.Log($"[DailyQuestManager] Создано заданий: {activeQuests.Count}");
    }

    public List<DailyQuest> GetActiveQuests() => activeQuests;
}
