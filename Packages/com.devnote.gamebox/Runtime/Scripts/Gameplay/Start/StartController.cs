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
            int locationIndex = IGameState.LastPlayLocationIndex.Value;

            if (IConfigs.Gamebox.MenuAvailable)
                menuController.ShowLocationsScreen(locationIndex);

            else
            {
                int levelIndex = IGameState.Levels.GetLastLevelIndexForPlay(locationIndex);
                levelController.StartLevel(locationIndex, levelIndex);
            }

            
        }



    }
}


