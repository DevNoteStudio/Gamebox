using System.Collections.Generic;

namespace DevNote
{
    public static class GameState
    {
        public static string GetEncodedData() => GameStateEncoder.Encode(GameStateParcer.ToDataString());
        public static void RestoreFromEncodedData(string data) => GameStateParcer.Parse(GameStateEncoder.Decode(data));


        public static ReactiveValue<bool> AdsEnabled;
        public static ReactiveValue<int> StarCount;
        public static ReactiveValue<int> CurrentLocation;
        public static ReactiveValue<int> CurrentLevel;
        public static ReactiveValue<List<List<int>>> CompletedLevels;
    }
}

