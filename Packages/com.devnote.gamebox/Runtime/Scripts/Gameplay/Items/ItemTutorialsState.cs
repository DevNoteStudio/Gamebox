using System.Collections.Generic;
using System.Text;
using DevNote;

namespace Gamebox
{
    public class ItemTutorialsState
    {

        private Dictionary<ItemKey, bool> _tutorialsComplete;



        public ItemTutorialsState(string data)
        {
            _tutorialsComplete = new();

            if (string.IsNullOrEmpty(data) == false)
            {
                string[] keyValues = data.Split(S.S2);
                foreach (string keyValue in keyValues)
                {
                    string[] keyValueSplit = keyValue.Split(S.S1);

                    ItemKey itemKey = (ItemKey)int.Parse(keyValueSplit[0]);
                    bool completed = keyValueSplit[1].FromBinaryToBool();

                    _tutorialsComplete.Add(itemKey, completed);
                }
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            int i = 0;
            foreach (var tutorialComplete in _tutorialsComplete)
            {
                if (i != 0) builder.Append(S.S2);
                builder.Append($"{(int)tutorialComplete.Key}{S.S1}{tutorialComplete.Value.ToBinaryString()}");
                i++;
            }

            return builder.ToString();
        }

        public bool IsCompleted(ItemKey itemKey) => _tutorialsComplete.GetValueOrDefault(itemKey, false);

        public void SetCompleted(ItemKey itemKey, bool completed) => _tutorialsComplete[itemKey] = completed;


    }
}

