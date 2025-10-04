using System;
using System.Collections.Generic;
using System.Text;
using DevNote;
using UnityEngine;


namespace Gamebox
{

    public class ItemsState
    {
        public delegate void OnChange(string itemKey, int change);
        public event OnChange OnSpent, OnEarned, OnChanged;

        private Dictionary<string, int> _amounts;
        private Dictionary<string, int> _cheatAmounts;

        private const int CHEAT_AMOUNT = 99999;


        public ItemsState(string data)
        {
            _amounts = new Dictionary<string, int>();
            _cheatAmounts = new Dictionary<string, int>();

            if (!string.IsNullOrEmpty(data))
            {
                string[] amountsData = data.Split(S.S2);
                foreach (var amountData in amountsData)
                {
                    string[] keyValueData = amountData.Split(S.S1);

                    string key = keyValueData[0];
                    int value = int.Parse(keyValueData[1]);

                    Set(key, value);
                }
            }
        }


        public int Get(string itemKey)
        {
            bool cheatModeEnabled = _cheatAmounts.ContainsKey(itemKey);
            return cheatModeEnabled ? _cheatAmounts[itemKey] : _amounts.GetValueOrDefault(itemKey, 0);
        }

        public void Set(string itemKey, int value)
        {
            int previousValue = Get(itemKey);
            bool cheatModeEnabled = _cheatAmounts.ContainsKey(itemKey);

            if (cheatModeEnabled) _cheatAmounts[itemKey] = value;
            else
            {
                if (!_amounts.ContainsKey(itemKey))
                    _amounts.Add(itemKey, value);

                else _amounts[itemKey] = value;
            }

            OnChanged?.Invoke(itemKey, value - previousValue);
        }

        public bool IsCheatMode(string itemKey) => _cheatAmounts.ContainsKey(itemKey);

        public void SetCheatMode(string itemKey, bool enabled)
        {
            if (enabled && !_cheatAmounts.ContainsKey(itemKey))
                _cheatAmounts.Add(itemKey, CHEAT_AMOUNT);

            if (!enabled && _cheatAmounts.ContainsKey(itemKey))
                _cheatAmounts.Remove(itemKey);
        }

        public void Spend(string itemKey, int value)
        {
            int currentValue = Get(itemKey);

            if (Get(itemKey) - value < 0)
                Debug.LogWarning($"{Info.LogPrefix} Not enough balance! Spend: {value}, balance: {Get(itemKey)}");

            Set(itemKey, currentValue - value);
            OnSpent?.Invoke(itemKey, -value);
        }

        public void Add(string itemKey, int value)
        {
            Set(itemKey, Get(itemKey) + value);
            OnEarned?.Invoke(itemKey, value);
        }

        public bool Has(string itemKey) => Get(itemKey) > 0;


        public override string ToString()
        {
            var builder = new StringBuilder();

            int i = 0;
            foreach (var itemAmount in _amounts)
            {
                string key = itemAmount.Key;
                int amount = itemAmount.Value;

                builder.Append($"{key}{S.S1}{amount}");
                if (i < _amounts.Count - 1) builder.Append(S.S2);

                i++;
            }

            return builder.ToString();
        }


        

    }
}


