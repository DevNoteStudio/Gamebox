
namespace DevNote.Modules.Levels
{
    public class LevelController
    {
        public delegate void OnLevelStart(int locationIndex, int levelIndex);
        public event OnLevelStart OnLevelStarted;

        public delegate void OnLevelComplete(int locationIndex, int levelIndex, int stars);
        public event OnLevelComplete OnLevelCompleted;


        public void StartLevel(int locationIndex, int levelIndex)
        {
            GameState.LevelProgress.SetCurrentLevel(locationIndex, levelIndex);
            OnLevelStarted?.Invoke(locationIndex, levelIndex);
        }

        public void CompleteCurrentLevel(int starsAmount)
        {
            GameState.LevelProgress.CompleteLevel
                (GameState.LevelProgress.CurrentLocationIndex, GameState.LevelProgress.CurrentLevelIndex, starsAmount);

            OnLevelCompleted?.Invoke(GameState.LevelProgress.CurrentLocationIndex, GameState.LevelProgress.CurrentLevelIndex, starsAmount);
        }

        public int GetLastAvailableLevelIndexInsideLocation(int locationIndex)
        {
            var levelsAmount = Configs.Levels.LocationDataList[locationIndex].levelsAmount;

            for (int levelIndex = 0; levelIndex < levelsAmount; levelIndex++)
            {
                if (GameState.LevelProgress.LevelIsCompleted(locationIndex, levelIndex) == false)
                    return levelIndex;
            }

            return levelsAmount - 1;
        }

        public int GetCompletedLocationLevelsAmount(int locationIndex)
        {
            var levelsAmount = Configs.Levels.LocationDataList[locationIndex].levelsAmount;

            for (int levelIndex = 0; levelIndex < levelsAmount; levelIndex++)
            {
                if (GameState.LevelProgress.LevelIsCompleted(locationIndex, levelIndex) == false)
                    return levelIndex;
            }

            return levelsAmount;
        }






    }
}