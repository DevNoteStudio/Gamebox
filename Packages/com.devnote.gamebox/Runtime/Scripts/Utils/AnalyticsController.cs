using DevNote;

namespace Gamebox
{
    public class AnalyticsController
    {
        private readonly LevelController levelController;
        private readonly IAnalytics analytics;


        private string CurrentLevelData => 
            $"{IGameState.LastPlayLevelIndex.Value + 1}_" +
            $"{IGameState.LastPlayLocationIndex.Value + 1}_" +
            $"{IGameState.Levels.CurrentLevel}";


        public AnalyticsController(IAnalytics analytics, LevelController levelController)
        {
            this.analytics = analytics;
            this.levelController = levelController;

            levelController.OnLevelCompleted += OnLevelCompleted;
            IPurchase.OnPurchaseHandled += OnPurchaseHandled;

            // IAds.OnInterstitialShown += OnInterstitialShown;
            // IAds.OnRewardedShown += OnRewardedShown;
            // IReview.OnGameRated += OnGameRated;
            // 
            // levelController.OnLevelLost += OnLevelLost;
            // levelController.OnRevive += OnRevive;
        }


        public void TutorialStepCompleted(int stepIndex, string stepName)
        {
            analytics.SendEvent("tutorial_step", new()
            {
                { "step", stepIndex + 1 },
                { "name", stepName },
            });
        }


        private void OnLevelCompleted()
        {
            analytics.SendEvent("level_completed", new()
            {
                { "level", levelController.CompletedLevel },
            });
        }

        private void OnPurchaseHandled(ProductKey productKey, bool success)
        {
            if (!success) return;

            analytics.SendEvent("iap_purchase", new()
            {
                { "product", productKey.ToString() },
                { "location", IGameState.LastPlayLocationIndex.Value + 1 },
                { "level", IGameState.LastPlayLevelIndex.Value + 1 },
            });
        }


        /*
        private void OnRevive()
        {
            analytics.SendEvent("level_revive", new()
            {
                { "level", CurrentLevelData },
            });
        }

        private void OnLevelLost()
        {
            analytics.SendEvent("level_lose", new()
            {
                { "level", CurrentLevelData },
            });
        }

        

        private void OnGameRated()
        {
            analytics.SendEvent("game_rated", new()
            {
                { "level", CurrentLevelData },
            });
        }

        private void OnRewardedShown(AdKey key, AdShowStatus status)
        {
            if (status != AdShowStatus.Success) return;

            analytics.SendEvent("ad_shown", new()
            {
                { "type", "reward" },
                { "key", key.ToString() },
                { "level", CurrentLevelData },
            });
        }

        private void OnInterstitialShown(AdKey key, AdShowStatus status)
        {
            if (status != AdShowStatus.Success) return;

            analytics.SendEvent("ad_shown", new()
            {
                { "type", "inter" },
                { "key", key.ToString() },
                { "level", CurrentLevelData },
            });
        }

        public void ItemUsed(ItemKey itemKey)
        {
            analytics.SendEvent("item_used", new()
            {
                { "type", itemKey.ToString() },
                { "level", CurrentLevelData },
            });
        }

        public void ItemPurchased(ItemKey itemKey, bool isAdBonus)
        {
            analytics.SendEvent("item_buy", new()
            {
                { "type", itemKey.ToString() },
                { "currency", isAdBonus ? "ad" : "coins" },
                { "level", CurrentLevelData }
            });
        }
        */


    }
}

