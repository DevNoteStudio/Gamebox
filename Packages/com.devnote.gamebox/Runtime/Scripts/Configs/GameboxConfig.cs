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

        [ SerializeField] private int _interstitialsFromLevel;


        public bool CanShowInterstitial(int currentLevel) => currentLevel >= _interstitialsFromLevel; 




    }
}


