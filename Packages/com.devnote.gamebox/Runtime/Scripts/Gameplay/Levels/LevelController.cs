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
        private readonly LeagueController leagueController;
        private readonly ILeaderboards leaderboards;
        private readonly IAds ads;

        public LevelController(MenuController menuController, ILeaderboards leaderboards, 
            IAds ads, LeagueController leagueController)
        {
            loseWindowViewer = new(IConfigs.GetViewPrefab<LoseWindowView>());
            victoryScreenViewer = new(IConfigs.GetViewPrefab<VictoryScreenView>());
            this.menuController = menuController;
            this.leaderboards = leaderboards;
            this.ads = ads;
            this.leagueController = leagueController;
        }


        public void StartNextLevelOrShowLevelSelection()
        {
            bool isLastLevel = CurrentLevelIndex == IConfigs.Gamebox.GetLocationLevelsAmount(CurrentLocationIndex) - 1;

            if (!_isLevelPlayRepeat)
            {
                if (isLastLevel)
                {
                    int nextLocationIndex = (CurrentLocationIndex + 1) % IConfigs.Gamebox.LocationsAmount;
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
            _isLevelPlayRepeat = IGameState.Levels.GetLevelStars(locationIndex, levelIndex) > 0;

            IGameState.LastPlayLocationIndex.Value = locationIndex;
            IGameState.LastPlayLevelIndex.Value = levelIndex;
            CurrentLocationIndex = locationIndex;
            CurrentLevelIndex = levelIndex;
            CompletedStars = 0;

            IsLevelPlaying = true;
            OnLevelStarted?.Invoke();
        }




        public void CompleteCurrentLevel(int stars)
        {
            IsLevelPlaying = false;

            stars = Mathf.Clamp(stars, 1, 3);

            CompletedStars = stars;

            int previousStars = IGameState.Levels.GetLevelStars(CurrentLocationIndex, CurrentLevelIndex);
            int newStars = Mathf.Max(0, stars - previousStars);

            bool isFirstComplete = previousStars <= 0;

            IGameState.Levels.SetLevelStars(CurrentLocationIndex, CurrentLevelIndex, Mathf.Max(previousStars, stars));

            int completedLevels = IGameState.Levels.CompletedLevels;

            var rewards = IConfigs.Gamebox.GetLevelRewards
                (CurrentLocationIndex, CurrentLevelIndex, newStars, _isLevelPlayRepeat, completedLevels, isFirstComplete);

            foreach (var reward in rewards )
                IGameState.Items.Add(reward.itemKey, reward.amount);

            var victoryScreen = victoryScreenViewer.ShowExpand(UI.Container);

            bool showBonus = IGameState.Levels.CompletedLevels >= IConfigs.Gamebox.VictoryRouletteFromLevel
                && ads.RewardedAvailable;

            int rewardRating = IConfigs.Gamebox.GetRatingForLevelCompletion
                (CurrentLocationIndex, CurrentLevelIndex, newStars);

            var currentLeague = IConfigs.Gamebox.GetLeagueType(IGameState.Rating.Value);
            int maxRewardRating = IConfigs.Gamebox.IsLastLeague(currentLeague) ?
                int.MaxValue : IConfigs.Gamebox.GetLeagueRatingRequire(currentLeague + 1);

            rewardRating = Mathf.Min(rewardRating, maxRewardRating);

            IGameState.Rating.Value += rewardRating;
            var currentLeagueNow = IConfigs.Gamebox.GetLeagueType(IGameState.Rating.Value);

            if (currentLeagueNow > currentLeague)
                leagueController.ApplyNewLeagueReward(currentLeagueNow);

            int fromRating = IGameState.Rating.Value - rewardRating;
            int toRating = IGameState.Rating.Value;

            victoryScreen.Display(stars, fromRating, toRating, rewards, showBonus);
            victoryScreen.AnimateShow();

            leaderboards.SetScore(IGameState.Items.Get(ItemKey.Stars), LeaderboardKey.Stars);

            OnLevelCompleted?.Invoke();
        }

        public void LoseCurrentLevel()
        {
            var loseWindow = loseWindowViewer.ShowFaded(UI.Container);

            bool showRevive = (IGameState.Levels.CompletedLevels >= IConfigs.Gamebox.ReviveFromLevel - 1)
                && ads.RewardedAvailable;

            loseWindow.Display(showRevive);
            loseWindow.AnimateShow();

            IsLevelPlaying = false;
            OnLevelLost?.Invoke();
        }


        public void HideVictoryScreen()
        {
            victoryScreenViewer.Hide();
        }

        public void HideLoseWindow(bool forceHide)
        {
            if (forceHide) loseWindowViewer.ForceFadedHide();
            else loseWindowViewer.AnimateFadedHide(loseWindowViewer.View.AnimateHide);
        }




    }
}