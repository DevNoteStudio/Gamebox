using System;
using System.Collections.Generic;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    [CreateAssetMenu(menuName = "Gamebox/InternalConfig", fileName = "Internal")]
    public class InternalConfig : ScriptableObject
    {
        [Serializable]
        private struct RarityColor
        {
            public RarityType rarityType;
            public Color backgroundColor;
            public Color textColor;
        }

        [field: SerializeField] public GameboxRoot GameboxRootPrefab { get; private set; }
        [SerializeField] private List<RarityColor> _rarityColors;


        public Color GetRarityBackgroundColor(RarityType rarityType)
            => _rarityColors.FindOrException(data => data.rarityType == rarityType).backgroundColor;

        public Color GetRarityTextColor(RarityType rarityType)
            => _rarityColors.FindOrException(data => data.rarityType == rarityType).textColor;




    }
}
