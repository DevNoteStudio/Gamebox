using DevNote.Gamebox;

namespace DevNote
{
    public static partial class GameState // Saves
    {
        public static ReactiveValue<bool> NoAdsPurchased { get; set; }
        public static LevelsState Levels { get; set; }
    }
}

