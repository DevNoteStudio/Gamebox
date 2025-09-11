using System;
using UnityEngine;

namespace DevNote.Gamebox
{
    public partial class GameboxConfig // Levels
    {
        [Serializable] private struct LocationData
        {
            public Sprite previewSprite;
            public int levels;
            public int starRequire;
        }


        public int LocationsAmount => _locations.Count;

        public string GetLocationName(int locationIndex)
            => Localization.GetLocalizedText($"location_name_{locationIndex}");

        public int GetLocationLevelsAmount(int locationIndex) => _locations[locationIndex].levels;

        public int GetLocationStarRequire(int locationIndex) => _locations[locationIndex].starRequire;

        public Sprite GetLocationPreviewSprite(int locationIndex) => _locations[locationIndex].previewSprite;


    }
}



