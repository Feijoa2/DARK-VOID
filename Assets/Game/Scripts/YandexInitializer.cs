using UnityEngine;
using YG;

public class YandexInitializer : MonoBehaviour
{
    public static bool IsSDKReady { get; private set; } = false;

    void Awake()
    {
        // Подписываемся на событие готовности SDK
        YG2.onGetSDKData += OnSDKInitialized;
        
        // Если SDK уже инициализирован (на всякий случай)
        if (YG2.isSDKEnabled)
        {
            OnSDKInitialized();
        }
    }

    private void OnSDKInitialized()
    {
        Debug.Log("✅ Yandex SDK инициализирован! Теперь можно сохранять.");
        IsSDKReady = true;
    }

    void OnDestroy()
    {
        YG2.onGetSDKData -= OnSDKInitialized;
    }
}