using System.Collections.Generic;
using UnityEngine;


namespace DevNote.Gamebox
{
    [CreateAssetMenu(menuName = "Gamebox/Config", fileName = "[Gamebox]")]
    public partial class GameboxConfig : ScriptableObject // Main
    {
        [Header("Gamebox " + Common.VERSION), Space]
        [SerializeField] private ResourcesData _resources;
        [SerializeField] private TestResources _testResources;
        [SerializeField] private List<ItemData> _items;
        [SerializeField] private List<LocationData> _locations;
        [SerializeField] private BaseLevelRewardData _baseLevelReward;
        [SerializeField] private AdsData _ads;


    }
}


