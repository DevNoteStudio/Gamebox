using System;
using DevNote;
using DG.Tweening;
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
        [SerializeField] private Button _upgradeButton; public Button UpgradeButton => _upgradeButton;
        [SerializeField] private Button _takeButton; public Button TakeButton => _takeButton;
        [SerializeField] private Button _removeButton;
        [SerializeField] private Material _upgradeAvailableMaterial;
        [SerializeField] private Material _upgradeNotAvailableMaterial;
        [SerializeField] private TextMeshProUGUI _upgradeNotAvailableText;
        [SerializeField] private Color _upgradeTextColor;

        private CardType _cardType;
        private Tween _upgradeTween;

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
            IGameState.Items.Subscribe(ItemKey.Coins, OnCoinsChanged);

        }

        private void OnDisable()
        {
            IGameState.Cards.OnCardChanged -= OnCardChanged;
            IGameState.Items.Dispose(ItemKey.Coins, OnCoinsChanged);
        }

        private void OnCoinsChanged() => Display(_cardType);

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
            int price = config.GetCardUpgradePrice(cardType, currentLevel);

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

        private void AnimateUpgrade()
        {
            //Sound.Play(SoundName.CardUpgrade);

            const float TO_SCALE = 1.3f;
            const float DURATION = 0.4f;

            _currentFeatureText.color = _upgradeTextColor;

            _upgradeTween?.Kill();
            _upgradeTween = DOTween.Sequence()
                .Append(_currentFeatureText.transform.DOScale(TO_SCALE, DURATION / 2f).SetEase(Ease.OutQuad))
                .Append(_currentFeatureText.transform.DOScale(1f, DURATION / 2f).SetEase(Ease.InQuad))
                .OnComplete(() => _currentFeatureText.color = Color.white);

        }



        private void OnCloseButtonClick() => cardsController.Item.HideCardInfoWindow();


        private void OnTakeButtonClick()
        {
            cardsController.Item.SetCardToFreeOrLastCell(_cardType);
            cardsController.Item.HideCardInfoWindow();
        }

        private void OnUpgradeButtonClick()
        {
            if (cardsController.Item.TryUpgradeCard(_cardType))
                AnimateUpgrade();
        }

        private void OnRemoveButtonClick()
        {
            cardsController.Item.RemoveCard(_cardType);
            cardsController.Item.HideCardInfoWindow();
        }


    }
}
