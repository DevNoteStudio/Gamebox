using Cysharp.Threading.Tasks;

namespace Gamebox
{
    public class EffectController
    {

        private GameplayScreenView GameplayScreen => levelController.GameplayScreen;

        private readonly LevelController levelController;
        private readonly CurrencyController currencyController;


        public EffectController(LevelController levelController, CurrencyController currencyController)
        {
            this.levelController = levelController;
            this.currencyController = currencyController;

            levelController.OnLevelStarted += OnLevelStarted;
        }

        private async void OnLevelStarted()
        {
            await UniTask.NextFrame();

            // <-- Display with using effects -->
            if (!levelController.IsFirstLevelInGameSession && levelController.IsLevelStartedFirstTime)
            {
                bool hasBoosterUnlock =
                    IConfigs.Gamebox.TryGetUnlockedBoosterKey(IGameState.Level, out var boosterKey);

                // <- Coins ->
                currencyController.UpdateCoinsRollup(particles: 8, delay: 0.3f);

                // <- Booster unlock ->
                if (hasBoosterUnlock)
                {
                    GameplayScreen.BoosterPanel.GetButton(boosterKey).AnimateUnlock(delay: 1.5f);
                    GameplayScreen.LeadersButton.Display();
                }

                // <- Leaderboard ->
                else if (IConfigs.Gamebox.ContentPipeline.IsAvailable(ContentKey.UnlockLeaderboard))
                {
                    GameplayScreen.LeadersButton.AnimateUpdateRank(delay: 1.3f);
                }

            }

            // <-- Display without effects -->
            else
            {
                GameplayScreen.LeadersButton.Display();

            }

        }
    }
}
