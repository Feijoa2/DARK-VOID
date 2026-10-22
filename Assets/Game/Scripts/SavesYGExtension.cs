using System.Collections.Generic;
using YG;

namespace YG
{
    // Расширяем стандартный класс сохранений плагина
    // Важно: пространство имен должно быть именно YG
    public partial class SavesYG
    {
        // Из ProgressManager
        public int maxLevelReached = 0;
        public int enemiesRepelled = 0;
        public int enemiesKilled = 0;
        public int levelsCompleted = 0;
        public int coinsCollected = 0;
        public int deathsCount = 0;
        public int crystalsCollected = 0;

        // Из DailyRewardManager
        public string lastDailyRewardDate = "";

        public List<string> claimedAchievementIds = new List<string>();
        public List<string> claimedQuestIds = new List<string>();
        
        // Прогресс достижений (вместо Dictionary — два параллельных списка)
        public List<string> achievementIds = new List<string>();
        public List<int> achievementProgress = new List<int>();
        
        // Из DailyQuestManager (даты последних квестов, чтобы не сбрасывать их при каждом Awake)
        public string lastQuestsDate = "";
    }
}