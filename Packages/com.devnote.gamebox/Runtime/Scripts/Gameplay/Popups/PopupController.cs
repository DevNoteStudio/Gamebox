using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DevNote;

namespace Gamebox
{
    public class PopupController
    {
        private int _interstitialsLeftToShowNoAdsWindow;
        private PriorityPopupList _priorityPopupList;

        private readonly Viewer<NoAdsWindowView> noAdsWindowViewer;
        private readonly Viewer<ItemTutorialWindowView> itemTutorialWindowViewer;
        private readonly Viewer<RateUsWindowView> rateUsWindowViewer;
        private readonly IAds ads;
        private readonly IReview review;
        private readonly IPurchase purchase;

        private const float SHOW_DELAY = 0.3f;


        public PopupController(LevelController levelController, IAds ads, IReview review, IPurchase purchase)
        {
            _priorityPopupList = GetPriorityPopupList();

            this.ads = ads;
            this.review = review;
            this.purchase = purchase;

            noAdsWindowViewer = new(IConfigs.GetViewPrefab<NoAdsWindowView>());
            itemTutorialWindowViewer = new(IConfigs.GetViewPrefab<ItemTutorialWindowView>());
            rateUsWindowViewer = new(IConfigs.GetViewPrefab<RateUsWindowView>());

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
                showCondition = () => IConfigs.Gamebox.RateUsNow && review.ReviewIsAvailable,
                priority = 2,
            },
            new PopupData
            {
                popupType = PopupType.NoAds,

                showCondition = () => _interstitialsLeftToShowNoAdsWindow <= 0 
                    && purchase.PlatformIsSupportsPurchases,

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


        private async void HandleShowPopup()
        {
            if (_priorityPopupList.TryGetNextPopup(out PopupType popupType) == false)
                return;

            await UniTask.Delay((int)(SHOW_DELAY * 1000));


            switch (popupType)
            {
                case PopupType.ItemTutorial:
                    IConfigs.Gamebox.TryGetItemForTutorial(out ItemKey itemKey);
                    itemTutorialWindowViewer.ShowFaded(UI.Container).Display(itemKey).AnimateShow();
                    break;

                case PopupType.RateUs:
                    rateUsWindowViewer.ShowFaded(UI.Container).AnimateShow();
                    break;

                case PopupType.NoAds:
                    _interstitialsLeftToShowNoAdsWindow = IConfigs.Gamebox.InterstitialsShowsToShowNoAdsWindow;
                    noAdsWindowViewer.ShowFaded(UI.Container).Display().AnimateShow();
                    break;
            }
        }

        public void HidePopup(PopupType popupType)
        {
            switch (popupType)
            {
                case PopupType.ItemTutorial:
                    itemTutorialWindowViewer.AnimateFadedHide(itemTutorialWindowViewer.View.AnimateHide);
                    break;

                case PopupType.RateUs:
                    rateUsWindowViewer.AnimateFadedHide(rateUsWindowViewer.View.AnimateHide);
                    break;

                case PopupType.NoAds:
                    noAdsWindowViewer.AnimateFadedHide(noAdsWindowViewer.View.AnimateHide);
                    break;
            }
        }


        public void ShowLocationsTutorialWindow()
        {
            itemTutorialWindowViewer.ShowFaded(UI.Container).
                Display(ItemKey.LocationsUnlocked).AnimateShow();
        }



    }
}



