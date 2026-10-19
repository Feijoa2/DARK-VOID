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
        Invoke(nameof(RefreshUI), 0.1f);
    }

    public void RefreshUI()
    {
        RefreshDailyReward();
        RefreshDailyQuests();
        RefreshAchievements();
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
        if (DailyQuestManager.Instance == null)
        {
            Debug.LogWarning("[ProgressPanelUI] DailyQuestManager не найден!");
            return;
        }

        if (questsContainer == null || questRowPrefab == null) return;

        foreach (Transform child in questsContainer)
            Destroy(child.gameObject);

        foreach (var quest in DailyQuestManager.Instance.GetActiveQuests())
        {
            var row = Instantiate(questRowPrefab, questsContainer);
            var ui = row.GetComponent<QuestRowUI>();
            if (ui != null) ui.Setup(quest);
        }
    }

    private void RefreshAchievements()
    {
        if (AchievementManager.Instance == null)
        {
            Debug.LogWarning("[ProgressPanelUI] AchievementManager не найден!");
            return;
        }

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
        if (DailyRewardManager.Instance == null) return;

        int amount = DailyRewardManager.Instance.ClaimReward();
        if (amount > 0)
        {
            Debug.Log($"[ProgressPanelUI] Получено {amount} монет!");
            RefreshUI();
        }
    }
}
