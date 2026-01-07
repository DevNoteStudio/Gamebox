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


        [SerializeField] private List<ItemData> _items;
        [SerializeField] private List<LocationData> _locations;
        [SerializeField] private RewardsData _rewards;
        [SerializeField] private List<LeagueData> _leagues;
        [field: SerializeField] public int VictoryRouletteFromLevel { get; private set; }
        [field: SerializeField] public int ReviveFromLevel { get; private set; }
        [field: SerializeField] public int InterstitialsShowsToShowNoAdsWindow { get; private set; }
        [SerializeField] private int _interstitialsFromLevel;
        [field: SerializeField] public ItemPack GameRateReward { get; private set; }
        [SerializeField] private List<int> _rateUsLevels;

        [SerializeField] private List<int> _cardCellGemPrices;
        [SerializeField] private List<CardData> _cardDataList;
        [SerializeField] private List<ShopBoxData> _shopBoxes;
        [SerializeField] private List<int> _gemsInsideShopPacks;
        [SerializeField] private List<int> _coinsInsideShopPacks;
        [SerializeField] private List<int> _coinsPackPrices;


        public bool CanShowInterstitial => IGameState.Levels.CurrentLevel >= _interstitialsFromLevel; 

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


