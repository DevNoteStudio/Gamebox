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
        [SerializeField] private RewardsData _rewards;

        [field: SerializeField] public int LocationSelectionFromLevel { get; private set; }
        [field: SerializeField] public int VictoryRouletteFromLevel { get; private set; }
        [field: SerializeField] public int ReviveFromLevel { get; private set; }

    }
}


