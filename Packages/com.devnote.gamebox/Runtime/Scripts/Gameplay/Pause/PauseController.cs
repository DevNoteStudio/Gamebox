using System;
using DevNote;

namespace Gamebox
{
    public class PauseController
    {
        public event Action OnGamePaused;

        private readonly Viewer<PauseWindowView> pauseWindowViewer;
        private readonly IEnvironment environment;


        public PauseController(IEnvironment environment)
        {
            pauseWindowViewer = new(IConfigs.GetViewPrefab<PauseWindowView>());
            this.environment = environment;
        }


        public void ShowPauseWindow()
        {
            environment.StopGameplay();
            pauseWindowViewer.ShowFaded(UI.Container).Display().AnimateShow();
            OnGamePaused?.Invoke();
        }


        public void HidePauseWindow(bool force = false)
        {
            if (force) pauseWindowViewer.ForceFadedHide();
            else pauseWindowViewer.AnimateFadedHide(pauseWindowViewer.View.AnimateHide);
        }




    }
}


