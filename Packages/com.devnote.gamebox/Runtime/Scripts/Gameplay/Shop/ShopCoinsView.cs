using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{

    public class ShopCoinsView : MonoBehaviour
    {
        [SerializeField] private int _coinsPackIndex;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _purchaseButton;

        private readonly Holder<ShopController> shopController = new();


        private void OnEnable() => Display();

        private void Start()
        {
            _purchaseButton.onClick.AddListener(OnPurchaseButtonClick);
        }

        private void Display()
        {
            _amountText.text = $"<sprite=0>{IConfigs.Gamebox.GetCoinsInsidePack(_coinsPackIndex)}";
            _priceText.text = $"<sprite=1>{IConfigs.Gamebox.GetCoinsPackPrice(_coinsPackIndex)}";
        }


        private void OnPurchaseButtonClick()
        {
            shopController.Item.TryPurchaseCoinsPack(_coinsPackIndex);
        }


    }
}
