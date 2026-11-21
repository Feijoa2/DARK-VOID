using UnityEngine;
using UnityEngine.UI;

public class ProgressTabsController : MonoBehaviour
{
    [Header("Кнопки вкладок")]
    [SerializeField] private Button rewardsTabButton;
    [SerializeField] private Button questsTabButton;

    [Header("Содержимое вкладок")]
    [SerializeField] private GameObject rewardsContent;
    [SerializeField] private GameObject questsContent;

    [Header("Подчёркивания")]
    [SerializeField] private GameObject rewardsUnderline;
    [SerializeField] private GameObject questsUnderline;

    private void Start()
    {
        rewardsTabButton.onClick.AddListener(ShowRewards);
        questsTabButton.onClick.AddListener(ShowQuests);

        ShowRewards();
    }

    public void ShowRewards()
    {
        rewardsContent.SetActive(true);
        questsContent.SetActive(false);

        rewardsUnderline.SetActive(true);
        questsUnderline.SetActive(false);
    }

    public void ShowQuests()
    {
        rewardsContent.SetActive(false);
        questsContent.SetActive(true);

        rewardsUnderline.SetActive(false);
        questsUnderline.SetActive(true);
    }
}
