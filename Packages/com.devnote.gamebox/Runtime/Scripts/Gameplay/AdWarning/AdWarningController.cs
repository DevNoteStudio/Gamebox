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

        public void ShowAdWarningWindow()
        {
            if (DevNote.IGameState.NoAdsPurchased)
                Debug.LogWarning($"{Info.LogPrefix} You try to show Ad Warning Window while No Ads is purchased");

            else adWarningWindowViewer.ShowFaded(UI.Container).Display().AnimateShow();
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
