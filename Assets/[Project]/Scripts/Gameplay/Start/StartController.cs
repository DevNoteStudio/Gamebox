
namespace DevNote.Gamebox
{
    public class StartController : IStartHandler
    {

        private readonly LevelController levelController;

        public StartController(LevelController levelController)
        {
            this.levelController = levelController;
        }


        void IStartHandler.Start()
        {
            levelController.ShowLocationsScreen(1);
        }


    }
}


