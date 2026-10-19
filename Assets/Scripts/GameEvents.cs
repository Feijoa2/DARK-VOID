using System;

public static class GameEvents
{
    // События игры
    public static event Action OnEnemyRepelled;
    public static event Action OnEnemyKilled;
    public static event Action OnPlayerDied;
    public static event Action<int> OnLevelCompleted;
    public static event Action<int> OnCoinsCollected;

    // Методы для вызова событий
    public static void EnemyRepelled() => OnEnemyRepelled?.Invoke();
    public static void EnemyKilled() => OnEnemyKilled?.Invoke();
    public static void PlayerDied() => OnPlayerDied?.Invoke();
    public static void LevelCompleted(int levelId) => OnLevelCompleted?.Invoke(levelId);
    public static void CoinsCollected(int amount) => OnCoinsCollected?.Invoke(amount);
}
