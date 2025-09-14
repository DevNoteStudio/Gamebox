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

            var menu = Register(new MenuController());
            var level = Register(new LevelController(menu));
            

            var test = Register(new TestController(level));
            var start = Register(new StartController(menu));


            



        }
    }

}

