using System;
using DevNote;

namespace Gamebox
{
    public class LeagueController
    {
        private Action _onLeagueLevelUpScreenHided;

        private readonly Viewer<LeagueLevelUpScreenView> leagueLevelUpScreenViewer;



        public LeagueController()
        {
            leagueLevelUpScreenViewer = new(IConfigs.GetViewPrefab<LeagueLevelUpScreenView>());
        }


        public void ShowLeagueLevelUpScreen(LeagueType nextLeague, Action onScreenHided = null)
        {
            _onLeagueLevelUpScreenHided = onScreenHided;
            leagueLevelUpScreenViewer.Show(UI.Container).AnimateDisplay(nextLeague);
        }

        public void HideLeagueLevelUpScreen()
        {
            leagueLevelUpScreenViewer.Hide();
            _onLeagueLevelUpScreenHided?.Invoke();
        }



    }
}
