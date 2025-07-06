using System.Collections.Generic;
using DevNote.Modules.Levels;

namespace DevNote
{
    public static class GameStateParcer
    {
        private const string ADS_ENABLED_KEY = "adsEnabled";
        private const string LEVEL_PROGRESS = "levelProgress";

        public static void Parse(Dictionary<string, string> data)
        {
            GameState.AdsEnabled = new ReactiveValue<bool>
                (data.ContainsKey(ADS_ENABLED_KEY) ? bool.Parse(data[ADS_ENABLED_KEY]) : true);

            var levelProgressData = data.ContainsKey(LEVEL_PROGRESS) ? data[LEVEL_PROGRESS] : string.Empty;
            GameState.LevelProgress = new LevelProgress(levelProgressData);
        }

        public static Dictionary<string, string> ToDataString()
        {
            var data = new Dictionary<string, string>
            {
                { ADS_ENABLED_KEY, GameState.AdsEnabled.ToString() },
                { LEVEL_PROGRESS, GameState.LevelProgress.ToString() },
            };

            return data;
        }
    }
}


