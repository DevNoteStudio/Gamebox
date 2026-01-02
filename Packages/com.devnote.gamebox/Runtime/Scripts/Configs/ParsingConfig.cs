using System.Collections.Generic;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    public partial class GameboxConfig // Parsing
    {

        public override void LoadData(Dictionary<TableKey, Table> tables)
        {
            ParseLeagues(tables[TableKey.Leagues]);
            ParseShop(tables[TableKey.Shop]);
        }

        private void ParseLeagues(Table leaguesTable)
        {
            for (int i = 0; i < _leagues.Count; i++)
            {
                int row = i + 2;
                _leagues[i].ratingRequire = leaguesTable.GetInt(row, Column.C);

                string rewardsData = leaguesTable.Get(row, Column.D).Trim();
                if (rewardsData != string.Empty)
                {
                    rewardsData = rewardsData.Replace(" ", string.Empty);

                    List<ItemPack> rewards = new();

                    string[] splitData = rewardsData.Split(',');
                    foreach (string rewardData in splitData)
                    {
                        string[] itemKeyAmountPair = rewardData.Split(':');

                        ItemKey itemKey = itemKeyAmountPair[0].ToEnum<ItemKey>();
                        int amount = int.Parse(itemKeyAmountPair[1]);

                        rewards.Add(new ItemPack(itemKey, amount));
                    }

                    _leagues[i].rewardItems = rewards;
                }
            }

        }

        private void ParseShop(Table shopTable)
        {
            _shopBoxes = new List<ShopBoxData>()
            {
                ParseBoxData("common_box", ItemKey.CommonBox, shopTable),
                ParseBoxData("rare_box", ItemKey.RareBox,  shopTable),
                ParseBoxData("epic_box", ItemKey.EpicBox,  shopTable),
                ParseBoxData("rare_card", ItemKey.RareCard,  shopTable),
                ParseBoxData("epic_card", ItemKey.EpicCard,  shopTable),
                ParseBoxData("legendary_card", ItemKey.LegendaryCard,  shopTable),
            };

            _gemsInsideShopPacks = new List<int>();
            for (int i = 1; i <= 6; i++)
                _gemsInsideShopPacks.Add(shopTable.GetInt(Column.A, Column.B, $"gems_{i}"));

            _coinsInsideShopPacks = new List<int>();
            _coinsPackPrices = new List<int>();
            for (int i = 1; i <= 3; i++)
            {
                _coinsInsideShopPacks.Add(shopTable.GetInt(Column.A, Column.B, $"coins_{i}"));
                _coinsPackPrices.Add(shopTable.GetInt(Column.A, Column.C, $"coins_{i}"));
            }
  
        }

        private ShopBoxData ParseBoxData(string id, ItemKey boxItemKey, Table shopTable)
        {
            var boxData = new ShopBoxData();
            boxData.boxItemKey = boxItemKey;

            int row = shopTable.GetRow(Column.A, id);

            boxData.price = shopTable.GetInt(row, Column.C);
            boxData.buyForGems = boxItemKey != ItemKey.CommonBox;

            string content = shopTable.Get(row, Column.B);

            string[] itemDataLines = content.Split('\n');

            foreach (var itemData in itemDataLines)
            {
                int min = 0, max = 0;
                string[] splitItemData = itemData.Split(' ');

                string amountData = splitItemData[0];
                string itemNameData = splitItemData[1];

                if (amountData.Contains('-'))
                {
                    string[] minMaxAmountData = amountData.Split('-');
                    min = int.Parse(minMaxAmountData[0]);
                    max = int.Parse(minMaxAmountData[1]);
                }
                else min = max = int.Parse(amountData);

                switch (itemNameData)
                {
                    case "Common": boxData.minMaxCommonCards = new Vector2Int(min, max); break;
                    case "Rare": boxData.minMaxRareCards = new Vector2Int(min, max); break;
                    case "Epic": boxData.minMaxEpicCards = new Vector2Int(min, max); break;
                    case "Legendary": boxData.minMaxLegendaryCards = new Vector2Int(min, max); break;
                    case "Boosters": boxData.minMaxBoosters = new Vector2Int(min, max); break;

                    default: throw new System.Exception($"Wrong item name: {itemNameData}");
                }
            }

            return boxData;
        }


    }
}
