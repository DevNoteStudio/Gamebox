using DevNote;


namespace Gamebox
{
    public class UnlockController
    {
        public delegate void OnUnlockWindowHandle(bool wasShown);
        public event OnUnlockWindowHandle OnUnlockWindowStartHandling;

        private readonly Viewer<UnlockWindowView> unlockWindowViewer;
        private readonly LevelController levelController;
        
        
        public UnlockController(LevelController levelController)
        {
            unlockWindowViewer = new(IConfigs.GetViewPrefab<UnlockWindowView>());

            this.levelController = levelController;
            levelController.OnLevelStarted += OnLevelStarted;
        }

        private void OnLevelStarted() => TryShowUnlockWindow(IGameState.Level);

        private void TryShowUnlockWindow(int level)
        {
            bool showAvailable = levelController.IsLevelStartedFirstTime;

            bool hasUnlock = IConfigs.Gamebox.TryGetLevelUnlockKey
                (level, out var previousKey, out var currentKey, out var nextKey);

            if (showAvailable && hasUnlock)
            {
                UI.AddFadePoint();
                unlockWindowViewer.ShowExpand(UI.Container).AnimateDisplay(previousKey, currentKey, nextKey);
                OnUnlockWindowStartHandling?.Invoke(wasShown: true);
            }
            else
            {
                OnUnlockWindowStartHandling?.Invoke(wasShown: false);
            }
        }


        public void HideUnlockWindow()
        {
            if (unlockWindowViewer.ViewExists)
            {
                UI.RemoveFadePoint();
                unlockWindowViewer.View.AnimateHide(onCompleted: unlockWindowViewer.Hide);
            }

            
        }




    }
}
