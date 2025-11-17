using DevNote;

namespace Gamebox
{
    public class MenuController
    {

        private readonly Viewer<LocationsScreenView> locationsScreenViewer;
        private readonly Viewer<LevelsScreenView> levelsScreenViewer;
        private readonly Viewer<ItemTutorialWindowView> itemTutorialWindowViewer;



        public MenuController()
        {
            locationsScreenViewer = new(IConfigs.GetViewPrefab<LocationsScreenView>());
            levelsScreenViewer = new(IConfigs.GetViewPrefab<LevelsScreenView>());
            itemTutorialWindowViewer = new(IConfigs.GetViewPrefab<ItemTutorialWindowView>());
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


