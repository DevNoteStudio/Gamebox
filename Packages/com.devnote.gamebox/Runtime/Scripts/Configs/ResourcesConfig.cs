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


    }
}


