using UnityEngine;
using UnityEngine.UI;
using YG;

public class ProgressPanelUI : MonoBehaviour
{
    [Header("Daily Reward")]
    [SerializeField] private Button dailyRewardButton;
    [SerializeField] private Text dailyRewardText;

    [Header("Daily Quests")]
    [SerializeField] private Transform questsContainer;
    [SerializeField] private GameObject questRowPrefab;

    [Header("Achievements")]
    [SerializeField] private Transform achievementsContainer;
    [SerializeField] private GameObject achievementRowPrefab;

    void Start()
    {
        Invoke(nameof(RefreshUI), 0.2f);
    }

    public void RefreshUI()
    {
        Debug.Log("[ProgressPanelUI] 🔄 RefreshUI ВЫЗВАН!");
        
        RefreshDailyReward();
        RefreshDailyQuests();
        RefreshAchievements();
        
        FindAnyObjectByType<CurrencyUI>()?.Refresh();
    }

    private void RefreshDailyReward()
    {
        if (DailyRewardManager.Instance == null) return;

        bool canClaim = DailyRewardManager.Instance.CanClaimToday();
        if (dailyRewardButton != null) dailyRewardButton.interactable = canClaim;
        
        if (dailyRewardText != null)
        {
            dailyRewardText.text = canClaim
                ? $"Ежедневная награда: +{DailyRewardManager.Instance.GetRewardAmount()} монет"
                : "Награда уже получена. Приходи завтра!";
        }
    }

    private void RefreshDailyQuests()
    {
        if (DailyQuestManager.Instance == null) return;
        if (questsContainer == null || questRowPrefab == null) return;

        foreach (Transform child in questsContainer) Destroy(child.gameObject);

        foreach (var quest in DailyQuestManager.Instance.GetActiveQuests())
        {
            var row = Instantiate(questRowPrefab, questsContainer);
            var ui = row.GetComponent<QuestRowUI>();
            if (ui != null) ui.Setup(quest);
        }
    }

    private void RefreshAchievements()
    {
        Debug.Log("[ProgressPanelUI] 🏆 Начало RefreshAchievements");
        
        if (AchievementManager.Instance == null) { Debug.LogError("❌ AchievementManager.Instance == null"); return; }
        if (achievementsContainer == null) { Debug.LogError("❌ achievementsContainer == null (проверь Inspector!)"); return; }
        if (achievementRowPrefab == null) { Debug.LogError("❌ achievementRowPrefab == null (проверь Inspector!)"); return; }

        foreach (Transform child in achievementsContainer) Destroy(child.gameObject);

        int count = 0;
        foreach (var ach in AchievementManager.Instance.GetAchievements())
        {
            var row = Instantiate(achievementRowPrefab, achievementsContainer);
            var ui = row.GetComponent<AchievementRowUI>();
            if (ui != null) 
            {
                ui.Setup(ach);
                count++;
            }
            else
            {
                Debug.LogError("❌ На префабе строки НЕТ компонента AchievementRowUI!");
            }
        }
        Debug.Log($"[ProgressPanelUI] ✅ Создано строк достижений: {count}");
    }

    public void OnClaimDailyReward()
    {
        Debug.Log("[ProgressPanelUI] Кнопка ежедневной награды нажата!");
    
        if (DailyRewardManager.Instance == null) 
        {
            Debug.LogError("[ProgressPanelUI] DailyRewardManager НЕ НАЙДЕН!");
            return;
        }

        int amount = DailyRewardManager.Instance.ClaimReward();
        if (amount > 0)
        {
            Debug.Log($"[ProgressPanelUI] ✅ Получено {amount} монет!");
            RefreshUI();
        }
    }
}