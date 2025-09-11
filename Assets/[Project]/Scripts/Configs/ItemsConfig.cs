using System;
using UnityEngine;

namespace DevNote.Gamebox
{
    public partial class GameboxConfig // Items
    {
        [Serializable] private struct ItemData
        {
            public ItemType itemType;
            public Sprite iconSprite;
        }


        public Sprite GetItemIconSprite(ItemType itemType)
            => _items.Find(data => data.itemType == itemType).iconSprite;


    }

}


