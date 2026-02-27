using DevNote;
using UnityEngine;

namespace Gamebox
{
    public class TestController : IUpdateHandler
    {
        private readonly Viewer<TestLevelView> testLevelViewer;
        private readonly LevelController levelController;



        public TestController(LevelController levelController)
        {
            testLevelViewer = new(IConfigs.GetViewPrefab<TestLevelView>());
            this.levelController = levelController;

            if (IConfigs.Gamebox.TestEnabled)
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

        void IUpdateHandler.Update()
        {
            if (!IConfigs.Gamebox.TestEnabled) return;
        }
    }
}


