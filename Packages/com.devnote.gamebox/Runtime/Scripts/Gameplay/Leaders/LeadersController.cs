using DevNote;

namespace Gamebox
{
    public class LeadersController
    {
        private readonly Viewer<LeadersWindowView> leaderboardWindowViewer;


        public LeadersController()
        {
            leaderboardWindowViewer = new(IConfigs.GetViewPrefab<LeadersWindowView>());
        }


        public void ShowLeadersWindow()
        {
            leaderboardWindowViewer.ShowFaded(UI.Container).Display().AnimateShow();
        }


        public void HideLeadersWindow()
        {
            leaderboardWindowViewer.AnimateFadedHide(leaderboardWindowViewer.View.AnimateHide);
        }



    }
}
