using TMPro;
using UnityEngine;

namespace Gamebox
{
    public class ItemCounterView : MonoBehaviour
    {
        [SerializeField] private ItemKey _itemKey;
        [SerializeField] private TextMeshProUGUI _valueText;

        private void OnEnable()
        {
            IGameState.Items.Subscribe(_itemKey, OnItemChanged);
            Display();
        }

        private void OnDisable()
        {
            IGameState.Items.Dispose(_itemKey, OnItemChanged);
        }


        private void OnItemChanged() => Display();


        private void Display() => _valueText.text = IGameState.Items.Get(_itemKey).ToString();



    }
}

