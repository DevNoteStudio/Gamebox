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
            public bool hasTutorial;
            public bool showInRewards;
            public Sprite iconSprite;
        }


        public Sprite GetItemIconSprite(ItemKey itemKey)
            => _items.Find(data => data.itemKey == itemKey).iconSprite;


        public string GetItemTutorialName(ItemKey itemKey)
            => Localization.GetLocalizedText($"{itemKey}_tutor_name");

        public string GetItemTutorialDescription(ItemKey itemKey)
            => Localization.GetLocalizedText($"{itemKey}_tutor_desc");

        public bool IsRewardItem(ItemKey itemKey) 
            => _items.Find(data => data.itemKey == itemKey).showInRewards;

        public List<ItemKey> GetTutorialItemKeys()
        {
            var list = new List<ItemKey>();
            foreach (var itemData in _items)
                if (itemData.hasTutorial) list.Add(itemData.itemKey);

            return list;
        }


    }

}


