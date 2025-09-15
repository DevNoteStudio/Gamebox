using System;
using System.Collections.Generic;

namespace DevNote.Gamebox
{
    public partial class GameboxConfig // Rewards
    {
        [Serializable] private struct AdditionalLocationLevelRewardData
        {
            public int levelIndex;
            public ItemType itemType;
            public int amount;
        }

        [Serializable] private struct RewardForTotalLevelCompletionData
        {
            public int completedLevels;
            public ItemType itemType;
            public int amount;
        }

        [Serializable] private struct RewardsData
        {
            public int coinsForLevelComplete;
            public int coinsForNewStar;

            public List<RewardForTotalLevelCompletionData> rewardsForTotalLevelCompletion;

            public int GetCoinsForComplete(int newStars) => coinsForLevelComplete + coinsForNewStar * newStars;
        }


        public List<(ItemType, int)> GetLevelRewards(int locationIndex, int levelIndex, 
            int newStars, bool isRepeatComplete, int totalCompletedLevels)
        {
            var rewards = new List<(ItemType, int)>();

            var locationData = _locations[locationIndex];

            int coins = _rewards.coinsForLevelComplete + _rewards.coinsForNewStar * newStars;
            coins = (int)(coins * locationData.coinsMultiplier);

            rewards.Add((ItemType.Coins, coins));
            if (newStars > 0) rewards.Add((ItemType.Stars, newStars));

            foreach (var rewardData in _rewards.rewardsForTotalLevelCompletion)
            {
                if (totalCompletedLevels == rewardData.completedLevels)
                    Put(rewards, rewardData.itemType, rewardData.amount);
            }

            foreach ( var additionalReward in locationData.additionalLevelRewards)
            {
                if (additionalReward.levelIndex == levelIndex)
                    Put(rewards, additionalReward.itemType, additionalReward.amount);
            }

            return rewards;
        }

        private void Put(List<(ItemType, int)> rewards, ItemType itemType, int value)
        {
            var index = rewards.FindIndex(element => element.Item1 == itemType);
            if (index != -1)
                rewards[index] = new (itemType, rewards[index].Item2 + value);

            else rewards.Add(new (itemType, value));
        }


    }
}


