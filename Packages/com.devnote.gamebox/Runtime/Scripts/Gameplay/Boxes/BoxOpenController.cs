using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    public class BoxRewardData
    {
        public Dictionary<CardType, int> cards;
        public Dictionary<ItemKey, int> boosters;
    }

    public class BoxOpenController
    {
        public event Action OnBoxOpenFinished;

        private BoxRewardData _nextBoxReward;

        private readonly Viewer<BoxOpenScreenView> boxOpenScreenViewer;


        public BoxOpenController()
        {
            boxOpenScreenViewer = new(IConfigs.GetViewPrefab<BoxOpenScreenView>());
        }


        private readonly List<int> SAME_RARE_AMOUNTS = new List<int>()
        {
            4, // 0-4 : 1 card
            14, // 5-14 : 2 cards
            29, // 15-29 : 3 cards
            49, // 30-49 : 4 cards
            // more : 5 cards
        };

        private readonly List<int> SAME_BOOSTER_AMOUNTS = new List<int>()
        {
            3, // 0-3 : 1 booster
            8, // 4-8 : 2 boosters
            // more : 3 boosters
        };


        public bool TryBuyBox(ItemKey boxItemKey, int amount)
        {
            int price = IConfigs.Gamebox.GetBoxPrice(boxItemKey, out bool buyForGems) * amount;
            ItemKey currency = buyForGems ? ItemKey.Gems : ItemKey.Coins;

            if (IGameState.Items.Get(currency) >= price)
            {
                IGameState.Items.Spend(currency, price);
                IGameState.Items.Add(boxItemKey, amount);
                return true;
            }
            else return false;
        }


        public void SetNextBoxReward(BoxRewardData boxRewardData) => _nextBoxReward = boxRewardData;

        public bool TryOpenBox(ItemKey boxItemKey, int boxAmount)
        {
            if (IGameState.Items.Get(boxItemKey) < boxAmount) 
                return false; 

            IGameState.Items.Spend(boxItemKey, boxAmount);

            var boxReward = _nextBoxReward != null ? _nextBoxReward : GenerateBoxReward(boxItemKey, boxAmount);
            _nextBoxReward = null;

            foreach (var cardAmount in boxReward.cards)
                IGameState.Cards.IncreaseAmount(cardAmount.Key, cardAmount.Value);

            foreach (var boosterAmount in boxReward.boosters)
                IGameState.Items.Add(boosterAmount.Key, boosterAmount.Value);

            boxOpenScreenViewer.ShowExpand(UI.Container)
                .Display(boxItemKey, boxReward.cards, boxReward.boosters).AnimateShow();

            return true;
        }

        public void HideBoxOpenScreen()
        {
            boxOpenScreenViewer.Hide();
            OnBoxOpenFinished?.Invoke();
        }



        private BoxRewardData GenerateBoxReward(ItemKey boxItemKey, int boxAmount)
        {
            Dictionary<CardType, int> GenerateCards(Vector2Int minMax, int amount, RarityType rarityType)
            {
                if (minMax.x == 0 && minMax.y == 0)
                    return new Dictionary<CardType, int>();

                int totalCardAmount = 0;
                for (int i = 0; i < boxAmount; i++)
                    totalCardAmount += UnityEngine.Random.Range(minMax.x, minMax.y + 1);

                int cardTypesAmount = 1;
                for (int i = 0; i < SAME_RARE_AMOUNTS.Count; i++)
                {
                    if (totalCardAmount > SAME_RARE_AMOUNTS[i]) 
                        cardTypesAmount++;
                }

                var allCardTypes = IConfigs.Gamebox.GetAllCardsSameRarity(rarityType);
                allCardTypes.Shuffle();

                var selectedCardTypes = new List<CardType>();
                for (int i = 0; i < cardTypesAmount && i < allCardTypes.Count; i++)
                    selectedCardTypes.Add(allCardTypes[i]);

                var result = new Dictionary<CardType, int>();
                for (int i = 0; i < totalCardAmount; i++)
                {
                    var cardType = selectedCardTypes.GetRandom();

                    if (result.ContainsKey(cardType)) result[cardType]++;
                    else result[cardType] = 1;
                }

                return result;
            }

            Dictionary<ItemKey, int> GenerateBoosters(Vector2Int minMax, int amount)
            {
                if (minMax.x == 0 && minMax.y == 0) 
                    return new Dictionary<ItemKey, int>();


                int totalAmount = 0;
                for (int i = 0; i < boxAmount; i++)
                    totalAmount += UnityEngine.Random.Range(minMax.x, minMax.y + 1);

                int boosterTypesAmount = 1;
                for (int i = 0; i < SAME_BOOSTER_AMOUNTS.Count; i++)
                {
                    if (totalAmount > SAME_BOOSTER_AMOUNTS[i])
                        boosterTypesAmount++;
                }

                var allBoosterTypes = IConfigs.Gamebox.GetAllBoosterKeys();
                ItemKey firstBooster = allBoosterTypes[0];
                allBoosterTypes.RemoveAll(boosterItemKey => !IGameState.Items.IsUnlocked(boosterItemKey));

                if (allBoosterTypes.Count == 0)
                    allBoosterTypes.Add(firstBooster);

                allBoosterTypes.Shuffle();

                var selectedBoosterTypes = new List<ItemKey>();
                for (int i = 0; i < boosterTypesAmount && i < allBoosterTypes.Count; i++)
                    selectedBoosterTypes.Add(allBoosterTypes[i]);

                var result = new Dictionary<ItemKey, int>();
                for (int i = 0; i < totalAmount; i++)
                {
                    var boosterType = selectedBoosterTypes.GetRandom();

                    if (result.ContainsKey(boosterType)) result[boosterType]++;
                    else result[boosterType] = 1;
                }

                return result;
            }

            var boxData = IConfigs.Gamebox.GetBoxData(boxItemKey);

            return new BoxRewardData
            {
                cards = new Dictionary<CardType, int>()
                {
                    GenerateCards(boxData.minMaxCommonCards, boxAmount, RarityType.Common),
                    GenerateCards(boxData.minMaxRareCards, boxAmount, RarityType.Rare),
                    GenerateCards(boxData.minMaxEpicCards, boxAmount, RarityType.Epic),
                    GenerateCards(boxData.minMaxLegendaryCards, boxAmount, RarityType.Legendary),
                },
                boosters = GenerateBoosters(boxData.minMaxBoosters, boxAmount),
            };
            
        }




    }
}
