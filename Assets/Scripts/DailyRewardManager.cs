using UnityEngine;
using System;

public class DailyRewardManager : MonoBehaviour
{
    public static DailyRewardManager Instance;

    [Header("Настройки")]
    [SerializeField] private int dailyRewardAmount = 30;

    private const string LastClaimKey = "LastDailyRewardClaim";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public bool CanClaimToday()
    {
        string lastClaim = PlayerPrefs.GetString(LastClaimKey, "");
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        return lastClaim != today;
    }

    public int ClaimReward()
    {
        if (!CanClaimToday()) return 0;

        string today = DateTime.Now.ToString("yyyy-MM-dd");
        PlayerPrefs.SetString(LastClaimKey, today);
        PlayerPrefs.Save();

        int currentCoins = PlayerPrefs.GetInt("Coins", 0);
        PlayerPrefs.SetInt("Coins", currentCoins + dailyRewardAmount);
        PlayerPrefs.Save();

        return dailyRewardAmount;
    }

    public int GetRewardAmount() => dailyRewardAmount;
}