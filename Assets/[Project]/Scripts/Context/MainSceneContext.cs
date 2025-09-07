using UnityEngine;

namespace DevNote.Gamebox
{
    public class MainSceneContext : SceneContext
    {
        [SerializeField] private VictoryScreenView _victoryScreen;
        [SerializeField] private LoseWindowView _loseWindow;


        public override void RegisterContext()
        {
            var level = Register(new LevelController());


            var test = Register(new TestController(_victoryScreen, _loseWindow));



        }
    }

}

