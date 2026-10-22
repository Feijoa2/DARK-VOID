using UnityEngine;
using UnityEngine.UI;

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
        // Небольшая задержка гарантирует, что все менеджеры уже инициализировались
        Invoke(nameof(RefreshUI), 0.1f);
    }

    public void RefreshUI()
    {
        RefreshDailyReward();
        RefreshDailyQuests();
        RefreshAchievements();
        
        // Обновляем валюту на всякий случай при открытии панели
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

        // Очистка старых
        foreach (Transform child in questsContainer)
            Destroy(child.gameObject);

        // Создание новых
        foreach (var quest in DailyQuestManager.Instance.GetActiveQuests())
        {
            var row = Instantiate(questRowPrefab, questsContainer);
            var ui = row.GetComponent<QuestRowUI>();
            if (ui != null) ui.Setup(quest);
        }
    }

    private void RefreshAchievements()
    {
        if (AchievementManager.Instance == null) return;
        if (achievementsContainer == null || achievementRowPrefab == null) return;

        foreach (Transform child in achievementsContainer)
            Destroy(child.gameObject);

        foreach (var ach in AchievementManager.Instance.GetAchievements())
        {
            var row = Instantiate(achievementRowPrefab, achievementsContainer);
            var ui = row.GetComponent<AchievementRowUI>();
            if (ui != null) ui.Setup(ach);
        }
    }

    public void OnClaimDailyReward()
    {
        Debug.Log("[ProgressPanelUI] Кнопка ежедневной награды нажата!");
    
        if (DailyRewardManager.Instance)
        {
            Debug.LogError("[ProgressPanelUI] DailyRewardManager НЕ НАЙДЕН!");
            return;
        }

        Debug.Log("[ProgressPanelUI] Вызываем ClaimReward...");
        int amount = DailyRewardManager.Instance.ClaimReward();
    
        if (amount > 0)
        {
            Debug.Log($"[ProgressPanelUI] ✅ Получено {amount} монет!");
            RefreshUI();
        }
        else
        {
            Debug.LogWarning("[ProgressPanelUI]  Награда не получена (уже забрана сегодня?)");
        }
    }
}