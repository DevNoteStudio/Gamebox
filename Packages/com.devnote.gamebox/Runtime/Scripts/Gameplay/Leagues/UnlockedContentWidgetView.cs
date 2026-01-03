using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class UnlockedContentWidgetView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _descriptionText;


        public void DisplayUnlockedLocation(int locationIndex)
        {
            _iconImage.sprite = IConfigs.Gamebox.GetLocationPreviewSprite(locationIndex);
            _descriptionText.text = Localization.GetLocalizedText("new_location");
        }

        public void DisplayUnlockedItem(ItemKey itemKey)
        {
            _iconImage.LoadSprite(AssetLoader.LoadItemSprite(itemKey));
            _descriptionText.text = IConfigs.Gamebox.GetItemUnlockName(itemKey);
        }



    }
}
