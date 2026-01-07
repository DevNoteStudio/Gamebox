using System;
using DevNote;

namespace Gamebox
{
    public class ShopController
    {
        private readonly Viewer<ShopScreenView> shopScreenViewer;
        private readonly Viewer<BoxWindowView> boxWindowViewer;
        private readonly IPurchase purchase;



        public ShopController(IPurchase purchase)
        {
            shopScreenViewer = new(IConfigs.GetViewPrefab<ShopScreenView>());
            boxWindowViewer = new(IConfigs.GetViewPrefab<BoxWindowView>());
            this.purchase = purchase;
        }


        public void ShowShopScreen() => shopScreenViewer.ShowExpand(UI.Container);
        public void HideShopScreen() => shopScreenViewer.Hide();



        public void ShowBoxWindow(ItemKey boxItemKey) 
            => boxWindowViewer.ShowFaded(UI.Container).Display(boxItemKey).AnimateShow();

        public void HideBoxWindow()
            => boxWindowViewer.AnimateFadedHide(boxWindowViewer.View.AnimateHide);



        public void TryPurchaseGemPack(ProductKey productKey, Action<bool> onSuccess = null)
        {
            purchase.Purchase(productKey, 
                onSuccess: () => onSuccess?.Invoke(true),
                onError: () => onSuccess?.Invoke(false));
        }

        public bool TryPurchaseCoinsPack(int coinsPackIndex)
        {
            int price = IConfigs.Gamebox.GetCoinsPackPrice(coinsPackIndex);

            if (IGameState.Items.Get(ItemKey.Gems) >= price)
            {
                IGameState.Items.Spend(ItemKey.Gems, price);

                int coins = IConfigs.Gamebox.GetCoinsInsidePack(coinsPackIndex);
                IGameState.Items.Add(ItemKey.Coins, coins);
                return true;
            }

            return false;
        }


    }
}
