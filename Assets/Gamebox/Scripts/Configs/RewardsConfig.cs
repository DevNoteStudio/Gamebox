using System;
using System.Collections.Generic;

namespace Gamebox
{
    public partial class GameboxConfig // Rewards
    {
        [Serializable] private struct AdditionalLocationLevelRewardData
        {
            public int levelIndex;
            public string itemKey;
            public int amount;
        }

        [Serializable] private struct RewardForTotalLevelCompletionData
        {
            public int completedLevels;
            public string itemKey;
            public int amount;
        }

        [Serializable] private struct RewardsData
        {
            public int coinsForLevelComplete;
            public int coinsForNewStar;

            public List<RewardForTotalLevelCompletionData> rewardsForTotalLevelCompletion;

            public int GetCoinsForComplete(int newStars) => coinsForLevelComplete + coinsForNewStar * newStars;
        }


        public List<ItemPack> GetLevelRewards(int locationIndex, int levelIndex, 
            int newStars, bool isRepeatComplete, int totalCompletedLevels)
        {
            var rewards = new List<ItemPack>();

            var locationData = _locations[locationIndex];

            int coins = _rewards.coinsForLevelComplete + _rewards.coinsForNewStar * newStars;
            coins = (int)(coins * locationData.coinsMultiplier);

            rewards.Add(new ItemPack(IItemKey.Coins, coins));
            if (newStars > 0) rewards.Add(new ItemPack(IItemKey.Stars, newStars));

            foreach (var rewardData in _rewards.rewardsForTotalLevelCompletion)
            {
                if (totalCompletedLevels == rewardData.completedLevels)
                    Put(rewards, rewardData.itemKey, rewardData.amount);
            }

            foreach ( var additionalReward in locationData.additionalLevelRewards)
            {
                if (additionalReward.levelIndex == levelIndex)
                    Put(rewards, additionalReward.itemKey, additionalReward.amount);
            }

            return rewards;
        }

        private void Put(List<ItemPack> rewards, string itemKey, int value)
        {
            var index = rewards.FindIndex(itemPack => itemPack.itemKey == itemKey);
            if (index != -1)
                rewards[index] = new ItemPack(itemKey, rewards[index].amount + value);

            else rewards.Add(new ItemPack(itemKey, value));
        }


    }
}


