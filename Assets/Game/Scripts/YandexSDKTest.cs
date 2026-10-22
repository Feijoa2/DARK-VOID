using UnityEngine;
using UnityEngine.InputSystem;
using YG;

public class YandexSDKTest : MonoBehaviour
{
    private Keyboard keyboard;
    
    void Start()
    {
        // Подписываемся на инициализацию SDK
        YG2.onGetSDKData += OnSDKReady;
        
        // Если SDK уже готов
        if (YG2.isSDKEnabled)
        {
            OnSDKReady();
        }
        else
        {
            Debug.Log("⏳ Ждём инициализацию SDK...");
        }
    }
    
    void Awake()
    {
        keyboard = Keyboard.current;
    }

    private void OnSDKReady()
    {
        Debug.Log("═══════════════════════════════════════");
        Debug.Log("✅ SDK ИНИЦИАЛИЗИРОВАН!");
        Debug.Log("═══════════════════════════════════════");
        
        // Данные игрока
        Debug.Log($"👤 Имя игрока: {YG2.player.name}");
        Debug.Log($"🆔 ID игрока: {YG2.player.id}");
        Debug.Log($" Авторизован: {YG2.player.auth}");
        
        // Платформа
        Debug.Log($"🎮 Платформа: {YG2.platform}");
        Debug.Log($" SDK включён: {YG2.isSDKEnabled}");
        Debug.Log($" Первая сессия: {YG2.isFirstGameSession}");
        
        // Сохранения
        Debug.Log($"💰 Монеты: {YG2.saves.coinsCollected}");
        Debug.Log($"💎 Кристаллы: {YG2.saves.crystalsCollected}");
        Debug.Log($"📅 Последняя награда: {YG2.saves.lastDailyRewardDate}");
        
        Debug.Log("═══════════════════════════════════════");
    }

    // Метод для проверки сохранения (повесьте на кнопку или вызывайте через клавишу)
    public void TestSave()
    {
        Debug.Log("🧪 ТЕСТ СОХРАНЕНИЯ...");
        
        YG2.saves.coinsCollected += 100;
        Debug.Log($"➕ Добавили 100 монет. Теперь: {YG2.saves.coinsCollected}");
        
        YG2.SaveProgress();
        Debug.Log(" Сохранено!");
        
        FindAnyObjectByType<CurrencyUI>()?.Refresh();
    }

    // Метод для проверки рекламы между уровнями
    public void TestInterstitial()
    {
        Debug.Log("📺 ТЕСТ МЕЖСТРАНИЧНОЙ РЕКЛАМЫ...");
        YG2.InterstitialAdvShow();
    }

    // Метод для проверки рекламы за вознаграждение
    public void TestRewarded()
    {
        Debug.Log("🎁 ТЕСТ РЕКЛАМЫ ЗА ВОЗНАГРАЖДЕНИЕ...");
        YG2.RewardedAdvShow("testReward", () => {
            Debug.Log("✅ Награда получена!");
            YG2.saves.coinsCollected += 50;
            YG2.SaveProgress();
        });
    }

    // Метод для проверки лидерборда
    public void TestLeaderboard()
    {
        Debug.Log("🏆 ТЕСТ ЛИДЕРБОРДА...");
        YG2.SetLeaderboard("testLeaderboard", 100);
        Debug.Log(" Рекорд отправлен: 100");
    }
    
    public void CheckUI()
    {
        Debug.Log("=== ПРОВЕРКА UI ===");
    
        var currencyUI = FindObjectOfType<CurrencyUI>();
        if (currencyUI == null)
        {
            Debug.LogError("❌ CurrencyUI НЕ НАЙДЕН на сцене!");
            return;
        }
    
        Debug.Log($"✅ CurrencyUI найден");
        Debug.Log($"💰 Монеты в saves: {YG2.saves.coinsCollected}");
    
        currencyUI.Refresh();
        Debug.Log("🔄 Вызван Refresh()");
    }

    void Update()
    {
        if (keyboard != null)
        {
            if (keyboard.f1Key.wasPressedThisFrame) TestSave();
            if (keyboard.f2Key.wasPressedThisFrame) TestInterstitial();
            if (keyboard.f3Key.wasPressedThisFrame) TestRewarded();
            if (keyboard.f4Key.wasPressedThisFrame) TestLeaderboard();
        }
    }

    void OnDestroy()
    {
        YG2.onGetSDKData -= OnSDKReady;
    }
}