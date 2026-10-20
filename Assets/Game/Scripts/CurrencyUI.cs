using UnityEngine;
using UnityEngine.UI;

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
            int coins = PlayerPrefs.GetInt("Coins", 0);
            coinsText.text = $"Монеты: {coins}";
        }

        if (crystalsText != null)
        {
            int crystals = PlayerPrefs.GetInt("Crystals", 0);
            crystalsText.text = $"Кристаллы: {crystals}";
        }
    }
}