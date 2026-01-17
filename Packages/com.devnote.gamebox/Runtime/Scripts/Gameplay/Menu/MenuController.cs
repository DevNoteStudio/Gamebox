using DevNote;
using UnityEngine;

namespace Gamebox
{
    public class MenuController
    {
        private readonly Viewer<TabsView> tabsViewer; public TabsView Tabs => tabsViewer.View;
        private readonly Viewer<LocationsScreenView> locationsScreenViewer; 
        public LocationsScreenView LocationsScreen => locationsScreenViewer.View;
        private readonly Viewer<LevelsScreenView> levelsScreenViewer;
        private readonly Viewer<ItemTutorialWindowView> itemTutorialWindowViewer;

        private int _locationIndex = 0;

        public MenuController(TabsView tabs)
        {
            tabsViewer = new(IConfigs.GetViewPrefab<TabsView>());
            locationsScreenViewer = new(IConfigs.GetViewPrefab<LocationsScreenView>());
            levelsScreenViewer = new(IConfigs.GetViewPrefab<LevelsScreenView>());
            itemTutorialWindowViewer = new(IConfigs.GetViewPrefab<ItemTutorialWindowView>());
        }


        public void ShowLocationsScreen(int locationIndex = -1)
        {
            var view = locationsScreenViewer.ShowExpand(UI.Container);
            if (locationIndex != -1)
            {
                view.Display(locationIndex);
                _locationIndex = locationIndex;
            }
        }

        public void HideLocationsScreen() => locationsScreenViewer.Hide();

        public void ShowLevelsScreen(int locationIndex)
            => levelsScreenViewer.ShowExpand(UI.Container).Display(locationIndex);


        public void HideLevelsScreen() => levelsScreenViewer.Hide();

        
        public void SetTabsActive(bool active, TabType selectedTab = TabType.Locations, RectTransform screenRect = null)
        {
            if (active)
            {
                tabsViewer.ShowExpand(screenRect);
                tabsViewer.View.AnimateSelectTab(selectedTab);
            }
            else tabsViewer.Hide();
        }
        


    }
}


