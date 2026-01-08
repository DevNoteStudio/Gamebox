using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamebox
{
    [CreateAssetMenu(menuName = "Gamebox/InternalConfig", fileName = "Internal")]
    public class InternalConfig : ScriptableObject
    {
        [field: SerializeField] public Color BackgroundBoosterCardColor { get; private set; }
        [field: SerializeField] public Color TextBoosterCardColor { get; private set; }


        [Serializable] private struct RarityColor
        {
            public RarityType rarityType;
            public Color backgroundColor;
            public Color textColor;
        }


        [SerializeField] private List<RarityColor> _rarityColors;


        public Color GetRarityBackgroundColor(RarityType rarityType)
            => _rarityColors.FindOrException(data => data.rarityType == rarityType).backgroundColor;

        public Color GetRarityTextColor(RarityType rarityType)
            => _rarityColors.FindOrException(data => data.rarityType == rarityType).textColor;




    }
}
