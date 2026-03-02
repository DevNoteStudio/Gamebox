using System;
using System.Collections.Generic;
using DevNote;

namespace Gamebox
{
    public partial class GameboxConfig // Boosters
    {
        [Serializable] private struct BoosterData
        {
            public ItemKey itemKey;
            public ContentKey contentKey;
            public int price;
        }


        public int GetBoosterPrice(ItemKey boosterItemKey)
            => _boosterData.FindOrException((data) => data.itemKey == boosterItemKey).price;

        public ItemKey GetBoosterKey(ContentKey contentKey)
            => _boosterData.FindOrException((data) => data.contentKey == contentKey).itemKey;


        public string GetBoosterHint(ItemKey boosterItemKey)
            => Localization.GetLocalizedText($"{boosterItemKey}_hint");

        public List<ItemKey> GetAllBoosterKeys()
        {
            var boosterKeys = new List<ItemKey>();
            foreach (var boosterData in _boosterData)
                boosterKeys.Add(boosterData.itemKey);

            return boosterKeys;
        }

        public bool TryGetUnlockedBoosterKey(int level, out ItemKey boosterItemKey)
        {
            var allBoosterKeys = GetAllBoosterKeys();

            foreach (var boosterKey in allBoosterKeys)
            {
                var contentKey = boosterKey.GetContentKey();
                if (ContentPipeline.IsNow(contentKey))
                {
                    boosterItemKey = boosterKey;
                    return true;
                }
            }

            boosterItemKey = default;
            return false;
        }


    }
}
