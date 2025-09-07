using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace DevNote.Gamebox
{
    using static Common;


    public class ItemsState<T> where T : Enum
    {
        public delegate void OnChange(T type, int change);

        public event OnChange OnSpent;
        public event OnChange OnEarned;
        public event OnChange OnChanged;

        private Dictionary<T, int> _amounts;
        private Dictionary<T, int> _cheatAmounts;

        private const int CHEAT_AMOUNT = 99999;


        public ItemsState(string data)
        {
            _amounts = new Dictionary<T, int>();
            _cheatAmounts = new Dictionary<T, int>();

            if (!string.IsNullOrEmpty(data))
            {
                string[] amountsData = data.Split(S2);
                foreach (var amountData in amountsData)
                {
                    string[] typeValueData = amountData.Split(S1);

                    var type = (T)Enum.Parse(typeof(T), typeValueData[0]);
                    int value = int.Parse(typeValueData[1]);

                    Set(type, value);
                }
            }
        }


        public int Get(T type)
        {
            bool cheatModeEnabled = _cheatAmounts.ContainsKey(type);
            return cheatModeEnabled ? _cheatAmounts[type] : _amounts.GetValueOrDefault(type, 0);
        }

        public void Set(T type, int value)
        {
            int previousValue = Get(type);
            bool cheatModeEnabled = _cheatAmounts.ContainsKey(type);

            if (cheatModeEnabled) _cheatAmounts[type] = value;
            else
            {
                if (!_amounts.ContainsKey(type))
                    _amounts.Add(type, value);

                else _amounts[type] = value;
            }

            OnChanged?.Invoke(type, value - previousValue);
        }

        public bool IsCheatMode(T type) => _cheatAmounts.ContainsKey(type);

        public void SetCheatMode(T type, bool enabled)
        {
            if (enabled && !_cheatAmounts.ContainsKey(type))
                _cheatAmounts.Add(type, CHEAT_AMOUNT);

            if (!enabled && _cheatAmounts.ContainsKey(type))
                _cheatAmounts.Remove(type);
        }

        public void Spend(T type, int value)
        {
            int currentValue = Get(type);

            if (Get(type) - value < 0)
                Debug.LogWarning($"{LogPrefix} Not enough balance! Spend: {value}, balance: {Get(type)}");

            Set(type, currentValue - value);
            OnSpent?.Invoke(type, -value);
        }

        public void Earn(T type, int value)
        {
            Set(type, Get(type) + value);
            OnEarned?.Invoke(type, value);
        }

        


        public override string ToString()
        {
            var builder = new StringBuilder();

            int i = 0;
            foreach (var itemAmount in _amounts)
            {
                T type = itemAmount.Key;
                int amount = itemAmount.Value;

                builder.Append($"{Convert.ToInt32(type)}{S1}{amount}");
                if (i < _amounts.Count - 1) builder.Append(S2);

                i++;
            }

            return builder.ToString();
        }


        

    }
}


