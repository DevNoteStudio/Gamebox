using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class ItemCounterView : MonoBehaviour
    {
        [SerializeField] private ItemKey _itemKey;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private Button _moreButton;

        private readonly Holder<RollupController> rollupController = new();
        private readonly Holder<ShopController> shopController = new();


        private void Start()
        {
            if (_moreButton != null)
                _moreButton.onClick.AddListener(OnMoreButtonClick);
        }

        

        private void OnEnable()
        {
            IGameState.Items.Subscribe(_itemKey, OnItemChanged);
            Display(IGameState.Items.Get(_itemKey));

            if (_itemKey == ItemKey.Coins)
                rollupController.Item.AddCoinsRollupTarget(this);
        }

        private void OnDisable()
        {
            IGameState.Items.Dispose(_itemKey, OnItemChanged);

            if (_itemKey == ItemKey.Coins)
                rollupController.Item.RemoveCoinsRollupTarget(this);
        }

        public void Display(int amount) => _valueText.text = amount.ToString();


        private void OnMoreButtonClick() => shopController.Item.GoToCurrency();
        private void OnItemChanged() => Display(IGameState.Items.Get(_itemKey));



    }
}

