using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace DevNote.Gamebox
{
    using static Common;


    public class ItemsState
    {
        public delegate void OnChange(ItemType itemType, int change);
        public event OnChange OnSpent, OnEarned, OnChanged;

        private Dictionary<ItemType, int> _amounts;
        private Dictionary<ItemType, int> _cheatAmounts;

        private const int CHEAT_AMOUNT = 99999;


        public ItemsState(string data)
        {
            _amounts = new Dictionary<ItemType, int>();
            _cheatAmounts = new Dictionary<ItemType, int>();

            if (!string.IsNullOrEmpty(data))
            {
                string[] amountsData = data.Split(S.S2);
                foreach (var amountData in amountsData)
                {
                    string[] typeValueData = amountData.Split(S.S1);

                    var type = (ItemType)Enum.Parse(typeof(ItemType), typeValueData[0]);
                    int value = int.Parse(typeValueData[1]);

                    Set(type, value);
                }
            }
        }


        public int Get(ItemType itemType)
        {
            bool cheatModeEnabled = _cheatAmounts.ContainsKey(itemType);
            return cheatModeEnabled ? _cheatAmounts[itemType] : _amounts.GetValueOrDefault(itemType, 0);
        }

        public void Set(ItemType itemType, int value)
        {
            int previousValue = Get(itemType);
            bool cheatModeEnabled = _cheatAmounts.ContainsKey(itemType);

            if (cheatModeEnabled) _cheatAmounts[itemType] = value;
            else
            {
                if (!_amounts.ContainsKey(itemType))
                    _amounts.Add(itemType, value);

                else _amounts[itemType] = value;
            }

            OnChanged?.Invoke(itemType, value - previousValue);
        }

        public bool IsCheatMode(ItemType itemType) => _cheatAmounts.ContainsKey(itemType);

        public void SetCheatMode(ItemType itemType, bool enabled)
        {
            if (enabled && !_cheatAmounts.ContainsKey(itemType))
                _cheatAmounts.Add(itemType, CHEAT_AMOUNT);

            if (!enabled && _cheatAmounts.ContainsKey(itemType))
                _cheatAmounts.Remove(itemType);
        }

        public void Spend(ItemType itemType, int value)
        {
            int currentValue = Get(itemType);

            if (Get(itemType) - value < 0)
                Debug.LogWarning($"{LogPrefix} Not enough balance! Spend: {value}, balance: {Get(itemType)}");

            Set(itemType, currentValue - value);
            OnSpent?.Invoke(itemType, -value);
        }

        public void Earn(ItemType itemType, int value)
        {
            Set(itemType, Get(itemType) + value);
            OnEarned?.Invoke(itemType, value);
        }

        


        public override string ToString()
        {
            var builder = new StringBuilder();

            int i = 0;
            foreach (var itemAmount in _amounts)
            {
                ItemType type = itemAmount.Key;
                int amount = itemAmount.Value;

                builder.Append($"{Convert.ToInt32(type)}{S.S1}{amount}");
                if (i < _amounts.Count - 1) builder.Append(S.S2);

                i++;
            }

            return builder.ToString();
        }


        

    }
}


