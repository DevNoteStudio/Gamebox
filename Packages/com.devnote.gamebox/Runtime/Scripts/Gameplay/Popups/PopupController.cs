using System.Collections.ObjectModel;
using DevNote;

namespace Gamebox
{
    public class PopupController
    {
        private int _interstitialsLeftToShowNoAdsWindow;

        private readonly Viewer<NoAdsWindowView> noAdsWindowViewer;
        private readonly Viewer<ItemTutorialWindowView> itemTutorialWindowViewer;
        private readonly IAds ads;

        public PopupController(LevelController levelController, IAds ads)
        {
            this.ads = ads;

            noAdsWindowViewer = new(IConfigs.Gamebox.NoAdsWindowPrefab);
            itemTutorialWindowViewer = new(IConfigs.Gamebox.ItemTutorialWindowPrefab);

            _interstitialsLeftToShowNoAdsWindow = 1;

            IAds.OnInterstitialShown += OnInterstitialShown;
            levelController.OnLevelAfterStarted += OnLevelAfterStarted;
        }

        private void OnLevelAfterStarted() => HandleStartLevelPopup();

        private void OnInterstitialShown(AdKey key, AdShowStatus status)
        {
            if (status == AdShowStatus.Success) 
                _interstitialsLeftToShowNoAdsWindow--;
        }


        private void HandleStartLevelPopup()
        {
            if (TryGetItemForTutorial(out ItemKey itemKey))
                ShowItemTutorialWindow(itemKey);

            else if (_interstitialsLeftToShowNoAdsWindow <= 0)
                ShowNoAdsWindow();
        }


        private bool TryGetItemForTutorial(out ItemKey resultItemKey)
        {
            foreach (var itemKey in IConfigs.Gamebox.GetTutorialItemKeys())
            {
                if (IGameState.Items.Has(itemKey) && IGameState.ItemTutorials.IsCompleted(itemKey) == false)
                {
                    resultItemKey = itemKey;
                    return true;
                }
            }
            resultItemKey = ItemKey.Coins; 
            return false;
        }

        private void ShowItemTutorialWindow(ItemKey itemKey) 
            => itemTutorialWindowViewer.ShowExpand(UI.Container).Display(itemKey).AnimateShow();

        public void HideItemTutorialWindow() 
            => itemTutorialWindowViewer.View.AnimateHide(onCompleted: itemTutorialWindowViewer.Hide);



        private void ShowNoAdsWindow()
        {
            _interstitialsLeftToShowNoAdsWindow = IConfigs.Gamebox.InterstitialsShowsToShowNoAdsWindow;
            noAdsWindowViewer.ShowExpand(UI.Container).Display().AnimateShow();
        }


        public void HideNoAdsWindow() 
            => noAdsWindowViewer.View.AnimateHide(onCompleted: noAdsWindowViewer.Hide);





    }
}



