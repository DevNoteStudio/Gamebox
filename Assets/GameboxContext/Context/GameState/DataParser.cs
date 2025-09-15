using System.Collections.Generic;

namespace DevNote
{

    public static partial class GameState // DataParser
    {

        private static class DataParser
        {
            public const string NO_ADS_PURCHASED_KEY = "noAdsPurchased";
            public const string LEVELS_KEY = "levels";
            public const string LAST_PLAY_LOCATION_INDEX = "lastPlayLocationIndex";
            public const string ITEMS_KEY = "items";


            public static void Parse(Dictionary<string, string> data)
            {
                NoAdsPurchased = new(bool.Parse(data.GetValueOrDefault(NO_ADS_PURCHASED_KEY, "false")));
                Levels = new(data.GetValueOrDefault(LEVELS_KEY, string.Empty));
                LastPlayLocationIndex = int.Parse(data.GetValueOrDefault(LAST_PLAY_LOCATION_INDEX, "0"));
                Items = new(data.GetValueOrDefault(ITEMS_KEY, string.Empty));
            }

            public static Dictionary<string, string> ToDataString()
            {
                var data = new Dictionary<string, string>
                {
                    { NO_ADS_PURCHASED_KEY, NoAdsPurchased.ToString() },
                    { LEVELS_KEY, Levels.ToString() },
                    { LAST_PLAY_LOCATION_INDEX, LastPlayLocationIndex.ToString() },
                    { ITEMS_KEY, Items.ToString() }
                };

                return data;
            }


        }
    }

    
}


