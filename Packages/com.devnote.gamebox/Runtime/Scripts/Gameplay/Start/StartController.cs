using DevNote;

namespace Gamebox
{
    public class StartController : IStartHandler
    {
        private readonly MenuController menuController;
        private readonly LevelController levelController;
        private readonly PopupController popupController;

        public StartController(MenuController menuController, LevelController levelController, 
            PopupController popupController)
        {
            this.menuController = menuController;
            this.levelController = levelController;
            this.popupController = popupController;
        }


        void IStartHandler.Start()
        {
            ConvertStarsToRating();

            int locationIndex = IGameState.LastPlayLocationIndex.Value;

            if (IConfigs.Gamebox.ItemIsAvailable(ItemKey.LocationsUnlocked))
            {
                menuController.ShowLocationsScreen(locationIndex);
                //menuController.SetTabsActive(true, TabType.Locations);
                if (IConfigs.Gamebox.LocationTutorialIsAvailable)
                    popupController.ShowLocationsTutorialWindow();
            }
            else
            {
                // menuController.SetTabsActive(false);
                int levelIndex = IGameState.Levels.GetLastLevelIndexForPlay(locationIndex);
                levelController.StartLevel(locationIndex, levelIndex);
            }

            
        }


        private void ConvertStarsToRating()
        {
            int stars = IGameState.Items.Get(ItemKey.Stars);
            IGameState.Items.Set(ItemKey.Stars, 0);

            IGameState.Rating.Value += stars * 50;
        }


    }
}


