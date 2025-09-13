using System;

namespace DevNote.Gamebox
{
    public class LevelController
    {
        public event Action OnLevelStarted, OnLevelLost, OnLevelCompleted;

        public int CurrentLocationIndex { get; private set; } = -1;
        public int CurrentLevelIndex { get; private set; } = -1;
        public int CompletedStars { get; private set; } = -1;


        private readonly Viewer<LocationsScreenView> locationsScreenViewer;
        private readonly Viewer<LevelsScreenView> levelsScreenViewer;



        public LevelController()
        {
            locationsScreenViewer = new(Configs.Gamebox.LocationsScreenPrefab);
            levelsScreenViewer = new(Configs.Gamebox.LevelsScreenPrefab);
        }



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
            locationsScreenViewer.ShowExpand(UI.Container).Display(locationIndex);
        }

        public void HideLocationsScreen()
        {
            locationsScreenViewer.Hide();
        }



        public void ShowLevelsScreen(int locationIndex)
        {
            levelsScreenViewer.ShowExpand(UI.Container).Display(locationIndex);
        }

        public void HideLevelsScreen()
        {
            levelsScreenViewer.Hide();
        }





    }
}