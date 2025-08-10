using UnityEngine;


namespace DevNote.LevelUp
{
    public class ScreenController
    {

        private Viewer<LevelCatalogScreenView> _levelCatalogScreenViewer;
        private Viewer<LocationCatalogScreenView> _locationCatalogScreenViewer;



        public ScreenController(RectTransform container)
        {
            _levelCatalogScreenViewer = new (Configs.LevelUp.LevelCatalogScreenPrefab, ViewerMode.EnableDisable);
            _locationCatalogScreenViewer = new (Configs.LevelUp.LocationCatalogScreenPrefab, ViewerMode.EnableDisable);
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
            _levelCatalogScreenViewer.Show(UI.ScreenContainer).Display(locationIndex);
        }

        public void HideLevelsScreen()
        {
            _levelCatalogScreenViewer.Hide();
        }



    }
}


