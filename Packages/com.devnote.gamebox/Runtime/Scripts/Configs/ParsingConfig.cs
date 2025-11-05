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



    }
}
