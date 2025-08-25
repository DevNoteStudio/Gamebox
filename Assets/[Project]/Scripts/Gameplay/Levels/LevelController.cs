
namespace DevNote.Gamebox
{
    public class LevelController
    {
        public delegate void OnLevelStart(int locationIndex, int levelIndex);
        public event OnLevelStart OnLevelStarted;

        public delegate void OnLevelComplete(int locationIndex, int levelIndex, int stars);
        public event OnLevelComplete OnLevelCompleted;


        public void StartLevel(int locationIndex, int levelIndex)
        {
            GameState.Levels.SetCurrentLevel(locationIndex, levelIndex);
            OnLevelStarted?.Invoke(locationIndex, levelIndex);
        }

        public void CompleteCurrentLevel(int stars)
        {
            GameState.Levels.CompleteLevel
                (GameState.Levels.LocationIndex, GameState.Levels.LevelIndex, stars);

            OnLevelCompleted?.Invoke(GameState.Levels.LocationIndex, GameState.Levels.LevelIndex, stars);
        }

        public int GetLastLevelIndex(int locationIndex)
        {
            var levels = Configs.LevelUp.GetLevelsAmount(locationIndex);

            for (int levelIndex = 0; levelIndex < levels; levelIndex++)
            {
                if (GameState.Levels.LevelIsCompleted(locationIndex, levelIndex) == false)
                    return levelIndex;
            }

            return levels - 1;
        }

        public int GetCompletedLevels(int locationIndex)
        {
            var levels = Configs.LevelUp.GetLevelsAmount(locationIndex);

            for (int levelIndex = 0; levelIndex < levels; levelIndex++)
            {
                if (GameState.Levels.LevelIsCompleted(locationIndex, levelIndex) == false)
                    return levelIndex;
            }

            return levels;
        }






    }
}