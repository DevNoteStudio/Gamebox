using System;
using System.Collections.Generic;
using System.Text;
using DevNote;


namespace Gamebox
{

    public class ItemsState
    {
        private class ItemData
        {
            public int amount;
            public bool isUnlocked;
            public Action onChanged;

            public ItemData(int amount, bool isUnlocked)
            {
                this.amount = amount;
                this.isUnlocked = isUnlocked;
                onChanged = null;
            }
        }

        public event Action<ItemKey> OnChanged;

        private Dictionary<ItemKey, ItemData> _items;

        public ItemsState(string data)
        {
            _items = new Dictionary<ItemKey, ItemData>();

            if (!string.IsNullOrEmpty(data))
            {
                string[] splitData = data.Split(S.S2);

                foreach (var itemData in splitData)
                {
                    string[] splitItemData = itemData.Split(S.S1);

                    if (splitItemData.Length == 2) // Old saves
                    {
                        ItemKey key = (ItemKey)int.Parse(splitItemData[0]);
                        int amount = int.Parse(splitItemData[1]);
                        bool isUnlocked = amount > 0;

                        _items[key] = new ItemData(amount, isUnlocked);
                    }
                    else if (splitItemData.Length == 3) // New saves
                    {
                        ItemKey key = (ItemKey)int.Parse(splitItemData[0]);
                        int amount = int.Parse(splitItemData[1]);
                        bool isUnlocked = splitItemData[2].FromBinaryToBool();

                        _items[key] = new ItemData(amount, isUnlocked);
                    }
                }
            }
        }

        private ItemData GetItemData(ItemKey key)
        {
            if (!_items.ContainsKey(key))
                _items.Add(key, new ItemData(amount: 0, isUnlocked: false));

            return _items[key];
        }

        public bool IsUnlocked(ItemKey itemKey) => GetItemData(itemKey).isUnlocked;

        public int Get(ItemKey itemKey) => GetItemData(itemKey).amount;

        public void Set(ItemKey itemKey, int amount)
        {
            var itemData = GetItemData(itemKey);

            itemData.amount = amount;

            if (amount > 0 && !itemData.isUnlocked)
                itemData.isUnlocked = true;

            itemData.onChanged?.Invoke();
            OnChanged?.Invoke(itemKey);
        }

        public void Spend(ItemKey itemKey, int amount) => Set(itemKey, Get(itemKey) - amount);
        public void Add(ItemKey itemKey, int amount) => Set(itemKey, Get(itemKey) + amount);
        public bool Has(ItemKey itemKey) => Get(itemKey) > 0;

        public void Unlock(ItemKey itemKey)
        {
            var itemData = GetItemData(itemKey);
            itemData.isUnlocked = true;
            itemData.onChanged?.Invoke();
            OnChanged?.Invoke(itemKey);
        }


        public void Subscribe(ItemKey itemKey, Action onChanged) 
            => GetItemData(itemKey).onChanged += onChanged;

        public void Dispose(ItemKey itemKey, Action onChanged)
            => GetItemData(itemKey).onChanged -= onChanged;

        public override string ToString()
        {
            var builder = new StringBuilder();

            int i = 0;
            foreach (var itemKeyValue in _items)
            {
                ItemKey key = itemKeyValue.Key;
                var data = itemKeyValue.Value;

                if (i != 0) builder.Append(S.S2);
                builder.Append($"{(int)key}{S.S1}{data.amount}{S.S1}{data.isUnlocked.ToBinaryString()}");

                i++;
            }

            return builder.ToString();
        }


        

    }
}


