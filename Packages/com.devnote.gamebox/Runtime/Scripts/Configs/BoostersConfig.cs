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
            public UnlockKey unlockKey;
            public int price;
            public int startAmount;
        }


        public int GetBoosterStartAmount(ItemKey boosterItemKey)
            => _boosterData.FindOrException((data) => data.itemKey == boosterItemKey).startAmount;

        public int GetBoosterPrice(ItemKey boosterItemKey)
            => _boosterData.FindOrException((data) => data.itemKey == boosterItemKey).price;

        public ItemKey GetBoosterKey(UnlockKey unlockKey)
            => _boosterData.FindOrException((data) => data.unlockKey == unlockKey).itemKey;


        public string GetBoosterHint(ItemKey boosterItemKey)
            => Localization.GetLocalizedText($"{boosterItemKey}_hint");

        public List<ItemKey> GetAllBoosterKeys()
        {
            var boosterKeys = new List<ItemKey>();
            foreach (var boosterData in _boosterData)
                boosterKeys.Add(boosterData.itemKey);

            return boosterKeys;
        }

        public UnlockKey GetBoosterUnlockKey(ItemKey boosterItemKey)
            => _boosterData.FindOrException(data => data.itemKey == boosterItemKey).unlockKey;



        public bool TryGetUnlockedBoosterKey(int level, out ItemKey boosterItemKey)
        {
            var allBoosterKeys = GetAllBoosterKeys();

            foreach (var boosterKey in allBoosterKeys)
            {
                var unlockKey = GetBoosterUnlockKey(boosterKey);
                if (GetUnlockLevel(unlockKey) == level)
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
