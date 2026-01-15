using System;
using System.Collections.Generic;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    public partial class GameboxConfig // Levels
    {
        [Serializable] private struct LocationData
        {
            public int levels;
            public LeagueType leagueRequire;
            public float coinsMultiplier;
            public float ratingMultiplier;
            public List<AdditionalLocationLevelRewardData> additionalLevelRewards;
        }


        public int LocationsAmount => _locations.Count;

        public string GetLocationName(int locationIndex)
            => Localization.GetLocalizedText($"location_name_{locationIndex}");

        public int GetLocationLevelsAmount(int locationIndex) => _locations[locationIndex].levels;

        public LeagueType GetLocationLeagueRequire(int locationIndex) 
            => _locations[locationIndex].leagueRequire;

        public bool TryGetUnlockedLocation(LeagueType leagueType, out int locationIndex)
        {
            int index = _locations.FindIndex(data => data.leagueRequire == leagueType);
            locationIndex = index;

            return index != -1;
        }



    }
}



