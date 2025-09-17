using DevNote.Gamebox;

namespace DevNote
{
    public static partial class GameState // Saves
    {
        public static ReactiveValue<bool> NoAdsPurchased { get; private set; }
        public static LevelsState Levels { get; private set; }
        public static ItemsState Items { get; private set; }
        public static int LastPlayLocationIndex { get; set; }

    }
}

