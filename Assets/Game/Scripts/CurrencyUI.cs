using UnityEngine;
using UnityEngine.UI;
using YG;

public class CurrencyUI : MonoBehaviour
{
    [Header("Тексты")]
    [SerializeField] private Text coinsText;
    [SerializeField] private Text crystalsText;

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (coinsText != null)
        {
            Debug.Log($"[CurrencyUI] Обновляем монеты: {YG2.saves.coinsCollected}");
            coinsText.text = $"Монеты: {YG2.saves.coinsCollected}";
        }

        if (crystalsText != null)
        {
            crystalsText.text = $"Кристаллы: {YG2.saves.crystalsCollected}";
        }
    }
}