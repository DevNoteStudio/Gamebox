using System.Collections.Generic;
using DevNote;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class CardsScreenView : MonoBehaviour
    {
        [SerializeField] private HorizontalLayoutGroup _activeCardsGroup;
        [SerializeField] private RectTransform _inventoryContainer;
        [SerializeField] private List<CardCellView> _cells;

        private List<CardView> _inventoryCards;

        private const float PORTRAIT_CARDS_SPACING = -45f;
        private const float LANDSCAPE_CARDS_SPACING = 25f;

        
        private void OnEnable()
        {
            ScreenState.OnOrientationChanged += OnOrientationChanged;
            IGameState.Cards.OnCardChanged += OnCardChanged;
            IGameState.Cards.OnCardCellChanged += OnCardCellChanged;
            OnOrientationChanged();
        }

        private void OnDisable()
        {
            ScreenState.OnOrientationChanged -= OnOrientationChanged;
            IGameState.Cards.OnCardChanged -= OnCardChanged;
            IGameState.Cards.OnCardCellChanged -= OnCardCellChanged;
        }

        public void Display()
        {
            if (_inventoryContainer.childCount == 0)
                CreateCards();

            bool moreButtonShown = false;
            for (int i = 0; i < _cells.Count; i++)
            {
                bool isLocked = IGameState.Cards.GetCellCard(i) == CardType.Locked;

                _cells[i].Display(i, showMoreButton: isLocked && !moreButtonShown);

                if (isLocked) moreButtonShown = true;
            }
                

        }




        private void CreateCards()
        {
            _inventoryCards = new List<CardView>();

            foreach (var cardType in IConfigs.Gamebox.GetAllCardTypes())
            {
                var card = Instantiate(IConfigs.GetViewPrefab<CardView>(), _inventoryContainer);
                card.Display(cardType, isCellPlaced: false);
                _inventoryCards.Add(card);
            }
        }



        private void OnCardCellChanged(int cellIndex) => Display();


        private void OnCardChanged(CardType cardType)
        {
            _inventoryCards.Find(card => card.CardType == cardType).Display(cardType, isCellPlaced: false);
        }

        

        private void OnOrientationChanged()
        {
            _activeCardsGroup.spacing = ScreenState.Orientation == Orientation.Portrait ?
                PORTRAIT_CARDS_SPACING : LANDSCAPE_CARDS_SPACING;
        }



    }
}
