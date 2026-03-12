using System.Collections.Generic;
using DevNote;
using UnityEngine;


namespace Gamebox
{
    [CreateAssetMenu(menuName = "Gamebox/Config", fileName = "- Gamebox -")]
    public partial class GameboxConfig : LoadableFromTable // Main
    {
        [field: Header("Gamebox " + Info.VERSION), Space]
        [field: SerializeField] public bool TestEnabled { get; private set; }

        [field: SerializeField] public ContentPipeline ContentPipeline { get; private set; }
        [SerializeField] private List<BoosterData> _boosterData;


        private List<ItemKey> _tutorialItems;
        private List<LocationData> _locations;
        private RewardsData _rewards;
        private List<LeagueData> _leagues;
        

        

        [field: SerializeField] public float DelayBeforeShowWinScreen { get; private set; }
        [field: SerializeField] public int LevelRewardCoins { get; private set; }
        [field: SerializeField] public bool ReviveAvailable { get; private set; }

        public int ReviveGemPrice { get; private set; }
        public int InterstitialsShowsToShowNoAdsWindow { get; private set; }
        private int _interstitialsFromLevel;
        private LeagueType _menuFromLeague;
        public ItemPack GameRateReward { get; private set; }
        private List<int> _rateUsLevels;

        private List<int> _cardCellGemPrices;
        private List<CardPrice> _cardUpgradePrices;
        private List<CardData> _cardDataList;
        private List<ShopBoxData> _shopBoxes;
        private List<int> _gemsInsideShopPacks;
        private List<int> _coinsInsideShopPacks;
        private List<int> _coinsPackPrices;
        

        public bool CanShowInterstitial => IGameState.Levels.CurrentLevel >= _interstitialsFromLevel; 
        public bool MenuAvailable => IConfigs.Gamebox.GetLeagueType(IGameState.Rating.Value) >= _menuFromLeague;


        public bool RateUsNow
        {
            get
            {
                if (IGameState.GameRated.Value) return false;

                foreach (int rateLevel in _rateUsLevels)
                    if (IGameState.Levels.CurrentLevel == rateLevel) return true;

                return false;
            }
        }


    }
}


