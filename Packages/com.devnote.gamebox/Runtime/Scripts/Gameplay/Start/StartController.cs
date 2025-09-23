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
            bool showLocationSelection = IGameState.Levels.CompletedLevels >= IConfigs.Gamebox.LocationSelectionFromLevel - 1;

            if (showLocationSelection)
                menuController.ShowLocationsScreen(locationIndex);

            else
            {
                int levelIndex = IGameState.Levels.GetLastLevelIndexForPlay(locationIndex);
                levelController.StartLevel(locationIndex, levelIndex);
             }

            
        }


    }
}


