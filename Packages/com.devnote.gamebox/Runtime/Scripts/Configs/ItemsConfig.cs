using System;
using System.Collections.Generic;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    public partial class GameboxConfig // Items
    {
        [Serializable] private struct ItemData
        {
            public ItemKey itemKey;
            public Sprite iconSprite;
            public bool hasTutorial;
            public string unlockNameLocalizationKey;
            public LeagueType requiredLeague;
            public int startAmount;
        }

        public string GetItemUnlockName(ItemKey itemKey)
        {
            string key = _items.Find(data => data.itemKey == itemKey).unlockNameLocalizationKey;
            return Localization.GetLocalizedText(key);
        }

        public bool TryGetUnlockedItem(LeagueType leagueType, out ItemKey itemKey)
        {
            int index = _items.FindIndex(data => data.requiredLeague == leagueType);

            itemKey = index != -1 ? _items[index].itemKey : default;
            return index != -1;
        }


        public int GetItemStartAmount(ItemKey itemKey)
            => _items.Find(data => data.itemKey == itemKey).startAmount;


        public string GetItemName(ItemKey itemKey)
            => Localization.GetLocalizedText($"{itemKey}_name");

        public string GetItemTutorialDescription(ItemKey itemKey)
            => Localization.GetLocalizedText($"{itemKey}_tutor_desc");

        public bool ItemIsAvailable(ItemKey itemKey)
        {
            var currentLeague = IConfigs.Gamebox.GetLeagueType(IGameState.Rating.Value);
            var itemData = _items.Find(data => data.itemKey == itemKey);

            return currentLeague >= itemData.requiredLeague;
        }


        public bool TryGetItemForTutorial(out ItemKey resultItemKey)
        {
            foreach (var itemKey in IConfigs.Gamebox.GetTutorialItemKeys())
            {
                bool showTutorial = IGameState.ItemTutorials.IsCompleted(itemKey) == false
                    && IConfigs.Gamebox.ItemIsAvailable(itemKey);

                if (showTutorial)
                {
                    resultItemKey = itemKey;
                    return true;
                }
            }
            resultItemKey = ItemKey.Coins;
            return false;
        }



        public List<ItemKey> GetTutorialItemKeys()
        {
            var list = new List<ItemKey>();
            foreach (var itemData in _items)
                if (itemData.hasTutorial) list.Add(itemData.itemKey);

            return list;
        }


    }

}


