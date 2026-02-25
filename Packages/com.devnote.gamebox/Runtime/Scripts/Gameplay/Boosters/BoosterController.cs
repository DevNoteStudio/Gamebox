using System;

namespace Gamebox
{

    public class BoosterController
    {
        public delegate void OnBoosterUsingFinish(bool success);

        public event Action OnBoosterUsingStarted;
        public event OnBoosterUsingFinish OnBoosterUsingFinished;

        public bool IsUsingBooster { get; private set; }
        public ItemKey CurrentUsingBoosterKey { get; private set; }


        public BoosterController(PauseController pauseController)
        {
            pauseController.OnGamePaused += OnGamePaused;
        }

        private void OnGamePaused() => CancelBoosterUsing();

        public void StartBoosterUsing(ItemKey boosterItemKey)
        {
            CurrentUsingBoosterKey = boosterItemKey;
            IsUsingBooster = true;
            OnBoosterUsingStarted?.Invoke();
        }

        public void FinishBoosterUsing(bool success)
        {
            IsUsingBooster = false;

            if (success) IGameState.Items.Spend(CurrentUsingBoosterKey, 1);

            OnBoosterUsingFinished?.Invoke(success);
        }

        public void CancelBoosterUsing()
        {
            if (!IsUsingBooster) return;

            IsUsingBooster = false;
            OnBoosterUsingFinished?.Invoke(success: false);
        }

        public void ShowBoosterHint()
        {

        }


    }
}
