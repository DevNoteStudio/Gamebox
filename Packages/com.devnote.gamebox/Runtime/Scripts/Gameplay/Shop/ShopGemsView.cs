using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{

    public class ShopGemsView : MonoBehaviour
    {
        [SerializeField] private ProductKey _gemProductKey;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _purchaseButton;

        private readonly Holder<IPurchase> purchase = new();
        private readonly Holder<ShopController> shopController = new();


        private void OnEnable() => Display();


        private void Start()
        {
            _purchaseButton.onClick.AddListener(OnPurchaseButtonClick);
        }

        private void Display()
        {
            _amountText.text = $"<sprite=1>{IConfigs.Gamebox.GetGemsInsidePack(_gemProductKey)}";
            _priceText.text = purchase.Item.GetPriceString(_gemProductKey);
        }


        private void OnPurchaseButtonClick()
        {
            shopController.Item.TryPurchaseGemPack(_gemProductKey);
        }


    }
}
