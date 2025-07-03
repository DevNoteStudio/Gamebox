using System;
using Zenject;

namespace DevNote.Modules.Levels
{
    public class LevelController : IInitializable
    {
        public event Action OnLevelStarted;

        void IInitializable.Initialize()
        {
            StartLevel(GameState.CurrentLocation.Value, GameState.CurrentLevel.Value);
        }

        public void StartLevel(int location, int level)
        {
            GameState.CurrentLocation.Value = location;
            GameState.CurrentLevel.Value = level;

            SetupLevel();

            OnLevelStarted?.Invoke();
        }

        public void Win(int starCount)
        {
            SetStarsOnLevel(starCount);

            if (Configs.Locations.List[GameState.CurrentLocation.Value].LevelCount - 1 == GameState.CurrentLevel.Value)
                StartLevel(GameState.CurrentLocation.Value + 1, 0);
            else
                StartLevel(GameState.CurrentLocation.Value, GameState.CurrentLevel.Value + 1);
        }

        public void Lose()
        {

        }

        private void SetupLevel()
        {

        }

        private void SetStarsOnLevel(int starCount)
        {
            if (starCount < 1 || starCount > 3)
                throw new ArgumentOutOfRangeException(nameof(starCount), "Star count must be between 1 and 3.");

            if (GameState.CompletedLevels.Value[GameState.CurrentLocation.Value][GameState.CurrentLevel.Value] == 0)
                GameState.CompletedLevels.Value[GameState.CurrentLocation.Value][GameState.CurrentLevel.Value] = starCount;
        }
    }
}