using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gamebox
{
    public class EffectController
    {

        private GameplayScreenView GameplayScreen => levelController.GameplayScreen;

        private readonly LevelController levelController;


        public EffectController(LevelController levelController, UnlockController unlockController)
        {
            this.levelController = levelController;

            unlockController.OnUnlockWindowStartHandling += OnUnlockWindowStartHandling;
        }

        private async void OnUnlockWindowStartHandling(bool unlockWindowWasShown)
        {
            bool useEffects = !unlockWindowWasShown && !levelController.IsFirstLevelInGameSession 
                && levelController.IsLevelStartedFirstTime;

            if (useEffects)
            {
                GameplayScreen.CurrencyView.AnimateDisplayWithRollup(particles: 8, Vector2.zero);

                await UniTask.WaitForSeconds(1.3f);

                if (IConfigs.Gamebox.IsAvailable(UnlockKey.Leaderboard))
                    GameplayScreen.LeadersButton.AnimateDisplay();
            }
            else
            {
                GameplayScreen.LeadersButton.Display();
                GameplayScreen.CurrencyView.Display();
            }
        }
    }
}
