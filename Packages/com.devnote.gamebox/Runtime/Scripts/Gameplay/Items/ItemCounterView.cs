using DevNote;
using TMPro;
using UnityEngine;

namespace Gamebox
{
    public class ItemCounterView : MonoBehaviour
    {
        [SerializeField] private ItemKey _itemKey;
        [SerializeField] private TextMeshProUGUI _valueText;

        private readonly Holder<CoinsRollupController> coinsRollupController = new();


        private void OnEnable()
        {
            IGameState.Items.Subscribe(_itemKey, OnItemChanged);
            Display(IGameState.Items.Get(_itemKey));

            if (_itemKey == ItemKey.Coins)
                coinsRollupController.Item.AddCoinsRollupTarget(this);
        }

        private void OnDisable()
        {
            IGameState.Items.Dispose(_itemKey, OnItemChanged);

            if (_itemKey == ItemKey.Coins)
                coinsRollupController.Item.RemoveCoinsRollupTarget(this);
        }


        private void OnItemChanged() => Display(IGameState.Items.Get(_itemKey));


        public void Display(int amount) => _valueText.text = amount.ToString();



    }
}

