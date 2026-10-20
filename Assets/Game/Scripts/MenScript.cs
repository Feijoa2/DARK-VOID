using UnityEngine;
using UnityEngine.SceneManagement;

public class MenScript : MonoBehaviour
{
    [Header("Панели")]
    public GameObject panelSettings;
    public GameObject panelMenu;
    public GameObject panelQuests;
    public GameObject panelShop; // Новая панель магазина

    private void Start()
    {
        // Скрываем все панели при старте
        panelSettings.SetActive(false);
        if (panelQuests != null) panelQuests.SetActive(false);
        if (panelShop != null) panelShop.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    // --- Настройки ---
    public void Settings()
    {
        panelMenu.SetActive(false);
        panelSettings.SetActive(true);
    }

    public void CloseSettings()
    {
        panelSettings.SetActive(false);
        panelMenu.SetActive(true);
    }

    // --- Прогресс (Квесты) ---
    public void Quests()
    {
        panelMenu.SetActive(false);
        panelQuests.SetActive(true);
    }

    public void CloseQuests()
    {
        panelQuests.SetActive(false);
        panelMenu.SetActive(true);
    }

    // --- Магазин ---
    public void Shop()
    {
        panelMenu.SetActive(false);
        panelShop.SetActive(true);
    }

    public void CloseShop()
    {
        panelShop.SetActive(false);
        panelMenu.SetActive(true);
    }

    // --- Выход ---
    public void Exit()
    {
        Application.Quit();
    }
}
