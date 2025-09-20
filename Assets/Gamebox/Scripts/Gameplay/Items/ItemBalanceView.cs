using TMPro;
using UnityEngine;

namespace Gamebox
{
    public class ItemBalanceView : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private TextMeshProUGUI _valueText;

        private void OnEnable()
        {
            GameState.Items.OnChanged += OnItemsChanged;
            Display();
        }

        private void OnDisable()
        {
            GameState.Items.OnChanged -= OnItemsChanged;
        }


        private void OnItemsChanged(ItemType itemType, int change) => Display();


        private void Display() => _valueText.text = GameState.Items.Value(_itemType).ToString();



    }
}

