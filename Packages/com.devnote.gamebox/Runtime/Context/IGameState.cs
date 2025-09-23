using DevNote;

namespace Gamebox
{
    public interface IGameState
    {
        public static LevelsState Levels { get; protected set; }
        public static ItemsState Items { get; protected set; }
        public static ReactiveValue<int> LastPlayLocationIndex { get; protected set; }
    }
}

