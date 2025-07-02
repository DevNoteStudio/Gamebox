using System.Collections.Generic;
using System.Linq;

namespace DevNote
{
    public static class GameStateParcer
    {
        private const string ADS_ENABLED_KEY = "adsEnabled";
        private const string STAR_COUNT = "starCount";
        private const string CURRENT_LOCATION = "currentLocation";
        private const string CURRENT_LEVEL = "currentLevel";
        private const string COMPLETED_LEVELS = "completedLevels";


        public static void Parse(Dictionary<string, string> data)
        {
            GameState.AdsEnabled = new ReactiveValue<bool>
                (data.ContainsKey(ADS_ENABLED_KEY) ? bool.Parse(data[ADS_ENABLED_KEY]) : true);

            GameState.StarCount = new ReactiveValue<int>
                (data.ContainsKey(STAR_COUNT) ? int.Parse(data[STAR_COUNT]) : 0);

            GameState.CurrentLocation = new ReactiveValue<int>
                (data.ContainsKey(CURRENT_LOCATION) ? int.Parse(data[CURRENT_LOCATION]) : 0);

            GameState.CurrentLevel = new ReactiveValue<int>
                (data.ContainsKey(CURRENT_LEVEL) ? int.Parse(data[CURRENT_LEVEL]) : 0);

            GameState.CompletedLevels = new ReactiveValue<List<List<int>>>(new List<List<int>>());

            if (data.ContainsKey(COMPLETED_LEVELS))
            {
                var completedLevels = data[COMPLETED_LEVELS]
                    .Split(',')
                    .Select(location => location.Split(':')
                        .Select(int.Parse)
                        .ToList())
                    .ToList();
                GameState.CompletedLevels.Value = completedLevels;
            }
            else
            {
                GameState.CompletedLevels.Value = new List<List<int>>();

                for (int i = 0; i < Configs.Locations.List.Count; i++)
                {
                    GameState.CompletedLevels.Value.
                        Add(Enumerable.Repeat(0, Configs.Locations.List[i].LevelCount).ToList());
                }
            }
        }

        public static Dictionary<string, string> ToDataString()
        {
            var data = new Dictionary<string, string>();

            data.Add(ADS_ENABLED_KEY, GameState.AdsEnabled.Value.ToString());
            data.Add(STAR_COUNT, GameState.StarCount.Value.ToString());
            data.Add(CURRENT_LOCATION, GameState.CurrentLocation.Value.ToString());
            data.Add(CURRENT_LEVEL, GameState.CurrentLevel.Value.ToString());
            data.Add(COMPLETED_LEVELS, string.Join(",", 
                GameState.CompletedLevels.Value.Select(level => string.Join(":", level))));

            return data;
        }
    }
}


