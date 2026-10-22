using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Экран смерти")]
    [SerializeField] private GameObject deathScreen;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (deathScreen != null)
            deathScreen.SetActive(false);
        
        StartCoroutine(AutoSaveRoutine());
    }
    
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("[GameManager] Игра свернута. Сохраняем прогресс...");
            YG2.SaveProgress();
        }
    }
    
    private void OnApplicationQuit()
    {
        Debug.Log("[GameManager] Выход из игры. Сохраняем прогресс...");
        YG2.SaveProgress();
    }

    
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        
        YG2.SaveProgress();
        
        GameEvents.PlayerDied(); 
        Time.timeScale = 0f;
        if (deathScreen != null) deathScreen.SetActive(true);

        // ОПЦИОНАЛЬНО: Показать рекламу за вознаграждение для возрождения
        // YandexAdsManager.Instance.ShowRewardedAd("Revive", () => { RevivePlayer(); });
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        
        YG2.SaveProgress();
        
        // Показать межуровневую рекламу ПЕРЕД перезагрузкой
        if (YG2.isSDKEnabled)
        {
            YG2.InterstitialAdvShow();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        
        YG2.SaveProgress();
        
        // Показать межуровневую рекламу ПЕРЕД выходом в меню
        if (YG2.isSDKEnabled)
        {
            YG2.InterstitialAdvShow();
        }

        SceneManager.LoadScene("Menu");
    }
    
    // Фоновая страховка: сохраняем каждые 60 секунд
    private System.Collections.IEnumerator AutoSaveRoutine()
    {
        var waitForSeconds = new WaitForSeconds(60f);
        while (true)
        {
            yield return waitForSeconds;
            // Сохраняем только если игра не на паузе
            if (Time.timeScale > 0f) 
            {
                Debug.Log("[GameManager] Автосохранение...");
                YG2.SaveProgress();
            }
        }
    }
}