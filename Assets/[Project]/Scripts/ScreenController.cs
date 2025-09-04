using UnityEngine;


namespace DevNote.Gamebox
{
    public class ScreenController
    {

        private Viewer<LevelCatalogScreenView> _levelCatalogScreenViewer;
        private Viewer<LocationsScreenView> _locationCatalogScreenViewer;



        public ScreenController(RectTransform container)
        {
            _levelCatalogScreenViewer = new (Configs.LevelUp.LevelCatalogScreenPrefab);
            _locationCatalogScreenViewer = new (Configs.LevelUp.LocationCatalogScreenPrefab);
        }


        public void ShowLocationsScreen(int locationIndex)
        {
            _locationCatalogScreenViewer.Show(UI.ScreenContainer).Display(locationIndex);
        }

        public void HideLocationsScreen()
        {
            _locationCatalogScreenViewer.Hide();
        }

        public void ShowLevelsScreen(int locationIndex)
        {
            //_levelCatalogScreenViewer.Show(UI.ScreenContainer).Display(locationIndex);
        }

        public void HideLevelsScreen()
        {
            _levelCatalogScreenViewer.Hide();
        }



    }
}


