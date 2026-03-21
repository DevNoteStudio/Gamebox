using UnityEngine;

namespace Gamebox
{
    public class CurrencyController
    {

        private readonly LevelController levelController;
        private CurrencyView CurrencyView => levelController.GameplayScreen.CurrencyView;


        public CurrencyController(LevelController levelController)
        {
            this.levelController = levelController;
        }

        public void AddCoinsWithRollup(int coins, int particles, Vector3 worldPosition)
        {
            IGameState.Items.Add(ItemKey.Coins, coins);

            Vector2 canvasPosition = Utils.WorldToCanvas(worldPosition, UI.Canvas, GameboxSceneContext.MainCamera);
            CurrencyView.AnimateDisplayWithRollup(particles, canvasPosition);
        }

        public bool TrySpendCoins(int coins)
        {
            if (IGameState.Items.Get(ItemKey.Coins) >= coins)
            {
                IGameState.Items.Spend(ItemKey.Coins, coins);
                CurrencyView.AnimateDisplayWithSpending();

                return true;
            }
            else return false;
        }





    }
}
