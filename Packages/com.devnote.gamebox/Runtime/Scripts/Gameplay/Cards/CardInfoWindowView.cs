using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class CardInfoWindowView : Window
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _rarityText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private GameObject _upgradeArrowObject;
        [SerializeField] private TextMeshProUGUI _currentFeatureText;
        [SerializeField] private TextMeshProUGUI _currentLevelText;
        [SerializeField] private TextMeshProUGUI _nextFeatureText;
        [SerializeField] private TextMeshProUGUI _nextLevelText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private GameObject _arrowObject;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _takeButton;
        [SerializeField] private Button _removeButton;

        private CardType _cardType;

        private readonly Holder<CardsController> cardsController = new();


        private void Start()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            _takeButton.onClick.AddListener(OnTakeButtonClick);
            _upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
            _removeButton.onClick.AddListener(OnRemoveButtonClick);
        }


        public CardInfoWindowView Display(CardType cardType)
        {
            _cardType = cardType;

            var config = IConfigs.Gamebox;
            var cardsState = IGameState.Cards;

            var rarity = config.GetCardRarity(cardType);

            _nameText.text = Localization.GetLocalizedText($"{cardType}_name");
            _rarityText.text = Localization.GetLocalizedText($"{rarity}_card");
            _rarityText.color = IConfigs.Internal.GetRarityTextColor(rarity);
            _descriptionText.text = Localization.GetLocalizedText($"{cardType}_desc");

            bool upgradeAvailable = cardsState.GetAmountOnCurrentLevel(cardType) 
                >= cardsState.GetRequiredCardsOnCurrentLevel(cardType);

            _nextFeatureText.gameObject.SetActive(upgradeAvailable);
            _arrowObject.SetActive(upgradeAvailable);
            _upgradeButton.gameObject.SetActive(upgradeAvailable);

            bool isActive = cardsState.IsActive(cardType);
            _removeButton.gameObject.SetActive(isActive);
            _takeButton.gameObject.SetActive(!isActive);

            int currentLevel = cardsState.GetLevel(cardType);
            int nextLevel = currentLevel + 1;

            _currentFeatureText.text = config.GetCardShortDescription(cardType, currentLevel);
            _currentLevelText.text = $"{currentLevel}<size=85%> {Localization.GetLocalizedText("lvl")}";
            _nextFeatureText.text = config.GetCardShortDescription(cardType, nextLevel);
            _nextLevelText.text = $"{nextLevel}<size=85%> {Localization.GetLocalizedText("lvl")}";

            int price = config.GetCardUpgradePrice(currentLevel);
            _priceText.text = $"{Localization.GetLocalizedText("upgrade")}\n<size=120%><sprite=0>{price}";



            return this;
        }


        private void OnCloseButtonClick() => cardsController.Item.HideCardInfoWindow();


        private void OnTakeButtonClick()
        {
            cardsController.Item.SetCardToFreeOrLastCell(_cardType);
            cardsController.Item.HideCardInfoWindow();
        }

        private void OnUpgradeButtonClick()
        {
            cardsController.Item.TryUpgradeCard(_cardType);
        }

        private void OnRemoveButtonClick()
        {
            cardsController.Item.RemoveCard(_cardType);
            cardsController.Item.HideCardInfoWindow();
        }


    }
}
