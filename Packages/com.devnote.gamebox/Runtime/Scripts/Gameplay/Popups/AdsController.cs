using DevNote;

namespace Gamebox
{
    public class AdsController
    {
        private int _interstitialsLeftToShowNoAdsWindow;

        private readonly Viewer<NoAdsWindowView> noAdsWindowViewer;
        private readonly IAds ads;

        public AdsController(LevelController levelController, IAds ads)
        {
            _interstitialsLeftToShowNoAdsWindow = 1;
            noAdsWindowViewer = new(IConfigs.Gamebox.NoAdsWindowPrefab);
            this.ads = ads;

            IAds.OnInterstitialShown += OnInterstitialShown;
            levelController.OnLevelStarted += OnLevelStarted;
        }

        private void OnLevelStarted()
        {
            if (IGameState.Levels.CurrentLevelNumber >= IConfigs.Gamebox.InterstitialsFromLevel)
                ads.ShowInterstitial(IAdKey.LevelStartInterstitial);
        }

        private void OnInterstitialShown(string key, AdShowStatus status)
        {
            if (status != AdShowStatus.Success) return;

            _interstitialsLeftToShowNoAdsWindow--;
            if (_interstitialsLeftToShowNoAdsWindow <= 0)
            {
                ShowNoAdsWindow();
                _interstitialsLeftToShowNoAdsWindow = IConfigs.Gamebox.InterstitialsShowsToShowNoAdsWindow;
            }
        }

        private void ShowNoAdsWindow()
        {
            noAdsWindowViewer.ShowExpand(UI.Container).Display().AnimateShow();
        }

        public void HideNoAdsWindow()
        {
            noAdsWindowViewer.View.AnimateHide(onCompleted: noAdsWindowViewer.Hide);
        }


    }
}


