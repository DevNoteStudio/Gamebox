using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gamebox
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinsText;


        private void OnEnable()
        {
            IGameState.Items.Subscribe(ItemKey.Coins, Display);
            Display();
        }

        private void OnDisable()
        {
            IGameState.Items.Dispose(ItemKey.Coins, Display);
        }

        private void Display()
        {
            _coinsText.text = $"<sprite=0>{IGameState.Items.Get(ItemKey.Coins)}";
        }


    }
}
