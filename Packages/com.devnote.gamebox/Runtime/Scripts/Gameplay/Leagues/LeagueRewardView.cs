using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class LeagueRewardView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _labelText;


        public void DisplayRewardItem(ItemKey itemKey, int amount)
        {
            _iconImage.LoadSprite(AssetLoader.LoadItemSprite(itemKey));
            _labelText.text = amount.ToString();
        }

        public void DisplayRewardBox(ItemKey itemKey)
        {
            _iconImage.LoadSprite(AssetLoader.LoadItemSprite(itemKey));
            _labelText.text = IConfigs.Gamebox.GetBoxName(itemKey);
        }


        public void DisplayUnlockedBooster(ItemKey itemKey)
        {
            _iconImage.LoadSprite(AssetLoader.LoadItemSprite(itemKey));
        }

        public void DisplayUnlockedLocation(int locationIndex)
        {
            _iconImage.LoadSprite(AssetLoader.LoadLocationSprite(locationIndex));
        }

    }



    
}
