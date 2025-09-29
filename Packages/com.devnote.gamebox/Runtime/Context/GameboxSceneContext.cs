using DevNote;
using UnityEngine;

namespace Gamebox
{
    public class GameboxSceneContext : SceneContext
    {
        [SerializeField] private RectTransform _uiContainer;
        [SerializeField] private RectTransform _fadeContainer;


        private readonly Holder<ILeaderboards> leaderboards = new();
        private readonly Holder<IAds> ads = new();

        public override void RegisterContext()
        {
            new UI(_uiContainer, _fadeContainer);

            var menu = Register(new MenuController());
            var level = Register(new LevelController(menu, leaderboards.Item));
            var test = Register(new TestController(level));
            var start = Register(new StartController(menu, level));
            var ads2 = Register(new AdsController(level, ads.Item));

        }


    }
}



