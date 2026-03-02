using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gamebox
{
    public class EffectController
    {

        private GameplayScreenView GameplayScreen => levelController.GameplayScreen;

        private readonly LevelController levelController;


        public EffectController(LevelController levelController)
        {
            this.levelController = levelController;

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
                int completedLevel = IGameState.Level - 1;
                int coins = IConfigs.Gamebox.GetCoinsForLevelComplete(completedLevel);
                GameplayScreen.CurrencyView.AnimateCoinsRollup(coins, 8, Vector2.zero);


                // <- Booster unlock ->
                if (hasBoosterUnlock)
                {
                    GameplayScreen.BoosterPanel.GetButton(boosterKey).AnimateUnlock();
                }
                // <- Leaderboard ->
                else
                {

                }

            }

            // <-- Display without effects -->
            else
            {


            }

        }
    }
}
