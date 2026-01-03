using System;
using System.Diagnostics;
using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class BoxWindowView : Window
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _openSingleButton;
        [SerializeField] private TextMeshProUGUI _openSingleButtonText;
        [SerializeField] private Button _openMultyButton;
        [SerializeField] private TextMeshProUGUI _openMultyButtonText;
        [SerializeField] private Image _boxIconImage;

        private readonly Holder<ShopController> shopController = new();



        private void Start()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            _openSingleButton.onClick.AddListener(OnOpenSingleButtonClick);
            _openMultyButton.onClick.AddListener(OnOpenMultyButtonClick);
        }

        public BoxWindowView Display(ItemKey boxItemKey)
        {
            _titleText.text = Localization.GetLocalizedText($"{boxItemKey}_name");
            _descriptionText.text = IConfigs.Gamebox.GetBoxDescription(boxItemKey);
            _boxIconImage.LoadSprite(AssetLoader.LoadItemSprite(boxItemKey));

            int amount = IGameState.Items.Get(boxItemKey);

            _openMultyButton.gameObject.SetActive(amount != 1);

            if (amount == 0)
            {
                int price = IConfigs.Gamebox.GetBoxPrice(boxItemKey, out bool buyForGems);
                int spriteIndex = buyForGems ? 1 : 0;

                _openSingleButtonText.text = $"<size=90%>{Localization.GetLocalizedText("buy")} " +
                    $"<size=75%>õ</size>1</size>\n<sprite={spriteIndex}>{price}";

                _openMultyButtonText.text = $"<size=90%>{Localization.GetLocalizedText("buy")} " +
                    $"<size=75%>õ</size>10</size>\n<sprite={spriteIndex}>{price * 10}";
            }
            else
            {
                string prefixText = $"{Localization.GetLocalizedText("open_box")} <size=85%>x</size>";
                _openSingleButtonText.text = $"{prefixText}1";
                _openMultyButtonText.text = $"{prefixText}{amount}";
            }
            

            return this;
        }

        private void OnOpenMultyButtonClick()
        {
            throw new NotImplementedException();
        }

        private void OnOpenSingleButtonClick()
        {
            throw new NotImplementedException();
        }

        private void OnCloseButtonClick()
        {
            shopController.Item.HideBoxWindow();
        }

        




    }
}
