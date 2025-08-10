using DevNote.LevelUp;

namespace DevNote
{
    public static partial class GameState // Saves
    {
        public static ReactiveValue<bool> NoAdsPurchased { get; set; }
        public static LevelStore Levels { get; set; }
    }
}

