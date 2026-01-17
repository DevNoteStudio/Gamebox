using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{

    public enum RarityType { Common, Rare, Epic, Legendary }


    public class CardView : MonoBehaviour
    {
        [SerializeField] private Button _infoButton; public Button OpenButton => _infoButton;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private GameObject _levelObject;
        [SerializeField] private GameObject _shineObject;
        [SerializeField] private GameObject _progressObject;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private Image _progressFillImage;
        [SerializeField] private Image _progressIconImage;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private GameObject _newMark;
        [SerializeField] private GameObject _activeMark;
        [Space(10)]
        [SerializeField] private Color _progressColor;
        [SerializeField] private Color _upgradeColor;

        public CardType CardType { get; private set; }


        private const float INVENTORY_TEXT_SIZE = 43f;
        private const float CELL_TEXT_SIZE = 54f;
        private const float LOCKED_TEXT_SIZE = 64f;

        private readonly Color LOCKED_BACKGROUND_COLOR = new Color(0.3f, 0.3f, 0.3f, 1f);
        private readonly Color LOCKED_ICON_COLOR = new Color(0f, 0f, 0f, 0.5f);

        private readonly Holder<CardsController> cardsController = new();


        private void Start()
        {
            _infoButton.onClick.AddListener(OnInfoButtonClick);
        }

        
        public enum DisplayType { Inventory, Cell, OpenBox }
        public void Display(CardType cardType, DisplayType displayType)
        {
            CardType = cardType;

            var config = IConfigs.Gamebox;

            var rarityType = config.GetCardRarity(cardType);
            var isLocked = !IGameState.Cards.Has(cardType) && displayType != DisplayType.OpenBox;

            _backgroundImage.color = isLocked ? LOCKED_BACKGROUND_COLOR : IConfigs.Internal.GetRarityBackgroundColor(rarityType);

            _iconImage.color = isLocked ? LOCKED_ICON_COLOR : Color.white;
            _iconImage.LoadSprite(AssetLoader.LoadCardSprite(cardType));

            _shineObject.SetActive(!isLocked);

            _levelObject.SetActive(!isLocked && displayType == DisplayType.Inventory);
            _levelText.text = IGameState.Cards.GetLevel(cardType).ToString();

            _descriptionText.alignment = isLocked ? TextAlignmentOptions.Center : TextAlignmentOptions.Top;

            _descriptionText.fontSizeMax = 
                isLocked ? LOCKED_TEXT_SIZE : 
                displayType == DisplayType.Cell ? CELL_TEXT_SIZE : INVENTORY_TEXT_SIZE;

            _descriptionText.text = isLocked ? "?" : config.GetCardShortDescription(cardType);

            _progressObject.SetActive(!isLocked && displayType != DisplayType.Cell);

            _newMark.SetActive(displayType == DisplayType.Inventory && !isLocked && IGameState.Cards.IsNew(cardType));
            _activeMark.SetActive(displayType == DisplayType.Inventory && IGameState.Cards.IsActive(cardType));

            _infoButton.image.raycastTarget = !isLocked && displayType != DisplayType.OpenBox;

            int cardsAmount = IGameState.Cards.GetAmountOnCurrentLevel(cardType);
            int cardsRequire = IGameState.Cards.GetRequiredCardsOnCurrentLevel(cardType);

            _progressText.text = $"{cardsAmount}<size=80%>/{cardsRequire}";
            _progressSlider.value = (float)cardsAmount / cardsRequire;

            bool upgradeAvailable = cardsAmount >= cardsRequire;

            _progressFillImage.color = upgradeAvailable ? _upgradeColor : _progressColor;
            _progressIconImage.color = upgradeAvailable ? _upgradeColor : _progressColor;


        }


        private void OnInfoButtonClick()
        {
            cardsController.Item.ShowCardInfoWindow(CardType);
        }



    }
}
