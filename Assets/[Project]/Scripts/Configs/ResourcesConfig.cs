
using System;
using UnityEngine.UI;

namespace DevNote.Gamebox
{
    public partial class GameboxConfig // Resources
    {
        [Serializable] private struct ResourcesData
        {
            public VictoryScreenView victoryScreenPrefab;
            public LoseWindowView loseWindowPrefab;
            public LevelsScreenView levelsScreenPrefab;
            public LocationsScreenView locationsScreenPrefab;
            public Image screenFadePrefab;
        }

        public VictoryScreenView VictoryScreenPrefab => _resources.victoryScreenPrefab;
        public LoseWindowView LoseWindowPrefab => _resources.loseWindowPrefab;
        public LevelsScreenView LevelsScreenPrefab => _resources.levelsScreenPrefab;
        public LocationsScreenView LocationsScreenPrefab => _resources.locationsScreenPrefab;
        public Image ScreenFadePrefab => _resources.screenFadePrefab;


    }
}


