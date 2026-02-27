using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamebox
{
    public partial class GameboxConfig // Rewards
    {
        [Serializable] private struct AdditionalLocationLevelRewardData
        {
            public int levelIndex;
            public ItemKey itemKey;
            public int amount;
        }

        [Serializable] private struct RewardForTotalLevelCompletionData
        {
            public int completedLevels;
            public ItemKey itemKey;
            public int amount;
        }

        [Serializable] private struct RewardsData
        {
            public int coinsForLevelComplete;
            public int coinsForNewStar;
            [Space]
            public int ratingForLevelComplete;
            public int ratingForNewStar;
            [Space]
            public List<RewardForTotalLevelCompletionData> rewardsForTotalLevelCompletion;
        }


        public List<ItemPack> GetLevelRewards(int locationIndex, int levelIndex, 
            int newStars, bool isRepeatComplete, int completedLevels, bool isFirstComplete)
        {
            var rewards = new List<ItemPack>();

            //var locationData = _locations[locationIndex];

            int coins = _rewards.coinsForLevelComplete + _rewards.coinsForNewStar * newStars;

            rewards.Add(new ItemPack(ItemKey.Coins, coins));

            if (isFirstComplete)
            {
                foreach (var rewardData in _rewards.rewardsForTotalLevelCompletion)
                {
                    if (completedLevels == rewardData.completedLevels)
                        Put(rewards, rewardData.itemKey, rewardData.amount);
                }
            }

            return rewards;
        }

        private void Put(List<ItemPack> rewards, ItemKey itemKey, int value)
        {
            var index = rewards.FindIndex(itemPack => itemPack.itemKey == itemKey);
            if (index != -1)
                rewards[index] = new ItemPack(itemKey, rewards[index].amount + value);

            else rewards.Add(new ItemPack(itemKey, value));
        }


    }
}


