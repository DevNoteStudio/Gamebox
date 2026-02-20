using System;
using System.Collections.Generic;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    public partial class GameboxConfig // Cards
    {
        [Serializable] private struct CardPrice
        {
            public RarityType rarityType;
            public List<int> levelPrices;
        }

        [Serializable] private struct CardData
        {
            public CardType cardType;
            public RarityType rarityType;
            public List<int> levelPowers;
        }


        public List<CardType> GetAllCardTypes()
        {
            var cardTypes = new List<CardType>(capacity: _cardDataList.Count);

            foreach (var cardData in _cardDataList)
                cardTypes.Add(cardData.cardType);

            return cardTypes;
        }



        public string GetCardShortDescription(CardType cardType, int level = -1)
        {
            if (level == -1)
                level = IGameState.Cards.GetLevel(cardType);

            int power = GetCardPower(cardType, level);
            return Localization.GetLocalizedText($"{cardType}_short").Replace("{VALUE}", power.ToString());
        }

        public int GetCardPower(CardType cardType, int level = -1)
        {
            if (level == -1) 
                level = IGameState.Cards.GetLevel(cardType);

            return _cardDataList.FindOrException(data => data.cardType == cardType)
                .levelPowers[level - 1];
        }

        public string GetCardName(CardType cardType) => Localization.GetLocalizedText($"{cardType}_name");
        public string GetRarityName(RarityType rarityType) => Localization.GetLocalizedText($"{rarityType}_card");


        public RarityType GetCardRarity(CardType cardType) 
            => _cardDataList.FindOrException(data => data.cardType == cardType).rarityType;

        public int GetCardUpgradePrice(CardType cardType, int level)
        {
            var rarity = GetCardRarity(cardType);
            return _cardUpgradePrices.Find(cardPrice => cardPrice.rarityType == rarity).levelPrices[level - 1];
        }

        public int GetCardCellGemPrice(int cellIndex) => _cardCellGemPrices[cellIndex];


    }
}
