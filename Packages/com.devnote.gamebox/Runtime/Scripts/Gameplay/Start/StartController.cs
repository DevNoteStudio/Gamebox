using DevNote;

namespace Gamebox
{
    public class StartController : IStartHandler
    {
        private readonly MenuController menuController;
        private readonly LevelController levelController;

        public StartController(MenuController menuController, LevelController levelController)
        {
            this.menuController = menuController;
            this.levelController = levelController;
        }


        void IStartHandler.Start()
        {
            int locationIndex = IGameState.LastPlayLocationIndex.Value;

            if (IGameState.Items.Has(ItemKey.LocationsUnlocked))
                menuController.ShowLocationsScreen(locationIndex);

            else
            {
                int levelIndex = IGameState.Levels.GetLastLevelIndexForPlay(locationIndex);
                levelController.StartLevel(locationIndex, levelIndex);
            }

            
        }


    }
}


