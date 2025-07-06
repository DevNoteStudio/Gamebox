using UnityEngine;


namespace DevNote.Modules.Levels
{
    public class ScreenController
    {

        private Viewer<LevelsScreenView> _levelsScreenViewer;
        private Viewer<LocationsScreenView> _locationsScreenViewer;



        public ScreenController(RectTransform container)
        {
            _levelsScreenViewer = new(Configs.Levels.LevelsScreenPrefab, container, ViewerMode.EnableDisable);
            _locationsScreenViewer = new(Configs.Levels.LocationsScreenPrefab, container, ViewerMode.EnableDisable);
        }


        public void ShowLocationsScreen(int locationIndex)
        {
            _locationsScreenViewer.Show().Display(locationIndex);
        }

        public void HideLocationsScreen()
        {
            _locationsScreenViewer.Hide();
        }

        public void ShowLevelsScreen(int locationIndex)
        {
            _levelsScreenViewer.Show().Display(locationIndex);
        }

        public void HideLevelsScreen()
        {
            _levelsScreenViewer.Hide();
        }



    }
}


