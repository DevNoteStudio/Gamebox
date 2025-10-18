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
        private readonly Holder<IReview> review = new();

        public override void RegisterContext()
        {
            new UI(_uiContainer, _fadeContainer);

            var menu = Register(new MenuController());
            var level = Register(new LevelController(menu, leaderboards.Item, ads.Item));
            var test = Register(new TestController(level));
            var popup = Register(new PopupController(level, ads.Item, review.Item));
            var pause = Register(new PauseController());

            var start = Register(new StartController(menu, level));
        }


    }
}



