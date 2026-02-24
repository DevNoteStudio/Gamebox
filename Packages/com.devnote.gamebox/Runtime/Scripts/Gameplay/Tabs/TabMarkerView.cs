using DevNote;
using TMPro;
using UnityEngine;

namespace Gamebox
{
    public class TabMarkerView : MonoBehaviour
    {
        [SerializeField] private TabButtonView _tabButton;
        [SerializeField] private GameObject _markerObject;
        [SerializeField] private TextMeshProUGUI _amountText;




        private void OnEnable()
        {
            switch (_tabButton.TabType)
            {
                case TabType.Shop:
                    IGameState.Items.OnChanged += OnItemChanged;
                    break;

                case TabType.Cards:
                    IGameState.Cards.OnCardChanged += OnCardChanged;
                    break;
            }

            Display();
        }

        private void OnDisable()
        {
            switch (_tabButton.TabType)
            {
                case TabType.Shop:
                    IGameState.Items.OnChanged -= OnItemChanged;
                    break;

                case TabType.Cards:
                    IGameState.Cards.OnCardChanged -= OnCardChanged;
                    break;
            }
        }

        private void OnCardChanged(CardType cardType) => Display();

        private void OnItemChanged(ItemKey itemKey) => Display();


        private void Display()
        {
            int amount = _tabButton.TabType switch
            {
                TabType.Locations => GetLocationsMarkersAmount(),
                TabType.Shop => GetShopMarkersAmount(),
                TabType.Cards => GetCardsMarkersAmount(),
                _ => 0,
            };

            _markerObject.SetActive(amount > 0);
            _amountText.text = amount.ToString();
        }



        private int GetShopMarkersAmount()
        {
            int amount = 0;

            foreach (var itemKey in DevNote.Utils.GetEnumTypes<ItemKey>())
            {
                if (itemKey.IsBox())
                    amount += IGameState.Items.Get(itemKey);
            }
                
            return amount;
        }

        private int GetLocationsMarkersAmount()
        {
            return 0;
        }

        private int GetCardsMarkersAmount()
        {
            int amount = 0;
            foreach (var cardType in DevNote.Utils.GetEnumTypes<CardType>())
            {
                if (IGameState.Cards.UpgradeAvailable(cardType) || IGameState.Cards.IsNew(cardType))
                    amount++;
            }

            return amount;
        }


    }
}
