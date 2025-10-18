using System.Collections.Generic;
using UnityEngine;


namespace Gamebox
{
    [CreateAssetMenu(menuName = "Gamebox/Config", fileName = "- Gamebox -")]
    public partial class GameboxConfig : ScriptableObject // Main
    {
        [field: Header("Gamebox " + Info.VERSION), Space]
        [field: SerializeField] public bool TestEnabled { get; private set; }


        [SerializeField] private List<ItemData> _items;
        [SerializeField] private List<LocationData> _locations;
        [SerializeField] private RewardsData _rewards;
        [field: SerializeField] public int VictoryRouletteFromLevel { get; private set; }
        [field: SerializeField] public int ReviveFromLevel { get; private set; }
        [field: SerializeField] public int InterstitialsShowsToShowNoAdsWindow { get; private set; }

        [SerializeField] private int _interstitialsFromLevel;
        [SerializeField] private List<int> _rateUsLevels;


        public bool CanShowInterstitial => IGameState.Levels.CurrentLevel >= _interstitialsFromLevel; 

        public bool RateUsNow
        {
            get
            {
                foreach (int rateLevel in _rateUsLevels)
                    if (IGameState.Levels.CurrentLevel == rateLevel) return true;

                return false;
            }
        }


    }
}


