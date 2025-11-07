using AssetKits.ParticleImage;
using DevNote;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public partial class GameboxConfig // Resources
    {
        public Image ScreenFadePrefab => Resources.Load<Image>("Views/ScreenFade");
        public Image WindowFadePrefab => Resources.Load<Image>("Views/WindowFade");


        public ParticleImage CoinsRollupParticlePrefab => Resources.Load<ParticleImage>("CoinsRollup");
        public SoundUnit ClickSound => Resources.Load<SoundUnit>("Click");
        public SoundUnit OpenClickSound => Resources.Load<SoundUnit>("OpenClick");
        public SoundUnit PointerEnterSound => Resources.Load<SoundUnit>("PointerEnter");
        public SoundUnit ShowSound => Resources.Load<SoundUnit>("Show");
        public SoundUnit HideSound => Resources.Load<SoundUnit>("Hide");
        public SoundUnit CoinsRollupStartSound => Resources.Load<SoundUnit>("CoinsRollupStart");
        public SoundUnit CoinsRollupFinishSound => Resources.Load<SoundUnit>("CoinsRollupFinish");

    }
}


