using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenScript : MonoBehaviour
{
    public GameObject panelSettings;
    public GameObject panelMenu;
    public GameObject panelQuests;       // ← новая панель

    // Тексты прогресса
    public Text questsText;              // ← текст с данными

    private void Start()
    {
        panelSettings.SetActive(false);
        if (panelQuests != null) panelQuests.SetActive(false);
    }

    public void CloseSettings()
    {
        panelSettings.SetActive(false);
        panelMenu.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Settings()
    {
        panelMenu.SetActive(false);
        panelSettings.SetActive(true);
    }

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

    public void Exit()
    {
        Application.Quit();
    }
}

