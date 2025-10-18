using System.Collections.Generic;
using DevNote;

namespace Gamebox
{
    public class PopupController
    {
        private int _interstitialsLeftToShowNoAdsWindow;
        private PriorityPopupList _priorityPopupList;

        private readonly Viewer<NoAdsWindowView> noAdsWindowViewer;
        private readonly Viewer<ItemTutorialWindowView> itemTutorialWindowViewer;
        private readonly IAds ads;
        private readonly IReview review;

        public PopupController(LevelController levelController, IAds ads, IReview review)
        {
            _priorityPopupList = GetPriorityPopupList();

            this.ads = ads;
            this.review = review;

            noAdsWindowViewer = new(IConfigs.GetViewPrefab<NoAdsWindowView>());
            itemTutorialWindowViewer = new(IConfigs.GetViewPrefab<ItemTutorialWindowView>());

            _interstitialsLeftToShowNoAdsWindow = IConfigs.Gamebox.InterstitialsShowsToShowNoAdsWindow;

            IAds.OnInterstitialShown += OnInterstitialShown;
            levelController.OnLevelStarted += OnLevelStarted;
        }

        private PriorityPopupList GetPriorityPopupList() => new PriorityPopupList(new List<PopupData>()
        {
            new PopupData
            {
                popupType = PopupType.ItemTutorial,
                showCondition = () => IConfigs.Gamebox.TryGetItemForTutorial(out _),
                priority = 1,
            },
            new PopupData
            {
                popupType = PopupType.RateUs,
                showCondition = () => IConfigs.Gamebox.RateUsNow,
                priority = 2,
            },
            new PopupData
            {
                popupType = PopupType.NoAds,
                showCondition = () => _interstitialsLeftToShowNoAdsWindow <= 0,
                priority = 3,
            },
        });


        private void OnLevelStarted()
        {
            if (IConfigs.Gamebox.CanShowInterstitial)
                ads.ShowInterstitial(AdKey.LevelStartInterstitial, (result) => HandleShowPopup());

            else HandleShowPopup();
        }

        private void OnInterstitialShown(AdKey key, AdShowStatus status)
        {
            if (status == AdShowStatus.Success) 
                _interstitialsLeftToShowNoAdsWindow--;
        }


        private void HandleShowPopup()
        {
            if (_priorityPopupList.TryGetNextPopup(out PopupType popupType) == false)
                return;

            switch (popupType)
            {
                case PopupType.ItemTutorial:
                    IConfigs.Gamebox.TryGetItemForTutorial(out ItemKey itemKey);
                    itemTutorialWindowViewer.ShowWindow(UI.Container).Display(itemKey).AnimateShow();
                    break;

                case PopupType.RateUs:
                    break;

                case PopupType.NoAds:
                    _interstitialsLeftToShowNoAdsWindow = IConfigs.Gamebox.InterstitialsShowsToShowNoAdsWindow;
                    noAdsWindowViewer.ShowWindow(UI.Container).Display().AnimateShow();
                    break;
            }
        }

        public void HidePopup(PopupType popupType)
        {
            switch (popupType)
            {
                case PopupType.ItemTutorial:
                    itemTutorialWindowViewer.AnimateHideWindow(itemTutorialWindowViewer.View.AnimateHide);
                    break;

                case PopupType.RateUs:
                    break;

                case PopupType.NoAds:
                    noAdsWindowViewer.AnimateHideWindow(noAdsWindowViewer.View.AnimateHide);
                    break;
            }
        }


    }
}



