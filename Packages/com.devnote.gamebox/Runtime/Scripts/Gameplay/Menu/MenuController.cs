using DevNote;

namespace Gamebox
{
    public class MenuController
    {

        private readonly Viewer<LocationsScreenView> locationsScreenViewer;
        private readonly Viewer<LevelsScreenView> levelsScreenViewer;



        public MenuController()
        {
            locationsScreenViewer = new(IConfigs.GetViewPrefab<LocationsScreenView>());
            levelsScreenViewer = new(IConfigs.GetViewPrefab<LevelsScreenView>());
        }




        public void ShowLocationsScreen(int locationIndex)
        {
            locationsScreenViewer.ShowExpand(UI.Container).Display(locationIndex);
        }

        public void HideLocationsScreen()
        {
            locationsScreenViewer.Hide();
        }



        public void ShowLevelsScreen(int locationIndex)
        {
            levelsScreenViewer.ShowExpand(UI.Container).Display(locationIndex);
        }

        public void HideLevelsScreen()
        {
            levelsScreenViewer.Hide();
        }




    }
}


