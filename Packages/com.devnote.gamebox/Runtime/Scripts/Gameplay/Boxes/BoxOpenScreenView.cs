using System.Collections.Generic;
using DevNote;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class BoxOpenScreenView : MonoBehaviour
    {
        [SerializeField] private Button _interactButton;
        [SerializeField] private BoxView _boxView;
        [SerializeField] private CardView _cardView;
        [SerializeField] private BoxItemAnimation _cardAnimation;
        [SerializeField] private BoxItemAnimation _boosterAnimation;

        private List<(CardType, int)> _cards;
        private List<(ItemKey, int)> _boosters;
        private int _cardIndex;
        private int _boosterIndex;

        private readonly Holder<BoxOpenController> boxOpenController = new();


        private void Start()
        {
            _interactButton.onClick.AddListener(OnInteractButtonClick);
        }

        public BoxOpenScreenView Display(ItemKey boxItemKey, 
            Dictionary<CardType, int> cards, Dictionary<ItemKey, int> boosters)
        {
            _cards = new List<(CardType, int)>();
            foreach (var card in cards)
                _cards.Add((card.Key, card.Value));

            _boosters = new List<(ItemKey, int)>();
            foreach (var booster in boosters)
                _boosters.Add((booster.Key, booster.Value));

            _boxView.Display(boxItemKey);

            _cardIndex = 0;
            _boosterIndex = 0;

            return this;
        }

        public void AnimateShow()
        {
            _interactButton.interactable = false;

            _boxView.AnimateShow(onCompleted: () => _interactButton.interactable = true);
            _cardView.gameObject.SetActive(false);
            _boosterAnimation.gameObject.SetActive(false);
            
        }



        private void OnInteractButtonClick()
        {
            _interactButton.interactable = false;

            int itemsLeft = (_cards.Count - _cardIndex) + (_boosters.Count - _boosterIndex) - 1;

            if (_cardIndex == 0)
                _boxView.AnimateOpen(itemsLeft, onOpened: AnimateShowNextItem);

            else
            {
                _boxView.AnimatePushCard(itemsLeft);
                AnimateShowNextItem();
            }
        }

        private void AnimateShowNextItem()
        {
            if (_cardIndex < _cards.Count)
            {
                var cardAmount = _cards[_cardIndex];

                _cardView.gameObject.SetActive(true);
                _cardView.Display(cardAmount.Item1, CardView.DisplayType.OpenBox);
                _cardAnimation.AnimateShowCard(cardAmount.Item1, cardAmount.Item2, _cardIndex,
                    onCompleted: () => _interactButton.interactable = true);

                _cardIndex++;
            }
            else if (_boosterIndex < _boosters.Count)
            {
                var boosterAmount = _boosters[_boosterIndex];

                _cardView.gameObject.SetActive(false);
                _boosterAnimation.gameObject.SetActive(true);
                _boosterAnimation.AnimateShowBooster(boosterAmount.Item1, boosterAmount.Item2,
                    onCompleted: () => _interactButton.interactable = true);

                _boosterIndex++;
            }
            else boxOpenController.Item.HideBoxOpenScreen();
        }




    }
}
