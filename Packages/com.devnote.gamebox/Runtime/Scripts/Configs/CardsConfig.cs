using System;
using System.Collections.Generic;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    public partial class GameboxConfig // Cards
    {
        [Serializable] private struct CardData
        {
            public CardType cardType;
            public RarityType rarityType;
            public Sprite iconSprite;
            public List<int> levelPowers;
        }

        [Serializable] private struct RarityColor
        { 
            public RarityType rarityType;
            public Color backgroundColor;
            public Color textColor;
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

        public RarityType GetCardRarity(CardType cardType) 
            => _cardDataList.FindOrException(data => data.cardType == cardType).rarityType;

        public Color GetRarityBackgroundColor(RarityType rarityType) 
            => _rarityColors.FindOrException(data => data.rarityType == rarityType).backgroundColor;

        public Color GetRarityTextColor(RarityType rarityType)
            => _rarityColors.FindOrException(data => data.rarityType == rarityType).textColor;


        public Sprite GetCardIconSprite(CardType cardType)
            => _cardDataList.FindOrException(data => data.cardType == cardType).iconSprite;

        public int GetCardUpgradePrice(int level)
        {
            return level * 50;
        }

        public int GetCardCellGemPrice(int cellIndex) => _cardCellGemPrices[cellIndex];


    }
}
