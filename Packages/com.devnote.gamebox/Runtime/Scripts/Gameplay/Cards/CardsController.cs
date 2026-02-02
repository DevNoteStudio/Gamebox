using System;
using DevNote;

namespace Gamebox
{
    public class CardsController
    {
        
        private readonly Viewer<CardsScreenView> cardsScreenViewer; public CardsScreenView CardsScreen => cardsScreenViewer.View;
        private readonly Viewer<CardInfoWindowView> cardInfoWindowViewer; public CardInfoWindowView CardInfoWindow => cardInfoWindowViewer.View;
        private readonly Viewer<UnlockCardCellWindowView> unlockCardCellWindowViewer;



        public CardsController()
        {
            cardsScreenViewer = new(IConfigs.GetViewPrefab<CardsScreenView>());
            cardInfoWindowViewer = new(IConfigs.GetViewPrefab<CardInfoWindowView>());
            unlockCardCellWindowViewer = new(IConfigs.GetViewPrefab<UnlockCardCellWindowView>());

        }

        public void ShowCardsScreen() => cardsScreenViewer.ShowExpand(UI.Container);
        public void HideCardsScreen() => cardsScreenViewer.Hide();

        public void ShowUnlockCardCellWindow(int cellIndex) 
            => unlockCardCellWindowViewer.ShowFaded(UI.Container).Display(cellIndex).AnimateShow();
        public void HideUnlockCardCellWindow() 
            => unlockCardCellWindowViewer.AnimateFadedHide(unlockCardCellWindowViewer.View.AnimateHide);




        public void ShowCardInfoWindow(CardType cardType)
        {
            cardInfoWindowViewer.ShowFaded(UI.Container).Display(cardType).AnimateShow();
            IGameState.Cards.SetCardAsViewed(cardType);
        }


        public void HideCardInfoWindow()
            => cardInfoWindowViewer.AnimateFadedHide(cardInfoWindowViewer.View.AnimateHide);



        public void SetCardToFreeOrLastCell(CardType cardType)
        {
            int cellIndex = GetFreeOrLastCellIndex();
            IGameState.Cards.SetCardToCell(cellIndex, cardType);
        }

        public void RemoveCard(CardType cardType)
        {
            for (int i = 0; i < CardsState.CELLS_AMOUNT; i++)
            {
                if (IGameState.Cards.GetCellCard(i) == cardType)
                {
                    IGameState.Cards.SetCardToCell(i, CardType.Empty);
                    return;
                }
            }

            throw new Exception($"Can't remove not active card: {cardType}");
        }

        public bool TryUpgradeCard(CardType cardType)
        {
            int currentLevel = IGameState.Cards.GetLevel(cardType);
            int price = IConfigs.Gamebox.GetCardUpgradePrice(currentLevel);

            if (IGameState.Items.Get(ItemKey.Coins) >= price)
            {
                IGameState.Cards.IncreaseLevel(cardType);
                IGameState.Items.Spend(ItemKey.Coins, price);
                return true;
            }

            else return false;
        }

        public bool TryBuyCardCell(int cellIndex)
        {
            int price = IConfigs.Gamebox.GetCardCellGemPrice(cellIndex);

            if (IGameState.Items.Get(ItemKey.Gems) >= price)
            {
                IGameState.Cards.SetCardToCell(cellIndex, CardType.Empty);
                IGameState.Items.Spend(ItemKey.Gems, price);
                return true;
            }

            else return false;
        }



        private int GetFreeOrLastCellIndex()
        {
            for (int i = 0; i < CardsState.CELLS_AMOUNT; i++)
            {
                var cellCard = IGameState.Cards.GetCellCard(i);

                if (cellCard == CardType.Empty)
                    return i;

                if (cellCard == CardType.Locked)
                    return i - 1;
            }

            return CardsState.CELLS_AMOUNT - 1;
        }


        



    }
}
