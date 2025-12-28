using System;
using DevNote;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class CardCellView : MonoBehaviour
    {
        [SerializeField] private CardView _card;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Button _moreButton;

        [Space]
        [SerializeField] private Sprite _availableIcon;
        [SerializeField] private Color _availableBackgroundColor;
        [SerializeField] private Color _availableIconColor;
        [Space]
        [SerializeField] private Sprite _lockedIcon;
        [SerializeField] private Color _lockedBackgroundColor;
        [SerializeField] private Color _lockedIconColor;

        private int _cellIndex;

        private readonly Holder<CardsController> cardsController = new();


        private void Start()
        {
            _moreButton.onClick.AddListener(OnMoreButtonClick);
        }


        public void Display(int cellIndex, bool showMoreButton)
        {
            _cellIndex = cellIndex;

            var cardType = IGameState.Cards.GetCellCard(cellIndex);

            bool isLocked = cardType == CardType.Locked;

            _iconImage.gameObject.SetActive(!showMoreButton);
            _backgroundImage.color = isLocked ? _lockedBackgroundColor : _availableBackgroundColor;
            _iconImage.sprite = isLocked ? _lockedIcon : _availableIcon;
            _iconImage.color = isLocked ? _lockedIconColor : _availableIconColor;

            bool isCard = cardType != CardType.Locked && cardType != CardType.Empty;

            _card.gameObject.SetActive(isCard);
            if (isCard) _card.Display(cardType, isCellPlaced: true);

            _moreButton.gameObject.SetActive(showMoreButton);
        }


        private void OnMoreButtonClick()
        {
            cardsController.Item.ShowUnlockCardCellWindow(_cellIndex);
        }


    }
}
