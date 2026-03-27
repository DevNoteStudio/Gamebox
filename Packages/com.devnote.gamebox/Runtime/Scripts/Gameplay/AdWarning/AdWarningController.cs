using DevNote;

namespace Gamebox
{
    public class AdWarningController
    {

        public readonly Viewer<AdWarningWindowView> adWarningWindowViewer;



        public AdWarningController()
        {
            adWarningWindowViewer = new(IConfigs.GetViewPrefab<AdWarningWindowView>());
        }

        public void ShowAdWarningWindow()
            => adWarningWindowViewer.ShowFaded(UI.Container).Display().AnimateShow();


        public void HideAdWarningWindow(bool showInterstitial)
            => adWarningWindowViewer.AnimateFadedHide(adWarningWindowViewer.View.AnimateHide);




    }
}
