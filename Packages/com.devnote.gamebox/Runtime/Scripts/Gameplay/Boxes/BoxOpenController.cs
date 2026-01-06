using System.Collections.Generic;
using DevNote;

namespace Gamebox
{
    public class BoxOpenController
    {

        private readonly Viewer<BoxOpenScreenView> boxOpenScreenViewer;


        public BoxOpenController()
        {
            boxOpenScreenViewer = new(IConfigs.GetViewPrefab<BoxOpenScreenView>());
        }


        public bool TryBuyBox(ItemKey boxItemKey, int amount)
        {
            int price = IConfigs.Gamebox.GetBoxPrice(boxItemKey, out bool buyForGems) * amount;
            ItemKey currency = buyForGems ? ItemKey.Gems : ItemKey.Coins;

            if (IGameState.Items.Get(currency) >= price)
            {
                IGameState.Items.Spend(currency, price);
                IGameState.Items.Add(boxItemKey, amount);
                return true;
            }
            else return false;
        }



        public bool TryOpenBox(ItemKey boxItemKey, int boxAmount)
        {
            if (IGameState.Items.Get(boxItemKey) < boxAmount) 
                return false; 


            IGameState.Items.Spend(boxItemKey, boxAmount);

            var generatedCards = new Dictionary<CardType, int>();

            boxOpenScreenViewer.ShowExpand(UI.Container)
                .Display(boxItemKey, generatedCards).AnimateShow();

            return true;
        }


    }
}
