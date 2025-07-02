using System;
using System.Collections.Generic;
using System.Linq;

namespace DevNote
{
    public static class GameStateParcer
    {
        private const string ADS_ENABLED_KEY = "adsEnabled";
        private const string STAR_COUNT = "starCount";
        private const string COMPLETED_LEVELS = "completedLevels";


        public static void Parse(Dictionary<string, string> data)
        {
            GameState.AdsEnabled = new ReactiveValue<bool>
                (data.ContainsKey(ADS_ENABLED_KEY) ? bool.Parse(data[ADS_ENABLED_KEY]) : true);

            GameState.StarCount = new ReactiveValue<int>
                (data.ContainsKey(STAR_COUNT) ? int.Parse(data[STAR_COUNT]) : 0);

            GameState.CompletedLevels = new ReactiveValue<List<int>>
                (data.ContainsKey(COMPLETED_LEVELS) ?
                    new List<int>(Array.ConvertAll(data[COMPLETED_LEVELS].Split(','), int.Parse)) :
                    Enumerable.Repeat(0, (Enum.GetValues(typeof(LocationType)).Length)).ToList());
        }

        public static Dictionary<string, string> ToDataString()
        {
            var data = new Dictionary<string, string>();

            data.Add(ADS_ENABLED_KEY, GameState.AdsEnabled.Value.ToString());
            data.Add(STAR_COUNT, GameState.StarCount.Value.ToString());
            data.Add(COMPLETED_LEVELS, string.Join(",", GameState.CompletedLevels.Value));

            return data;
        }
    }
}


