using System;
using System.Collections.Generic;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    public partial class GameboxConfig // Leagues
    {
        [Serializable] private class LeagueData
        {
            public LeagueType leagueType;
            public int ratingRequire;
            public Sprite iconSprite;
            public Color frameColor;
            public List<ItemPack> rewardItems;
        }


        public List<ItemPack> GetLeagueRewardItems(LeagueType leagueType)
            => _leagues.Find(data => data.leagueType == leagueType).rewardItems;


        public LeagueType GetLeagueType(int rating)
        {
            rating = Mathf.Max(0, rating);
            int commonRequire = 0;

            for (int i = 1; i < _leagues.Count; i++)
            {
                commonRequire += _leagues[i].ratingRequire;

                if (rating < commonRequire)
                    return _leagues[i - 1].leagueType;
            }

            return _leagues[^1].leagueType;
        }

        public Sprite GetLeagueSprite(LeagueType leagueType)
            => _leagues.Find(data => data.leagueType == leagueType).iconSprite;

        public Color GetLeagueFrameColor(LeagueType leagueType)
            => _leagues.Find(data => data.leagueType == leagueType).frameColor;


        public string GetLeagueStageSymbol(LeagueType leagueType)
        {
            int leagueStage = int.Parse(leagueType.ToString().Split('_')[1]);

            return leagueStage switch
            {
                1 => "I", 2 => "II", 3 => "III",
                _ => throw null
            };
        }

        public int GetLeagueRatingRequire(LeagueType leagueType)
            => _leagues.Find(data => data.leagueType == leagueType).ratingRequire;

        public int GetCurrentLeagueRating(int rating)
        {
            rating = Mathf.Max(0, rating);

            for (int i = 0; i < _leagues.Count; i++)
            {
                var require = _leagues[i].ratingRequire;

                if (rating - require < 0) break;
                else rating -= require;
            }

            return rating;
        }


        public string GetLeagueName(LeagueType leagueType)
        {
            string[] splitName = leagueType.ToString().Split('_');
            return Localization.GetLocalizedText("league_" + splitName[0]) + " " + splitName[1];
        }
          

        public int GetRatingForLevelCompletion(int locationIndex, int levelIndex, int newStars)
        {
            int baseReward = _rewards.ratingForLevelComplete + (_rewards.ratingForNewStar * newStars);
            return (int)(baseReward * _locations[locationIndex].ratingMultiplier);
        }

        public bool IsLastLeague(LeagueType leagueType) => leagueType == LeagueType.Imperium_1;


    }
}
