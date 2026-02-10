using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class ShopBoxView : MonoBehaviour
    {
        [SerializeField] private ItemKey _boxItemKey;
        [SerializeField] private GameObject _amountMarker;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private TextMeshProUGUI _openButtonText;
        [SerializeField] private Button _openButton; public Button OpenButton => _openButton;
        [SerializeField] private Image _iconImage;

        private readonly Holder<ShopController> shopController = new();

        private void OnEnable()
        {
            IGameState.Items.Subscribe(_boxItemKey, OnItemChanged);
            Display();
        }

        private void OnDisable()
        {
            IGameState.Items.Dispose(_boxItemKey, OnItemChanged);
        }



        private void Start()
        {
            _openButton.onClick.AddListener(OnOpenButtonClick);
        }

        private void OnItemChanged() => Display();


        private void Display()
        {
            int amount = IGameState.Items.Get(_boxItemKey);

            _amountMarker.SetActive(amount > 0);
            _amountText.text = amount > 99 ? "99+" : amount.ToString();

            if (amount == 0)
            {
                int price = IConfigs.Gamebox.GetBoxPrice(_boxItemKey, out bool buyForGems);
                int spriteIndex = buyForGems ? 1 : 0;
                _openButtonText.text = $"<sprite={spriteIndex}>{price}";
            }
            else _openButtonText.text = Localization.GetLocalizedText("open");

            _iconImage.LoadSprite(AssetLoader.LoadItemSprite(_boxItemKey));
        }



        private void OnOpenButtonClick()
        {
            shopController.Item.ShowBoxWindow(_boxItemKey);
        }






    }

}
