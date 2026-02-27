using System.Collections.Generic;
using DevNote;

namespace Gamebox
{
    public interface IGameState
    {
        public static LevelsState Levels { get; private set; }
        public static int Level { get; set; }
        public static bool LevelWasStarted { get; set; } 
        public static ItemsState Items { get; private set; }
        public static CardsState Cards { get; private set; }
        public static ItemTutorialsState ItemTutorials { get; private set; }
        public static ReactiveValue<int> LastPlayLocationIndex { get; private set; }
        public static ReactiveValue<int> LastPlayLevelIndex { get; private set; }
        public static ReactiveValue<bool> GameRated { get; private set; }
        public static ReactiveValue<int> Rating { get; private set; }
        public static ReactiveValue<bool> BoxAndCardTutorialCompleted { get; private set; }


        private const string LEVEL = "level";
        private const string LEVEL_WAS_STARTED = "lvl_start";
        private const string LEVELS = "levels";
        private const string RATING = "rating";
        private const string LAST_PLAY_LOCATION_INDEX = "lastLocIndex";
        private const string LAST_PLAY_LEVEL_INDEX = "lastLevIndex";
        private const string ITEMS = "items";
        private const string ITEM_TUTORIALS = "tutors";
        private const string GAME_RATED = "rated";
        private const string CARDS = "cards";
        private const string BOX_AND_CARD_TUTORIAL_COMPLETED = "boxCardTutorial";


        protected static void ParseState(Dictionary<string, string> data)
        {
            Level = int.Parse(data.GetValueOrDefault(LEVEL, $"{1}"));
            LevelWasStarted = bool.Parse(data.GetValueOrDefault(LEVEL_WAS_STARTED, $"{false}"));
            Levels = new(data.GetValueOrDefault(LEVELS, string.Empty));
            LastPlayLocationIndex = new(int.Parse(data.GetValueOrDefault(LAST_PLAY_LOCATION_INDEX, "0")));
            LastPlayLevelIndex = new(int.Parse(data.GetValueOrDefault(LAST_PLAY_LEVEL_INDEX, "0")));
            Items = new(data.GetValueOrDefault(ITEMS, string.Empty));
            ItemTutorials = new(data.GetValueOrDefault(ITEM_TUTORIALS, string.Empty));
            GameRated = new(data.GetValueOrDefault(GAME_RATED, "0").FromBinaryToBool());
            Rating = new(int.Parse(data.GetValueOrDefault(RATING, "0")));
            Cards = new(data.GetValueOrDefault(CARDS, string.Empty));
            BoxAndCardTutorialCompleted = new(data.GetValueOrDefault(BOX_AND_CARD_TUTORIAL_COMPLETED, "0").FromBinaryToBool());
        }

        protected static Dictionary<string, string> ToDictionary() => new()
        {
            { LEVEL, Level.ToString() },
            { LEVEL_WAS_STARTED, LevelWasStarted.ToString() },
            { LEVELS, Levels.ToString() },
            { LAST_PLAY_LOCATION_INDEX, LastPlayLocationIndex.ToString() },
            { LAST_PLAY_LEVEL_INDEX, LastPlayLevelIndex.ToString() },
            { ITEMS, Items.ToString() },
            { ITEM_TUTORIALS, ItemTutorials.ToString() },
            { GAME_RATED, GameRated.Value.ToBinaryString() },
            { RATING, Rating.ToString() },
            { CARDS, Cards.ToString() },
            { BOX_AND_CARD_TUTORIAL_COMPLETED, BoxAndCardTutorialCompleted.Value.ToBinaryString() },
        };






    }
}

