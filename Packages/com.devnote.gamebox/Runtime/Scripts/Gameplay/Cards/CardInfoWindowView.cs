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
        [SerializeField] private Material _upgradeAvailableMaterial;
        [SerializeField] private Material _upgradeNotAvailableMaterial;
        [SerializeField] private TextMeshProUGUI _upgradeNotAvailableText;

        private CardType _cardType;

        private readonly Holder<CardsController> cardsController = new();


        private void Start()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            _takeButton.onClick.AddListener(OnTakeButtonClick);
            _upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
            _removeButton.onClick.AddListener(OnRemoveButtonClick);
        }

        private void OnEnable()
        {
            IGameState.Cards.OnCardChanged += OnCardChanged;

        }

        private void OnDisable()
        {
            IGameState.Cards.OnCardChanged -= OnCardChanged;
        }


        private void OnCardChanged(CardType cardType)
        {
            if (cardType == _cardType)
                Display(_cardType);
        }

        public CardInfoWindowView Display(CardType cardType)
        {
            _cardType = cardType;

            var config = IConfigs.Gamebox;
            var cardsState = IGameState.Cards;
            var rarity = config.GetCardRarity(cardType);

            int currentLevel = cardsState.GetLevel(cardType);
            int nextLevel = currentLevel + 1;
            int price = config.GetCardUpgradePrice(currentLevel);

            _iconImage.LoadSprite(AssetLoader.LoadCardSprite(cardType));

            _nameText.text = IConfigs.Gamebox.GetCardName(cardType); 
            _rarityText.text = IConfigs.Gamebox.GetRarityName(rarity);
            _rarityText.color = IConfigs.Internal.GetRarityTextColor(rarity);
            _descriptionText.text = Localization.GetLocalizedText($"{cardType}_desc");

            bool cardsEnough = cardsState.GetAmountOnCurrentLevel(cardType) 
                >= cardsState.GetRequiredCardsOnCurrentLevel(cardType);

            bool coinsEnough = IGameState.Items.Get(ItemKey.Coins) >= price;

            //_nextFeatureText.gameObject.SetActive(upgradeAvailable);
            //_arrowObject.SetActive(upgradeAvailable);

            var upgradeButtonMaterial = cardsEnough && coinsEnough ?
                _upgradeAvailableMaterial : _upgradeNotAvailableMaterial;

            _upgradeNotAvailableText.gameObject.SetActive(!cardsEnough || !coinsEnough);

            if (!cardsEnough)
                _upgradeNotAvailableText.text = Localization.GetLocalizedText("no_cards");

            else if (!coinsEnough)
                _upgradeNotAvailableText.text = Localization.GetLocalizedText("no_coins");


            _upgradeButton.image.material = upgradeButtonMaterial;
            _upgradeButton.interactable = cardsEnough;

            bool isActive = cardsState.IsActive(cardType);
            _removeButton.gameObject.SetActive(isActive);
            _takeButton.gameObject.SetActive(!isActive);

            
            _currentFeatureText.text = config.GetCardShortDescription(cardType, currentLevel);
            _currentLevelText.text = $"{currentLevel}<size=85%> {Localization.GetLocalizedText("lvl")}";
            _nextFeatureText.text = config.GetCardShortDescription(cardType, nextLevel);
            _nextLevelText.text = $"{nextLevel}<size=85%> {Localization.GetLocalizedText("lvl")}";
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
