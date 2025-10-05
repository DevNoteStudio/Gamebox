using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class ItemWidgetView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private TMP_ColorGradient _commonTextGradient;
        [SerializeField] private TMP_ColorGradient _increasedTextGradient;


        public void Display(ItemKey itemKey, int amount)
        {
            _iconImage.sprite = IConfigs.Gamebox.GetItemIconSprite(itemKey);
            _amountText.text = amount.ToString();
            _amountText.colorGradientPreset = _commonTextGradient;
        }


        public void AnimateIncrease(int newValue)
        {
            TweenHub.PopUp(_amountText.transform).Attach(gameObject);
            _amountText.text = newValue.ToString();
            _amountText.colorGradientPreset = _increasedTextGradient;
        }


    }
}

