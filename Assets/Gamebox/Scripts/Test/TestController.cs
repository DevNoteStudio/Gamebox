
namespace DevNote.Gamebox
{
    public class TestController
    {
        private readonly Viewer<TestLevelView> testLevelViewer;
        private readonly LevelController levelController;



        public TestController(LevelController levelController)
        {
            testLevelViewer = new(Configs.Gamebox.TestLevelPrefab);
            this.levelController = levelController;

            if (Configs.Gamebox.TestEnabled)
            {
                levelController.OnLevelStarted += OnLevelStarted;
                levelController.OnLevelExit += OnLevelExit;
            }
        }

        private void OnLevelStarted()
        {
            testLevelViewer.ShowExpand(UI.Container)
                .Display(levelController.CurrentLocationIndex, levelController.CurrentLevelIndex);
        }


        private void OnLevelExit() => testLevelViewer.Hide();

        




    }
}


