
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
            public SoundUnit clickSound;
            public SoundUnit openClickSound;
            public SoundUnit pointerEnterSound;
            public SoundUnit showSound;
        }

        public VictoryScreenView VictoryScreenPrefab => _resources.victoryScreenPrefab;
        public LoseWindowView LoseWindowPrefab => _resources.loseWindowPrefab;
        public LevelsScreenView LevelsScreenPrefab => _resources.levelsScreenPrefab;
        public LocationsScreenView LocationsScreenPrefab => _resources.locationsScreenPrefab;
        public Image ScreenFadePrefab => _resources.screenFadePrefab;


        public SoundUnit ClickSound => _resources.clickSound;
        public SoundUnit OpenClickSound => _resources.openClickSound;
        public SoundUnit PointerEnterSound => _resources.pointerEnterSound;
        public SoundUnit ShowSound => _resources.showSound;


    }
}


