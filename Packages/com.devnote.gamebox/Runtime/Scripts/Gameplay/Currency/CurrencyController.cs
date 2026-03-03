using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gamebox
{

    public class CurrencyController
    {
        public event Action OnCoinsEarned;
        public event Action OnCoinsSpent;


        public int DisplayedCoins { get; private set; }


        private readonly LevelController levelController;
        private CurrencyView CurrencyView => levelController.GameplayScreen.CurrencyView;


        public CurrencyController(LevelController levelController)
        {
            this.levelController = levelController;
            DisplayedCoins = IGameState.Items.Get(ItemKey.Coins);
        }



        public void AddCoinsRollup(int coins, int particles, Vector3 worldPosition)
        {
            IGameState.Items.Add(ItemKey.Coins, coins);

            Vector2 canvasPosition = Utils.WorldToCanvas(worldPosition, UI.Canvas, GameboxSceneContext.MainCamera);
            CurrencyView.AnimateCoinsRollup(coins, particles, canvasPosition);
        }

        public async void UpdateCoinsRollup(int particles, float delay = 0f)
        {
            await UniTask.WaitForSeconds(delay);

            int addDisplayedCoins = IGameState.Items.Get(ItemKey.Coins) - DisplayedCoins;
            CurrencyView.AnimateCoinsRollup(addDisplayedCoins, particles, Vector2.zero);
        }

        public void EarnDisplayedCoins(int coins)
        {
            DisplayedCoins += coins;
            OnCoinsEarned?.Invoke();
        }

        public bool TrySpendCoins(int coins)
        {
            if (IGameState.Items.Get(ItemKey.Coins) >= coins)
            {
                IGameState.Items.Spend(ItemKey.Coins, coins);

                DisplayedCoins -= coins;
                OnCoinsSpent?.Invoke();

                return true;
            }
            else return false;
        }





    }
}
