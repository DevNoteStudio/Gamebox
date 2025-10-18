using DevNote;

namespace Gamebox
{
    public class PauseController
    {
        private readonly Viewer<PauseWindowView> pauseWindowViewer;


        public PauseController()
        {
            pauseWindowViewer = new(IConfigs.GetViewPrefab<PauseWindowView>());
        }


        public void ShowPauseWindow() 
            => pauseWindowViewer.ShowWindow(UI.Container).Display().AnimateShow();


        public void HidePauseWindow(bool force = false)
        {
            if (force) pauseWindowViewer.ForceHideWindow();
            else pauseWindowViewer.AnimateHideWindow(pauseWindowViewer.View.AnimateHide);
        }




    }
}


