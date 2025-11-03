using System;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace Gamebox
{
    public partial class GameboxConfig // Leagues
    {
        [Serializable] private struct LeagueData
        {
            public LeagueType leagueType;
            public int ratingRequire;
            public Sprite iconSprite;
            public Color frameColor;
        }


        public LeagueType GetLeagueType(int rating)
        {
            rating = Mathf.Max(0, rating);
            int commonRequire = 0;

            for (int i = 0; i < _leagues.Count; i++)
            {
                commonRequire += _leagues[i].ratingRequire;

                if (rating <= commonRequire)
                    return _leagues[i].leagueType;
            }

            return _leagues[^1].leagueType;
        }

        public Sprite GetLeagueSprite(LeagueType leagueType)
            => _leagues.Find(data => data.leagueType == leagueType).iconSprite;

        public Color GetLeagueFrameColor(LeagueType leagueType)
            => _leagues.Find(data => data.leagueType == leagueType).frameColor;


        public int GetLeagueStage(LeagueType leagueType) 
            => int.Parse(leagueType.ToString().Split('_')[1]);

        public int GetLeagueRatingRequire(LeagueType leagueType)
            => _leagues.Find(data => data.leagueType == leagueType).ratingRequire;

        public int GetCurrentLeagueRating(int rating)
        {
            rating = Mathf.Max(0, rating);

            for (int i = 0; i < _leagues.Count; i++)
            {
                var require = _leagues[i].ratingRequire;

                if (rating - require >= 0)
                {
                    rating -= require;
                    break;
                }
            }

            return rating;
        }



    }
}
