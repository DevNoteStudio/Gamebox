using System;
using UnityEngine;

namespace DevNote.Gamebox
{

    public partial class LevelUpConfig // Locations
    {
        [Serializable] private struct LocationData
        {
            public string nameLocalizationKey;
            public Sprite previewSprite;
            public int levels;
            public int starRequire;
        }


        public int LocationsAmount => _locations.Count;

        public string GetLocationName(int locationIndex) 
            => Localization.GetLocalizedText(_locations[locationIndex].nameLocalizationKey);

        public int GetLevelsAmount(int locationIndex) => _locations[locationIndex].levels;

        public int GetLocationStarRequire(int locationIndex) => _locations[locationIndex].starRequire;

        public Sprite GetLocationPreviewSprite(int locationIndex) => _locations[locationIndex].previewSprite;

        


    }



    
}