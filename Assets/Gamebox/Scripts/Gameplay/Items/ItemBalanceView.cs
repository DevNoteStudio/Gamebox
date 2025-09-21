using TMPro;
using UnityEngine;

namespace Gamebox
{
    public class ItemBalanceView : MonoBehaviour
    {
        [SerializeField] private string _itemKey;
        [SerializeField] private TextMeshProUGUI _valueText;

        private void OnEnable()
        {
            IGameState.Items.OnChanged += OnItemsChanged;
            Display();
        }

        private void OnDisable()
        {
            IGameState.Items.OnChanged -= OnItemsChanged;
        }


        private void OnItemsChanged(string itemKey, int change) => Display();


        private void Display() => _valueText.text = IGameState.Items.Value(_itemKey).ToString();



    }
}

