using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevNote.Gamebox
{
    public partial class GameboxConfig // Rewards
    {
        [Serializable] private struct AdditionalRewardData
        {
            public int levelIndex;
            public ItemType itemType;
            public int amount;
        }

        [Serializable] private struct BaseLevelRewardData
        {
            [SerializeField] private int _coinsForComplete;
            [SerializeField] private int _coinsForStar;

            public int GetValue(int newStars) => _coinsForComplete + _coinsForStar * newStars;
        }


        public List<(ItemType, int)> GetLevelRewards(int locationIndex, int levelIndex, 
            int newStars, bool isRepeatComplete, int totalCompletedLevels)
        {
            var rewards = new List<(ItemType, int)>();

            var locationData = _locations[locationIndex];
            int rewardCoins = (int)(_baseLevelReward.GetValue(newStars) * locationData.coinsMultiplier);

            rewards.Add((ItemType.Coins, rewardCoins));
            if (newStars > 0) rewards.Add((ItemType.Stars, newStars));
            
            foreach ( var additionalReward in locationData.additionalLevelRewards )
            {
                if (additionalReward.levelIndex == levelIndex)
                    rewards.Add((additionalReward.itemType, additionalReward.amount));
            }

            return rewards;
        }




    }
}


