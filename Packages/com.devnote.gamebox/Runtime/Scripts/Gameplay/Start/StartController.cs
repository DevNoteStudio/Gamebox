using DevNote;

namespace Gamebox
{
    public class StartController : IStartHandler
    {
        private readonly MenuController menuController;
        private readonly LevelController levelController;
        private readonly PopupController popupController;

        private const int CURRENT_SAVE_VERSION = 1;


        public StartController(MenuController menuController, LevelController levelController, 
            PopupController popupController)
        {
            this.menuController = menuController;
            this.levelController = levelController;
            this.popupController = popupController;
        }


        void IStartHandler.Start()
        {
            levelController.StartLevel(IGameState.Level, isFirstLevelInGameSession: true);
        }




    }
}


