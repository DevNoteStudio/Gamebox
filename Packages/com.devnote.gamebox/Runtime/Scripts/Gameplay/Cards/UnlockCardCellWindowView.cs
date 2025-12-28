using System;
using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class UnlockCardCellWindowView : Window
    {
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _buyButton;

        private int _cellIndex;

        private readonly Holder<CardsController> cardsController = new();


        private void Start()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            _buyButton.onClick.AddListener(OnBuyButtonClick);
        }


        public UnlockCardCellWindowView Display(int cellIndex)
        {
            _cellIndex = cellIndex;

            _descriptionText.text = Localization.GetLocalizedText("unlock_cell_desc").Replace("{VALUE}", (cellIndex + 1).ToString());

            int price = IConfigs.Gamebox.GetCardCellGemPrice(cellIndex);
            _priceText.text = $"{Localization.GetLocalizedText("buy")}\n<sprite=0>{price}";


            return this;
        }



        private void OnBuyButtonClick()
        {
            cardsController.Item.TryBuyCardCell(_cellIndex);
        }

        private void OnCloseButtonClick()
        {
            cardsController.Item.HideUnlockCardCellWindow();
        }



    }
}
