using System;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    public class LevelController
    {
        public event Action OnLevelStarted, OnLevelLost, OnLevelCompleted, OnLevelExit, OnRevive;

        public int CurrentLocationIndex { get; private set; } = -1;
        public int CurrentLevelIndex { get; private set; } = -1;
        public int CompletedStars { get; private set; } = -1;

        public bool IsLevelPlaying { get; private set; } = false;

        private bool _isLevelPlayRepeat;

        private readonly Viewer<LoseWindowView> loseWindowViewer;
        private readonly Viewer<VictoryScreenView> victoryScreenViewer;
        private readonly MenuController menuController;
        private readonly ILeaderboards leaderboards;


        public LevelController(MenuController menuController, ILeaderboards leaderboards)
        {
            loseWindowViewer = new(Configs.Gamebox.LoseWindowPrefab);
            victoryScreenViewer = new(Configs.Gamebox.VictoryScreenPrefab);
            this.menuController = menuController;
            this.leaderboards = leaderboards;
        }


        public void StartNextLevelOrShowLevelSelection()
        {
            bool isLastLevel = CurrentLevelIndex == Configs.Gamebox.GetLocationLevelsAmount(CurrentLocationIndex) - 1;

            if (!_isLevelPlayRepeat)
            {
                if (isLastLevel)
                {
                    int nextLocationIndex = (CurrentLocationIndex + 1) % Configs.Gamebox.LocationsAmount;
                    menuController.ShowLocationsScreen(nextLocationIndex);
                    OnLevelExit?.Invoke();
                }
                else StartLevel(CurrentLocationIndex, CurrentLevelIndex + 1);
            }
            else
            {
                menuController.ShowLevelsScreen(CurrentLocationIndex);
                OnLevelExit?.Invoke();
            }
        }


        public void Revive()
        {
            IsLevelPlaying = true;
            OnRevive?.Invoke();
        }


        public void ExitLevel()
        {
            IsLevelPlaying = false;
            OnLevelExit?.Invoke();
        }


        public void StartLevel(int locationIndex, int levelIndex)
        {
            _isLevelPlayRepeat = GameState.Levels.GetLevelStars(locationIndex, levelIndex) > 0;

            GameState.LastPlayLocationIndex = locationIndex;
            CurrentLocationIndex = locationIndex;
            CurrentLevelIndex = levelIndex;
            CompletedStars = 0;

            IsLevelPlaying = true;
            OnLevelStarted?.Invoke();
        }

        public void CompleteCurrentLevel(int stars)
        {
            CompletedStars = stars;

            int previousStars = GameState.Levels.GetLevelStars(CurrentLocationIndex, CurrentLevelIndex);
            int newStars = Mathf.Max(0, stars - previousStars);

            GameState.Levels.SetLevelStars(CurrentLocationIndex, CurrentLevelIndex, Mathf.Max(previousStars, stars));

            int completedLevels = GameState.Levels.CompletedLevels;

            var rewards = Configs.Gamebox.GetLevelRewards
                (CurrentLocationIndex, CurrentLevelIndex, newStars, _isLevelPlayRepeat, completedLevels);

            foreach (var reward in rewards )
                GameState.Items.Add(reward.Item1, reward.Item2);

            var victoryScreen = victoryScreenViewer.ShowExpand(UI.Container);

            bool showBonus = GameState.Levels.CompletedLevels >= Configs.Gamebox.VictoryRouletteFromLevel;

            victoryScreen.Display(stars, rewards, showBonus);
            victoryScreen.AnimateShow();

            leaderboards.SetScore(completedLevels, LeaderboardKey.Stars);

            IsLevelPlaying = false;
            OnLevelCompleted?.Invoke();
        }

        public void LoseCurrentLevel()
        {
            var loseWindow = loseWindowViewer.ShowExpand(UI.Container);

            bool showRevive = GameState.Levels.CompletedLevels >= Configs.Gamebox.ReviveFromLevel - 1;

            loseWindow.Display(showRevive);
            loseWindow.AnimateShow();

            IsLevelPlaying = false;
            OnLevelLost?.Invoke();
        }


        public void HideVictoryScreen()
        {
            victoryScreenViewer.Hide();
        }

        public void HideLoseWindow(bool useHideAnimation)
        {
            if (useHideAnimation) loseWindowViewer.View.AnimateHide(onCompleted: loseWindowViewer.Hide);
            else loseWindowViewer.Hide(); 
        }




    }
}