using Cysharp.Threading.Tasks;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    public class GameboxSceneContext : SceneContext
    {
        [SerializeField] private Camera _mainCamera; public static Camera MainCamera { get; private set; }

        private readonly Holder<IEnvironment> environment = new();
        private readonly Holder<ILeaderboards> leaderboards = new();
        private readonly Holder<IAds> ads = new();
        private readonly Holder<IReview> review = new();
        private readonly Holder<IPurchase> purchase = new();
        private readonly Holder<IAnalytics> analytics = new();
        private readonly Holder<ISave> save = new();
        private readonly Holder<IRemote> remote = new();



        public override async void RegisterContext()
        {
            SetDefaultHandlers();

            MainCamera = _mainCamera;

            var gamebox = CreateGameboxRoot();

            new UI(gamebox.ScreenContainer, gamebox.FadeContainer, gamebox.Canvas);
            new TutorialPointer();

            await UniTask.WaitUntil(() => save.Item.Initialized && remote.Item.Initialized);

            IAds.InterstitialCooldown = remote.Item.GetFloat(RemoteKey.InterstitialCooldown);


            var rollup = Register(new RollupController());
            var league = Register(new LeagueController());
            var menu = Register(new MenuController());
            var level = Register(new LevelController(menu, leaderboards.Item, ads.Item, league, environment.Item, save.Item, review.Item));
            var currency = Register(new CurrencyController(level));
            var test = Register(new TestController(level));
            var popup = Register(new PopupController(level, ads.Item, review.Item, purchase.Item));
            var pause = Register(new PauseController(environment.Item));
            var reward = Register(new RewardController());
            var shop = Register(new ShopController(purchase.Item));
            var boxOpen = Register(new BoxOpenController());
            var cards = Register(new CardsController());
            var boxAndCardTutorial = Register(new BoxAndCardTutorialController(menu, shop, boxOpen, cards, gamebox.GraphicRaycaster));
            var sound = Register(new SoundController());
            var score = Register(new ScoreController(level));
            var booster = Register(new BoosterController(pause, level));
            var leaders = Register(new LeadersController());
            var unlock = Register(new UnlockController(level));
            var effect = Register(new EffectController(level, unlock));
            var adWarning = Register(new AdWarningController(level));

            var start = Register(new StartController(menu, level, popup));

            Register(new AnalyticsController(analytics.Item, level));

            Initialized = true;
        }


        private GameboxRoot CreateGameboxRoot()
        {
            var gamebox = Instantiate(IConfigs.Internal.GameboxRootPrefab);
            gamebox.name = "G Gamebox";
            gamebox.transform.SetAsLastSibling();
            gamebox.ConnectCamera(_mainCamera);

            return gamebox;
        }


        private void SetDefaultHandlers()
        {
            IRemote.SetDefaultHandler(RemoteKey.AdWarningDuration, () => "5");
            IRemote.SetDefaultHandler(RemoteKey.InterstitialCooldown, () => "60");
        }


    }
}



