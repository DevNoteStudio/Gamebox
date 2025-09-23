
namespace Gamebox
{
    public struct ItemPack
    {
        public string itemKey;
        public int amount;

        public ItemPack(string itemKey, int amount)
        {
            this.itemKey = itemKey;
            this.amount = amount;
        }
    }
}

