using DevNote.Modules.Levels;

namespace DevNote
{
    public static class GameState
    {
        public static string GetEncodedData() => GameStateEncoder.Encode(GameStateParcer.ToDataString());
        public static void RestoreFromEncodedData(string data) => GameStateParcer.Parse(GameStateEncoder.Decode(data));

        public static ReactiveValue<bool> AdsEnabled { get; set; }
        public static LevelProgress LevelProgress { get; set; }
    }
}

