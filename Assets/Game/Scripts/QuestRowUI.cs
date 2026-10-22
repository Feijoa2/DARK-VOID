using UnityEngine;
using UnityEngine.UI;
using YG;
using System.Collections.Generic;

public class QuestRowUI : MonoBehaviour
{
    [Header("Тексты")]
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text progressText;

    [Header("Прогресс")]
    [SerializeField] private Slider progressBar;

    [Header("Кнопка награды")]
    [SerializeField] private Button rewardButton;
    [SerializeField] private Text rewardButtonText;
    [SerializeField] private Image rewardButtonImage;

    [Header("Цвета кнопки")]
    [SerializeField] private Color availableColor = new Color(0.3f, 0.7f, 0.3f);
    [SerializeField] private Color lockedColor = new Color(0.4f, 0.4f, 0.4f);
    [SerializeField] private Color claimedColor = new Color(0.3f, 0.3f, 0.3f);

    [SerializeField] private Color claimedTextColor = new Color(0.3f, 0.9f, 0.3f);
    [SerializeField] private int claimedFontSize = 32;
    [SerializeField] private int normalFontSize = 18;

    private DailyQuest currentQuest;

    public void Setup(DailyQuest quest)
    {
        currentQuest = quest;

        descriptionText.text = quest.description;
        progressBar.maxValue = quest.targetAmount;
        progressBar.value = quest.currentAmount;
        progressText.text = $"{quest.currentAmount} / {quest.targetAmount}";

        UpdateButton();
    }

    private void UpdateButton()
    {
        if (currentQuest == null) return;

        bool isCompleted = currentQuest.currentAmount >= currentQuest.targetAmount;
        
        // ПРОВЕРКА ИЗ ОБЛАКА
        bool isClaimed = YG2.saves.claimedQuestIds.Contains(currentQuest.id);

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
            rewardButtonText.text = $"+{currentQuest.rewardCoins}";
            rewardButtonText.color = Color.white;
            rewardButtonText.fontSize = normalFontSize;
            rewardButtonImage.color = lockedColor;
            rewardButton.interactable = false;
        }
    }

    public void OnClaimReward()
    {
        if (currentQuest == null) return;
        if (currentQuest.currentAmount < currentQuest.targetAmount) return;

        if (YG2.saves.claimedQuestIds.Contains(currentQuest.id)) return;

        // 1. Начисляем монеты
        YG2.saves.coinsCollected += currentQuest.rewardCoins;
        
        // 2. Помечаем квест как выполненный
        YG2.saves.claimedQuestIds.Add(currentQuest.id);
        
        // 3. Сохраняем
        YG2.SaveProgress();

        UpdateButton();
        FindAnyObjectByType<CurrencyUI>()?.Refresh();
    }
}