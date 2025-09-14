
namespace DevNote.Gamebox
{
    public class StartController : IStartHandler
    {

        private readonly MenuController menuController;

        public StartController(MenuController menuController)
        {
            this.menuController = menuController;
        }


        void IStartHandler.Start()
        {
            menuController.ShowLocationsScreen(1);
        }


    }
}


