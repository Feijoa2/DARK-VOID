using UnityEngine;
using UnityEngine.UI;
using YG;
using System.Collections.Generic;

public class AchievementRowUI : MonoBehaviour
{
    [Header("Тексты")]
    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text progressText;
    [Header("Прогресс")]
    [SerializeField] private Slider progressBar;
    [Header("Кнопка награды")]
    [SerializeField] private Button rewardButton;
    [SerializeField] private Text rewardButtonText;
    [SerializeField] private Image rewardButtonImage;
    [Header("Цвета")]
    [SerializeField] private Color availableColor = new Color(0.3f, 0.7f, 0.3f);
    [SerializeField] private Color lockedColor = new Color(0.4f, 0.4f, 0.4f);
    [SerializeField] private Color claimedColor = new Color(0.3f, 0.3f, 0.3f);
    [SerializeField] private Color claimedTextColor = new Color(0.3f, 0.9f, 0.3f);
    [SerializeField] private int claimedFontSize = 32;
    [SerializeField] private int normalFontSize = 18;

    private Achievement currentAchievement;

    public void Setup(Achievement ach)
    {
        currentAchievement = ach;
        titleText.text = ach.title;
        descriptionText.text = ach.description;
        progressBar.maxValue = ach.targetAmount;
        progressBar.value = ach.currentAmount;
        progressText.text = $"{ach.currentAmount} / {ach.targetAmount}";
        UpdateButton();
    }

    private void UpdateButton()
    {
        if (currentAchievement == null) return;
        if (YG2.saves.claimedAchievementIds == null) YG2.saves.claimedAchievementIds = new List<string>(); // ЗАЩИТА

        bool isCompleted = currentAchievement.currentAmount >= currentAchievement.targetAmount;
        bool isClaimed = YG2.saves.claimedAchievementIds.Contains(currentAchievement.id);

        if (isClaimed)
        {
            rewardButtonText.text = "✓";
            rewardButtonText.color = claimedTextColor;
            rewardButtonText.fontSize = claimedFontSize;
            rewardButtonImage.color = claimedColor;
            rewardButton.interactable = false;
        }
        else if (isCompleted)
        {
            rewardButtonText.text = "Забрать";
            rewardButtonText.color = Color.white;
            rewardButtonText.fontSize = normalFontSize;
            rewardButtonImage.color = availableColor;
            rewardButton.interactable = true;
        }
        else
        {
            rewardButtonText.text = $"+{currentAchievement.rewardCoins}";
            rewardButtonText.color = Color.white;
            rewardButtonText.fontSize = normalFontSize;
            rewardButtonImage.color = lockedColor;
            rewardButton.interactable = false;
        }
    }

    public void OnClaimReward()
    {
        if (currentAchievement == null || currentAchievement.currentAmount < currentAchievement.targetAmount) return;
        if (YG2.saves.claimedAchievementIds.Contains(currentAchievement.id)) return;

        YG2.saves.coinsCollected += currentAchievement.rewardCoins;
        YG2.saves.claimedAchievementIds.Add(currentAchievement.id);
        YG2.SaveProgress();

        UpdateButton();
        FindAnyObjectByType<CurrencyUI>()?.Refresh();
    }
}