using DevNote;
using UnityEngine;

namespace Gamebox
{
    public class GameboxSceneContext : SceneContext
    {
        [SerializeField] private RectTransform _uiContainer;
        [SerializeField] private RectTransform _fadeContainer;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private ExtendedGraphicRaycaster _graphicRaycaster;

        private readonly Holder<IEnvironment> environment = new();
        private readonly Holder<ILeaderboards> leaderboards = new();
        private readonly Holder<IAds> ads = new();
        private readonly Holder<IReview> review = new();
        private readonly Holder<IPurchase> purchase = new();
        private readonly Holder<IAnalytics> analytics = new();
        private readonly Holder<ISave> save = new();

        public override void RegisterContext()
        {
            new UI(_uiContainer, _fadeContainer, _canvas);
            new TutorialPointer();

            var rollup = Register(new RollupController());
            var league = Register(new LeagueController());
            var menu = Register(new MenuController());
            var level = Register(new LevelController(menu, leaderboards.Item, ads.Item, league, environment.Item, save.Item));
            var test = Register(new TestController(level));
            var popup = Register(new PopupController(level, ads.Item, review.Item, purchase.Item));
            var pause = Register(new PauseController(environment.Item));
            var reward = Register(new RewardController());
            var shop = Register(new ShopController(purchase.Item));
            var boxOpen = Register(new BoxOpenController());
            var cards = Register(new CardsController());
            var boxAndCardTutorial = Register(new BoxAndCardTutorialController(menu, shop, boxOpen, cards, _graphicRaycaster));
            var sound = Register(new SoundController());

            var start = Register(new StartController(menu, level, popup));

            Register(new AnalyticsController(analytics.Item, level));
        }


    }
}



