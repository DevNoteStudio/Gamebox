using System.Collections.Generic;
using DevNote;

namespace Gamebox
{
    public interface IGameState
    {
        public static LevelsState Levels { get; private set; }
        public static ItemsState Items { get; private set; }
        public static ItemTutorialsState ItemTutorials { get; private set; }
        public static ReactiveValue<int> LastPlayLocationIndex { get; private set; }
        public static ReactiveValue<int> LastPlayLevelIndex { get; private set; }
        public static ReactiveValue<bool> GameRated { get; private set; }


        private const string LEVELS = "levels";
        private const string LAST_PLAY_LOCATION_INDEX = "lastLocIndex";
        private const string LAST_PLAY_LEVEL_INDEX = "lastLevIndex";
        private const string ITEMS = "items";
        private const string ITEM_TUTORIALS = "tutors";
        private const string GAME_RATED = "rated";


        protected static void ParseState(Dictionary<string, string> data)
        {
            Levels = new(data.GetValueOrDefault(LEVELS, string.Empty));
            LastPlayLocationIndex = new(int.Parse(data.GetValueOrDefault(LAST_PLAY_LOCATION_INDEX, "0")));
            LastPlayLevelIndex = new(int.Parse(data.GetValueOrDefault(LAST_PLAY_LEVEL_INDEX, "0")));
            Items = new(data.GetValueOrDefault(ITEMS, string.Empty));
            ItemTutorials = new(data.GetValueOrDefault(ITEM_TUTORIALS, string.Empty));
            GameRated = new(data.GetValueOrDefault(GAME_RATED, "0").FromBinaryToBool());
        }

        protected static Dictionary<string, string> ToDictionary() => new()
        {
            { LEVELS, Levels.ToString() },
            { LAST_PLAY_LOCATION_INDEX, LastPlayLocationIndex.ToString() },
            { LAST_PLAY_LEVEL_INDEX, LastPlayLevelIndex.ToString() },
            { ITEMS, Items.ToString() },
            { ITEM_TUTORIALS, ItemTutorials.ToString() },
            { GAME_RATED, GameRated.Value.ToBinaryString() }
        };






    }
}

