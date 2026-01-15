using System;

namespace Gamebox
{
    [Serializable] public struct ItemPack
    {
        public ItemKey itemKey;
        public int amount;

        public ItemPack(ItemKey itemKey, int amount)
        {
            this.itemKey = itemKey;
            this.amount = amount;
        }

        public ItemPack Set(int amount) => new ItemPack(itemKey, amount);
    }
}

