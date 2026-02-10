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
        public bool IsLevelRestarted { get; private set; } = false;
        public int ReviveCount { get; private set; } = 0;


        private bool _isLevelPlayRepeat;

        private readonly Viewer<LoseWindowView> loseWindowViewer;
        private readonly Viewer<VictoryScreenView> victoryScreenViewer;
        private readonly MenuController menuController;
        private readonly LeagueController leagueController;
        private readonly ISave save;
        private readonly ILeaderboards leaderboards;
        private readonly IAds ads;
        private readonly IEnvironment environment;

        public LevelController(MenuController menuController, ILeaderboards leaderboards, 
            IAds ads, LeagueController leagueController, IEnvironment environment, ISave save)
        {
            loseWindowViewer = new(IConfigs.GetViewPrefab<LoseWindowView>());
            victoryScreenViewer = new(IConfigs.GetViewPrefab<VictoryScreenView>());
            this.menuController = menuController;
            this.leaderboards = leaderboards;
            this.ads = ads;
            this.leagueController = leagueController;
            this.environment = environment;
            this.save = save;
        }


        public void StartNextLevelOrShowMenu()
        {
            bool isLastLevel = CurrentLevelIndex == IConfigs.Gamebox.GetLocationLevelsAmount(CurrentLocationIndex) - 1;

            var currentLeague = IConfigs.Gamebox.GetLeagueType(IGameState.Rating.Value);

            if (IConfigs.Gamebox.MenuAvailable && !IGameState.BoxAndCardTutorialCompleted.Value)
            {
                menuController.ShowLocationsScreen(CurrentLocationIndex);
                OnLevelExit?.Invoke();
            }
            else if (!_isLevelPlayRepeat)
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


        public bool TryRevive()
        {
            void Revive()
            {
                Sound.Play(SoundName.Revive);
                environment.StartGameplay();
                ReviveCount++;
                IsLevelPlaying = true;
                OnRevive?.Invoke();
            }

            int maxFreeRevives = IGameState.Cards.IsActive(CardType.Reviver) ?
                IConfigs.Gamebox.GetCardPower(CardType.Reviver) : 0;

            bool freeReviveAvailable = maxFreeRevives - ReviveCount > 0;

            if (freeReviveAvailable)
            {
                Revive();
                return true;
            }
            else
            {
                int gemPrice = IConfigs.Gamebox.ReviveGemPrice;
                if (IGameState.Items.Get(ItemKey.Gems) >= gemPrice)
                {
                    IGameState.Items.Spend(ItemKey.Gems, gemPrice);
                    Revive();
                    return true;
                }

                else return false;
            }
        }


        public void ExitLevel()
        {
            environment.StopGameplay();
            IsLevelPlaying = false;
            OnLevelExit?.Invoke();
        }


        public void StartLevel(int locationIndex, int levelIndex, bool isRestart = false)
        {
            environment.StartGameplay();

            _isLevelPlayRepeat = IGameState.Levels.GetLevelStars(locationIndex, levelIndex) > 0;

            IGameState.LastPlayLocationIndex.Value = locationIndex;
            IGameState.LastPlayLevelIndex.Value = levelIndex;
            CurrentLocationIndex = locationIndex;
            CurrentLevelIndex = levelIndex;
            CompletedStars = 0;

            IsLevelPlaying = true;
            IsLevelRestarted = isRestart;

            ReviveCount = 0;

            OnLevelStarted?.Invoke();
        }


        public void CompleteCurrentLevel(int stars)
        {
            environment.StopGameplay();
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

            int coinsIndex = rewards.FindIndex(itemPack => itemPack.itemKey == ItemKey.Coins);
            if (IGameState.Cards.IsActive(CardType.CoinsMultiplier) && coinsIndex != -1)
            {
                int coinsMultiplierPercentage = IConfigs.Gamebox.GetCardPower(CardType.CoinsMultiplier,
                    IGameState.Cards.GetLevel(CardType.CoinsMultiplier));

                float multiplier = 1f + coinsMultiplierPercentage / 100f;
                int totalCoins = (int)(rewards[coinsIndex].amount * multiplier);
                rewards[coinsIndex] = rewards[coinsIndex].Set(totalCoins);
            }
            if (IGameState.Cards.IsActive(CardType.GemRewarder))
            {
                int gemsPerNewStar = IConfigs.Gamebox.GetCardPower(CardType.GemRewarder,
                    IGameState.Cards.GetLevel(CardType.GemRewarder));

                rewards.Add(new ItemPack(ItemKey.Gems, gemsPerNewStar * newStars));
            }


            foreach (var reward in rewards )
                IGameState.Items.Add(reward.itemKey, reward.amount);

            var victoryScreen = victoryScreenViewer.ShowExpand(UI.Container);

            int rewardRating = IConfigs.Gamebox.GetRatingForLevelCompletion
                (CurrentLocationIndex, CurrentLevelIndex, newStars);

            if (IGameState.Cards.IsActive(CardType.RatingMultiplier))
            {
                int ratingMultiplierPercentage = IConfigs.Gamebox.GetCardPower(CardType.RatingMultiplier, 
                    IGameState.Cards.GetLevel(CardType.RatingMultiplier));

                float multiplier = 1f + ratingMultiplierPercentage / 100f;
                rewardRating = Mathf.RoundToInt(rewardRating * multiplier);
            }

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

            victoryScreen.Display(stars, fromRating, toRating, rewards);
            victoryScreen.AnimateShow();

            leaderboards.SetScore(IGameState.Rating.Value);
            save.FullSave();
            OnLevelCompleted?.Invoke();
        }

        public void LoseCurrentLevel()
        {
            environment.StopGameplay();

            loseWindowViewer.ShowFaded(UI.Container).Display().AnimateShow();

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