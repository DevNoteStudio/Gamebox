using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevNote.Gamebox
{
    public class ItemWidgetView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _amountText;


        public void Display(ItemType itemType, int amount)
        {
            _iconImage.sprite = Configs.Gamebox.GetItemIconSprite(itemType);
            _amountText.text = amount.ToString();
        }


    }
}

