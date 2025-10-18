using System;
using System.Collections.Generic;
using System.Text;
using DevNote;


namespace Gamebox
{

    public class ItemsState
    {
        public event Action<ItemKey> OnChanged;

        private Dictionary<ItemKey, int> _amounts;
        private Dictionary<ItemKey, int> _cheatAmounts;
        private Dictionary<ItemKey, Action> _changeActions = new();


        public ItemsState(string data)
        {
            _amounts = new Dictionary<ItemKey, int>();
            _cheatAmounts = new Dictionary<ItemKey, int>();

            if (!string.IsNullOrEmpty(data))
            {
                string[] amountsData = data.Split(S.S2);
                foreach (var amountData in amountsData)
                {
                    string[] keyValueData = amountData.Split(S.S1);

                    ItemKey key = (ItemKey)int.Parse(keyValueData[0]);
                    int value = int.Parse(keyValueData[1]);

                    Set(key, value);
                }
            }
        }


        public int Get(ItemKey itemKey)
        {
            bool cheatModeEnabled = _cheatAmounts.ContainsKey(itemKey);
            return cheatModeEnabled ? _cheatAmounts[itemKey] : _amounts.GetValueOrDefault(itemKey, 0);
        }

        public void Set(ItemKey itemKey, int value)
        {
            bool cheatModeEnabled = _cheatAmounts.ContainsKey(itemKey);

            if (cheatModeEnabled) _cheatAmounts[itemKey] = value;
            else
            {
                if (!_amounts.ContainsKey(itemKey))
                    _amounts.Add(itemKey, value);

                else _amounts[itemKey] = value;
            }

            GetChangeAction(itemKey)?.Invoke();
            OnChanged?.Invoke(itemKey);
        }

        public bool IsCheatMode(ItemKey itemKey) => _cheatAmounts.ContainsKey(itemKey);

        public void SetCheatMode(ItemKey itemKey, bool enabled, int cheatAmount = 99999)
        {
            if (enabled && !_cheatAmounts.ContainsKey(itemKey))
                _cheatAmounts.Add(itemKey, cheatAmount);

            if (!enabled && _cheatAmounts.ContainsKey(itemKey))
                _cheatAmounts.Remove(itemKey);
        }

        public void Spend(ItemKey itemKey, int value) => Set(itemKey, Get(itemKey) - value);
        public void Add(ItemKey itemKey, int value) => Set(itemKey, Get(itemKey) + value);
        public bool Has(ItemKey itemKey) => Get(itemKey) > 0;


        public void Subscribe(ItemKey itemKey, Action onChanged)
        {
            var action = GetChangeAction(itemKey);
            _changeActions[itemKey] = action + onChanged;
        }

        public void Dispose(ItemKey itemKey, Action onChanged)
        {
            var action = GetChangeAction(itemKey);
            _changeActions[itemKey] = action - onChanged;
        }


        private Action GetChangeAction(ItemKey itemKey)
        {
            if (!_changeActions.ContainsKey(itemKey))
                _changeActions.Add(itemKey, null);

            return _changeActions[itemKey];
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            int i = 0;
            foreach (var itemAmount in _amounts)
            {
                ItemKey key = itemAmount.Key;
                int amount = itemAmount.Value;

                builder.Append($"{(int)key}{S.S1}{amount}");
                if (i < _amounts.Count - 1) builder.Append(S.S2);

                i++;
            }

            return builder.ToString();
        }


        

    }
}


