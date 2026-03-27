using DevNote;
using UnityEngine;

namespace Gamebox
{
    public class AdWarningController
    {

        private readonly Viewer<AdWarningWindowView> adWarningWindowViewer;
        private readonly Holder<IAds> ads = new();


        public AdWarningController()
        {
            adWarningWindowViewer = new(IConfigs.GetViewPrefab<AdWarningWindowView>());
        }

        public bool TryShowAdWarningWindow()
        {
            if (!DevNote.IGameState.NoAdsPurchased && ads.Item.InterstitialAvailable && IConfigs.Gamebox.CanShowInterstitial)
            {
                adWarningWindowViewer.ShowFaded(UI.Container).Display().AnimateShow();
                return true;
            }
            else return false;
        }


        public void HideAdWarningWindow(bool showInterstitial)
        {
            adWarningWindowViewer.View.AnimateHide(onCompleted: () =>
            {
                adWarningWindowViewer.ForceFadedHide();

                if (showInterstitial)
                    ads.Item.ShowInterstitial(AdKey.DuringLevelInterstitial);
            });
        }




    }
}
