using System;
using UnityEngine;

namespace Gamebox
{
    public enum RollupType { OnlyEffect, AddCurrency }


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



        public void AnimateCoinsRollup(RollupType rollupType, int coins, int particles, Vector3 worldPosition)
        {
            if (rollupType == RollupType.AddCurrency)
                IGameState.Items.Add(ItemKey.Coins, coins);

            Vector2 canvasPosition = Utils.WorldToCanvas(worldPosition, UI.Canvas, GameboxSceneContext.MainCamera);
            CurrencyView.AnimateCoinsRollup(coins, particles, canvasPosition);
        }

        public void AnimateCoinsRollup(RollupType rollupType, int coins, int particles)
        {
            if (rollupType == RollupType.AddCurrency)
                IGameState.Items.Add(ItemKey.Coins, coins);

            CurrencyView.AnimateCoinsRollup(coins, particles, Vector2.zero);
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
