using System;
using System.Collections.Generic;
using System.Linq;
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
            public string unlockNameLocalizationKey;
            public LeagueType requiredLeague;
            public int startAmount;
        }

        public string GetItemUnlockName(ItemKey itemKey) => Localization.GetLocalizedText($"{itemKey}_unlocked");


        public List<ItemKey> GetAllBoosters()
        {
            var allItemKeys = Enum.GetValues(typeof(ItemKey)).Cast<ItemKey>().ToList();
            return allItemKeys.FindAll(itemKey => itemKey.IsBooster());
        }


        public string GetItemName(ItemKey itemKey)
            => Localization.GetLocalizedText($"{itemKey}_name");

        public string GetItemTutorialDescription(ItemKey itemKey)
            => Localization.GetLocalizedText($"{itemKey}_tutor_desc");


        public bool TryGetItemForTutorial(out ItemKey resultItemKey)
        {
            foreach (var itemKey in _tutorialItems)
            {
                bool showTutorial = !IGameState.ItemTutorials.IsCompleted(itemKey)
                    && IGameState.Items.IsUnlocked(itemKey);

                if (showTutorial)
                {
                    resultItemKey = itemKey;
                    return true;
                }
            }
            resultItemKey = ItemKey.Coins;
            return false;
        }



    }

}


