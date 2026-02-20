using System.Collections.Generic;
using DevNote;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class CardsScreenView : MonoBehaviour
    {
        [SerializeField] private HorizontalLayoutGroup _activeCardsGroup;
        [SerializeField] private RectTransform _inventoryContainer;
        [SerializeField] private List<CardCellView> _cells;
        [field: SerializeField] public ScrollRect ScrollRect { get; private set; }

        private List<CardView> _inventoryCards;

        private const float PORTRAIT_CARDS_SPACING = -45f;
        private const float LANDSCAPE_CARDS_SPACING = 25f;

        
        private void OnEnable()
        {
            ScreenState.OnOrientationChanged += OnOrientationChanged;
            IGameState.Cards.OnCardChanged += OnCardChanged;
            IGameState.Cards.OnCardCellChanged += OnCardCellChanged;
            OnOrientationChanged();
            Display();
        }

        private void OnDisable()
        {
            ScreenState.OnOrientationChanged -= OnOrientationChanged;
            IGameState.Cards.OnCardChanged -= OnCardChanged;
            IGameState.Cards.OnCardCellChanged -= OnCardCellChanged;
        }

        private void Display()
        {
            if (_inventoryContainer.childCount == 0)
                CreateCards();

            else
            {
                for (int i = 0; i < _inventoryCards.Count; i++)
                {
                    var card = _inventoryCards[i];
                    card.Display(card.CardType, CardView.DisplayType.Inventory);
                }  
            }
            
            SortCards();

            bool moreButtonShown = false;
            for (int i = 0; i < _cells.Count; i++)
            {
                bool isLocked = IGameState.Cards.GetCellCard(i) == CardType.Locked;

                _cells[i].Display(i, showMoreButton: isLocked && !moreButtonShown);

                if (isLocked) moreButtonShown = true;
            }
                

        }


        public CardView GetInventoryCard(CardType cardType) 
            => _inventoryCards.FindOrException(card => card.CardType == cardType);


        private void CreateCards()
        {
            _inventoryCards = new List<CardView>();

            foreach (var cardType in IConfigs.Gamebox.GetAllCardTypes())
            {
                var card = Instantiate(IConfigs.GetViewPrefab<CardView>(), _inventoryContainer);
                card.Display(cardType, CardView.DisplayType.Inventory);
                _inventoryCards.Add(card);
            }
        }


        private void SortCards()
        {
            _inventoryCards.Sort((a, b) =>
            {
                var rarityA = IConfigs.Gamebox.GetCardRarity(a.CardType);
                var rarityB = IConfigs.Gamebox.GetCardRarity(b.CardType);

                var hasA = IGameState.Cards.Has(a.CardType);
                var hasB = IGameState.Cards.Has(b.CardType);

                if (hasA != hasB) return hasB.CompareTo(hasA);
                else return rarityB.CompareTo(rarityA);
            });

            for (int i = 0; i < _inventoryCards.Count; i++)
                _inventoryCards[i].transform.SetSiblingIndex(i);
        }



        private void OnCardCellChanged(int cellIndex) => Display();


        private void OnCardChanged(CardType cardType)
        {
            _inventoryCards.Find(card => card.CardType == cardType)
                .Display(cardType, CardView.DisplayType.Inventory);
        }

        

        private void OnOrientationChanged()
        {
            _activeCardsGroup.spacing = ScreenState.Orientation == Orientation.Portrait ?
                PORTRAIT_CARDS_SPACING : LANDSCAPE_CARDS_SPACING;
        }



    }
}
