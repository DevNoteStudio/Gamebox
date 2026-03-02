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


        [SerializeField] private List<ItemKey> _tutorialItems;
        [SerializeField] private List<LocationData> _locations;
        [SerializeField] private RewardsData _rewards;
        [SerializeField] private List<LeagueData> _leagues;
        [SerializeField] private List<BoosterData> _boosterData;

        [field: SerializeField] public ContentPipeline ContentPipeline { get; private set; }

        [field: SerializeField] public float DelayBeforeShowWinScreen { get; private set; }
        [field: SerializeField] public int VictoryRouletteFromLevel { get; private set; }
        [field: SerializeField] public int ReviveFromLevel { get; private set; }
        [field: SerializeField] public int ReviveGemPrice { get; private set; }
        [field: SerializeField] public int InterstitialsShowsToShowNoAdsWindow { get; private set; }
        [SerializeField] private int _interstitialsFromLevel;
        [SerializeField] private LeagueType _menuFromLeague;
        [field: SerializeField] public ItemPack GameRateReward { get; private set; }
        [SerializeField] private List<int> _rateUsLevels;

        [SerializeField] private List<int> _cardCellGemPrices;
        [SerializeField] private List<CardPrice> _cardUpgradePrices;
        [SerializeField] private List<CardData> _cardDataList;
        [SerializeField] private List<ShopBoxData> _shopBoxes;
        [SerializeField] private List<int> _gemsInsideShopPacks;
        [SerializeField] private List<int> _coinsInsideShopPacks;
        [SerializeField] private List<int> _coinsPackPrices;
        

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


