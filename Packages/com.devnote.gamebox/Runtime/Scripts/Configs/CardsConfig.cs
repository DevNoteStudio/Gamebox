using System;
using System.Collections.Generic;
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
        }

        [SerializeField] private struct RarityColor
        { 
            public RarityType rarityType;
            public Color color;
        }


        public List<CardType> GetAllCardTypes()
        {
            var cardTypes = new List<CardType>(capacity: _cardDataList.Count);

            foreach (var cardData in _cardDataList)
                cardTypes.Add(cardData.cardType);

            return cardTypes;
        }



        public string GetCardShortDescription(CardType cardType)
        {
            return string.Empty;
        }


        public RarityType GetCardRarity(CardType cardType) 
            => _cardDataList.FindOrException(data => data.cardType == cardType).rarityType;

        public Color GetRarityColor(RarityType rarityType) 
            => _rarityColors.FindOrException(data => data.rarityType == rarityType).color;

        public Sprite GetCardIconSprite(CardType cardType)
            => _cardDataList.FindOrException(data => data.cardType == cardType).iconSprite;


        public int GetCardCellGemPrice(int cellIndex) => _cardCellGemPrices[cellIndex];


    }
}
