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


        private const string LEVELS = "levels";
        private const string LAST_PLAY_LOCATION = "lastLocIndex";
        private const string ITEMS = "items";
        private const string ITEM_TUTORIALS = "tutors";


        protected static void ParseState(Dictionary<string, string> data)
        {
            Levels = new(data.GetValueOrDefault(LEVELS, string.Empty));
            LastPlayLocationIndex = new(int.Parse(data.GetValueOrDefault(LAST_PLAY_LOCATION, "0")));
            Items = new(data.GetValueOrDefault(ITEMS, string.Empty));
            ItemTutorials = new(data.GetValueOrDefault(ITEM_TUTORIALS, string.Empty));
        }

        protected static Dictionary<string, string> ToDictionary() => new()
        {
            { LEVELS, Levels.ToString() },
            { LAST_PLAY_LOCATION, LastPlayLocationIndex.ToString() },
            { ITEMS, Items.ToString() },
            { ITEM_TUTORIALS, ItemTutorials.ToString() }
        };






    }
}

