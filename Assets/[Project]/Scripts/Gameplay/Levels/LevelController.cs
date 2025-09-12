
using System;

namespace DevNote.Gamebox
{
    public class LevelController
    {
        public event Action OnLevelStarted, OnLevelLost, OnLevelCompleted;

        public int CurrentLocationIndex { get; private set; } = -1;
        public int CurrentLevelIndex { get; private set; } = -1;
        public int CompletedStars { get; private set; } = -1;



        public void StartLevel(int locationIndex, int levelIndex)
        {
            CurrentLocationIndex = locationIndex;
            CurrentLevelIndex = levelIndex;
            CompletedStars = 0;

            OnLevelStarted?.Invoke();
        }

        public void CompleteCurrentLevel(int stars)
        {
            CompletedStars = stars;
            GameState.Levels.SetLevelStars(CurrentLocationIndex, CurrentLevelIndex, stars);

            OnLevelCompleted?.Invoke();
        }

        public void LoseCurrentLevel()
        {
            OnLevelLost?.Invoke();
        }


        public void ShowLocationsScreen(int locationIndex)
        {

        }

        public void HideLocationsScreen()
        {

        }



        public void ShowLevelsScreen(int locationIndex)
        {

        }

        public void HideLevelsScreen()
        {

        }





    }
}