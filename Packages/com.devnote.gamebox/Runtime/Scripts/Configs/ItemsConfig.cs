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
            public int availableFromLevel;
        }


        public Sprite GetItemIconSprite(ItemKey itemKey)
            => _items.Find(data => data.itemKey == itemKey).iconSprite;


        public string GetItemTutorialName(ItemKey itemKey)
            => Localization.GetLocalizedText($"{itemKey}_tutor_name");

        public string GetItemTutorialDescription(ItemKey itemKey)
            => Localization.GetLocalizedText($"{itemKey}_tutor_desc");

        public bool ItemIsAvailable(ItemKey itemKey)
        {
            var itemData = _items.Find(data => data.itemKey == itemKey);
            bool hasItem = IGameState.Items.Has(itemKey);
            bool unlocked = IGameState.Levels.CurrentLevel >= itemData.availableFromLevel;
            return hasItem || unlocked;
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


