using DevNote;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class CardsScreenView : MonoBehaviour
    {
        [SerializeField] private HorizontalLayoutGroup _activeCardsGroup;
        [SerializeField] private RectTransform _inventoryContainer;


        private const float PORTRAIT_CARDS_SPACING = -45f;
        private const float LANDSCAPE_CARDS_SPACING = 25f;


        public void Display()
        {
            if (_inventoryContainer.childCount == 0)
                CreateCards();
        }


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


        private void CreateCards()
        {
            foreach (var cardType in IConfigs.Gamebox.GetAllCardTypes())
            {
                Instantiate(IConfigs.GetViewPrefab<CardView>(), _inventoryContainer)
                    .Display(cardType, isCellPlaced: false);
            }
        }



        private void OnCardCellChanged(int cellIndex)
        {
            
        }

        private void OnCardChanged(CardType cardType)
        {
            
        }

        

        private void OnOrientationChanged()
        {
            _activeCardsGroup.spacing = ScreenState.Orientation == Orientation.Portrait ?
                PORTRAIT_CARDS_SPACING : LANDSCAPE_CARDS_SPACING;

            Debug.Log("change");
        }



    }
}
