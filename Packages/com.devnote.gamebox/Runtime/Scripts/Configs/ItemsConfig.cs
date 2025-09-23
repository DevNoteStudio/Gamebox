using System;
using UnityEngine;

namespace Gamebox
{
    public partial class GameboxConfig // Items
    {
        [Serializable] private struct ItemData
        {
            public string itemKey;
            public Sprite iconSprite;
        }


        public Sprite GetItemIconSprite(string itemKey)
            => _items.Find(data => data.itemKey == itemKey).iconSprite;


    }

}


