using System;
using System.Collections.Generic;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    public partial class GameboxConfig // Shop
    {

        [Serializable] public struct ShopBoxData
        {
            public ItemKey boxItemKey;
            public bool buyForGems;
            public int price;
            public Vector2Int minMaxCommonCards;
            public Vector2Int minMaxRareCards;
            public Vector2Int minMaxEpicCards;
            public Vector2Int minMaxLegendaryCards;
            public Vector2Int minMaxBoosters;
        }


        public string GetBoxDescription(ItemKey boxItemKey)
        {
            string GetDescriptionPoint(Vector2Int minMax, int spriteIndex, string localizationKey) 
                => $"<sprite={spriteIndex}>{minMax.x}-{minMax.y} {Localization.GetLocalizedText(localizationKey)}";

            var boxData = _shopBoxes.FindOrException(data => data.boxItemKey == boxItemKey);
            string text = $"{Localization.GetLocalizedText("contains")}:";

            if (boxData.minMaxCommonCards != Vector2Int.zero)
                text += "\n" + GetDescriptionPoint(boxData.minMaxCommonCards, spriteIndex: 3, "box_common_cards");

            if (boxData.minMaxRareCards != Vector2Int.zero)
                text += "\n" + GetDescriptionPoint(boxData.minMaxRareCards, spriteIndex: 4, "box_rare_cards");

            if (boxData.minMaxEpicCards != Vector2Int.zero)
                text += "\n" + GetDescriptionPoint(boxData.minMaxEpicCards, spriteIndex: 5, "box_epic_cards");

            if (boxData.minMaxLegendaryCards != Vector2Int.zero)
                text += "\n" + GetDescriptionPoint(boxData.minMaxLegendaryCards, spriteIndex: 6, "box_legendary_cards");

            if (boxData.minMaxBoosters != Vector2Int.zero)
                text += "\n" + GetDescriptionPoint(boxData.minMaxBoosters, spriteIndex: 2, "box_boosters");

            return text;
        }

        public ShopBoxData GetBoxData(ItemKey boxItemKey)
            => _shopBoxes.FindOrException(data => data.boxItemKey == boxItemKey);

        public List<CardType> GetAllCardsSameRarity(RarityType rarityType)
        {
            var cardTypes = new List<CardType>();
            var cardDataList = _cardDataList.FindAll((data) => data.rarityType == rarityType);

            foreach (var cardData in cardDataList)
                cardTypes.Add(cardData.cardType);

            return cardTypes;
        }



        public int GetBoxPrice(ItemKey boxItemKey, out bool buyForGems)
        {
            var boxData = _shopBoxes.FindOrException(data => data.boxItemKey == boxItemKey);
            buyForGems = boxData.buyForGems;
            return boxData.price;
        }

        public int GetGemsInsidePack(ProductKey gemProductKey)
        {
            int index = gemProductKey switch
            {
                ProductKey.Gems1 => 0,
                ProductKey.Gems2 => 1,
                ProductKey.Gems3 => 2,
                ProductKey.Gems4 => 3,
                ProductKey.Gems5 => 4,
                ProductKey.Gems6 => 5,
                _ => -1
            };

            return _gemsInsideShopPacks[index];
        }

        public int GetCoinsInsidePack(int packIndex) => _coinsInsideShopPacks[packIndex];

        public int GetCoinsPackPrice(int packIndex) => _coinsPackPrices[packIndex];




    }
}
