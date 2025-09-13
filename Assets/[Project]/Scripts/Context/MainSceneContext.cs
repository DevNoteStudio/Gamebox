using UnityEngine;

namespace DevNote.Gamebox
{
    public class MainSceneContext : SceneContext
    {
        [SerializeField] private RectTransform _uiContainer;
        [SerializeField] private RectTransform _fadeContainer;

        public override void RegisterContext()
        {
            new UI(_uiContainer, _fadeContainer);

            var level = Register(new LevelController());


            var start = Register(new StartController(level));


            //var test = Register(new TestController(_victoryScreen, _loseWindow));



        }
    }

}

