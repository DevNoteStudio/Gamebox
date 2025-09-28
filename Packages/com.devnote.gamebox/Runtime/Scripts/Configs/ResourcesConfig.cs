using System;
using DevNote;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public partial class GameboxConfig : ScriptableObject
    {
        public VictoryScreenView VictoryScreenPrefab => Resources.Load<VictoryScreenView>("VictoryScreen");
        public LoseWindowView LoseWindowPrefab => Resources.Load<LoseWindowView>("LoseWindow");
        public LevelsScreenView LevelsScreenPrefab => Resources.Load<LevelsScreenView>("LevelsScreen");
        public LocationsScreenView LocationsScreenPrefab => Resources.Load<LocationsScreenView>("LocationsScreen");
        public NoAdsWindowView NoAdsWindowPrefab => Resources.Load<NoAdsWindowView>("NoAdsWindow");
        public TestLevelView TestLevelPrefab => Resources.Load<TestLevelView>("TestLevel");
        public Image ScreenFadePrefab => Resources.Load<Image>("ScreenFade");



        public SoundUnit ClickSound => Resources.Load<SoundUnit>("Click");
        public SoundUnit OpenClickSound => Resources.Load<SoundUnit>("OpenClick");
        public SoundUnit PointerEnterSound => Resources.Load<SoundUnit>("PointerEnter");
        public SoundUnit ShowSound => Resources.Load<SoundUnit>("Show");
        public SoundUnit HideSound => Resources.Load<SoundUnit>("Hide");

    }
}


