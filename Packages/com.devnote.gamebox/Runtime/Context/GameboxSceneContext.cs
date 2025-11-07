using DevNote;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class GameboxSceneContext : SceneContext
    {
        [SerializeField] private RectTransform _uiContainer;
        [SerializeField] private RectTransform _fadeContainer;


        private readonly Holder<ILeaderboards> leaderboards = new();
        private readonly Holder<IAds> ads = new();
        private readonly Holder<IReview> review = new();
        private readonly Holder<IPurchase> purchase = new();
        private readonly Holder<IAnalytics> analytics = new();

        public override void RegisterContext()
        {
            new UI(_uiContainer, _fadeContainer);

            var coinsRollup = Register(new CoinsRollupController());
            var league = Register(new LeagueController());
            var menu = Register(new MenuController());
            var level = Register(new LevelController(menu, leaderboards.Item, ads.Item, league));
            var test = Register(new TestController(level));
            var popup = Register(new PopupController(level, ads.Item, review.Item, purchase.Item));
            var pause = Register(new PauseController());
            var reward = Register(new RewardController());

            var start = Register(new StartController(menu, level));

            Register(new AnalyticsController(analytics.Item, level));
        }


    }
}



