using UnityEngine;
using System;
using YG;

public class DailyRewardManager : MonoBehaviour
{
    public static DailyRewardManager Instance;

    [SerializeField] private int dailyRewardAmount = 30;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public int ClaimReward()
    {
        // ПРОВЕРКА: SDK готов?
        if (!YandexInitializer.IsSDKReady)
        {
            Debug.LogWarning("[DailyRewardManager] SDK ещё не готов! Попробуйте позже.");
            return 0;
        }

        if (!CanClaimToday()) 
        {
            Debug.LogWarning("[DailyRewardManager] Награду уже забрали сегодня!");
            return 0;
        }

        string today = System.DateTime.Now.ToString("yyyy-MM-dd");
        YG2.saves.lastDailyRewardDate = today;
        YG2.saves.coinsCollected += dailyRewardAmount;
    
        Debug.Log($"[DailyRewardManager] Сохраняем {dailyRewardAmount} монет. Всего: {YG2.saves.coinsCollected}");
        YG2.SaveProgress();

        return dailyRewardAmount;
    }

    public bool CanClaimToday()
    {
        string lastClaim = YG2.saves.lastDailyRewardDate;
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        bool canClaim = lastClaim != today;
    
        Debug.Log($"[DailyRewardManager] CanClaimToday: lastClaim='{lastClaim}', today='{today}', result={canClaim}");
        return canClaim;
    }

    public int GetRewardAmount() => dailyRewardAmount;
}