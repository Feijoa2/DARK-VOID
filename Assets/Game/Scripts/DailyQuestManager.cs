using UnityEngine;
using System;
using System.Collections.Generic;
using YG;

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

public enum QuestType { KillEnemies, CompleteLevels, RepelEnemies, CollectCoins }

public class DailyQuestManager : MonoBehaviour
{
    public static DailyQuestManager Instance;
    [SerializeField] private List<DailyQuest> questTemplates = new List<DailyQuest>();
    private List<DailyQuest> activeQuests = new List<DailyQuest>();

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }

        if (YG2.saves.claimedQuestIds == null) YG2.saves.claimedQuestIds = new List<string>();

        string today = DateTime.Now.ToString("yyyy-MM-dd");
        if (YG2.saves.lastQuestsDate != today)
        {
            YG2.saves.lastQuestsDate = today;
            YG2.saves.claimedQuestIds.Clear(); // Сброс забранных наград
            GenerateNewQuests();
            YG2.SaveProgress();
        }
        else
        {
            // Если день тот же, просто генерируем их снова. 
            // (В идеале прогресс нужно хранить в SavesYG, но для отображения этого достаточно, 
            // так как GameEvents будут обновлять их в реальном времени)
            GenerateNewQuests(); 
        }
    }

    void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnLevelCompleted += HandleLevelCompleted;
        GameEvents.OnEnemyRepelled += HandleEnemyRepelled;
    }

    void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnLevelCompleted -= HandleLevelCompleted;
        GameEvents.OnEnemyRepelled -= HandleEnemyRepelled;
    }

    private void HandleEnemyKilled() => UpdateProgress(QuestType.KillEnemies, 1);
    private void HandleLevelCompleted(int levelId) => UpdateProgress(QuestType.CompleteLevels, 1);
    private void HandleEnemyRepelled() => UpdateProgress(QuestType.RepelEnemies, 1);

    private void UpdateProgress(QuestType type, int amount)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.type == type && !quest.isCompleted)
            {
                quest.currentAmount += amount;
                if (quest.currentAmount >= quest.targetAmount)
                {
                    quest.isCompleted = true;
                    YG2.SaveProgress();
                }
            }
        }
    }

    private void GenerateNewQuests()
    {
        activeQuests.Clear();
        int count = Mathf.Min(3, questTemplates.Count);
        for (int i = 0; i < count; i++)
        {
            activeQuests.Add(new DailyQuest
            {
                id = questTemplates[i].id,
                description = questTemplates[i].description,
                targetAmount = questTemplates[i].targetAmount,
                currentAmount = 0,
                isCompleted = false,
                rewardCoins = questTemplates[i].rewardCoins,
                type = questTemplates[i].type
            });
        }
    }

    public List<DailyQuest> GetActiveQuests() => activeQuests;
}