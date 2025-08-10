using System.Collections.Generic;

namespace DevNote
{
    public static class GameStateParcer
    {
        public const string NO_ADS_PURCHASED_KEY = "noAdsPurchased";
        public const string LEVELS = "levels";


        public static void Parse(Dictionary<string, string> data)
        {
            bool noAdsPurchased = bool.Parse(data.GetValueOrDefault(NO_ADS_PURCHASED_KEY, "false"));
            GameState.NoAdsPurchased = new (noAdsPurchased);

            string levels = data.GetValueOrDefault(LEVELS, string.Empty);
            GameState.Levels = new (levels);


        }


        public static Dictionary<string, string> ToDataString()
        {
            var data = new Dictionary<string, string>
            {
                { NO_ADS_PURCHASED_KEY, GameState.NoAdsPurchased.ToString() },
                { LEVELS, GameState.Levels.ToString() }
            };

            return data;
        }


    }
}


