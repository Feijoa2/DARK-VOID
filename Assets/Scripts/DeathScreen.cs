using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    void Start()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(() => GameManager.Instance.RestartLevel());

        if (menuButton != null)
            menuButton.onClick.AddListener(() => GameManager.Instance.BackToMenu());
    }
}
    