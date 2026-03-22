using System;
using DevNote;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gamebox
{
    public partial class GameboxConfig // Unlocks
    {
        [Serializable] private struct LevelUnlock
        {
            public int level;
            public UnlockKey unlockKey;
            public AssetReferenceT<Sprite> iconSpriteReference;
        }


        public string GetUnlockName(UnlockKey key) => Localization.GetLocalizedText($"{key}_unlock_name");

        public string GetUnlockDescription(UnlockKey key) => Localization.GetLocalizedText($"{key}_unlock_desc");


        public AssetReferenceT<Sprite> GetUnlockIconSpriteReference(UnlockKey unlockKey)
            => GetUnlockData(unlockKey).iconSpriteReference;


        public bool TryGetLevelUnlockKey(int level, out UnlockKey previousKey, out UnlockKey currentKey, out UnlockKey nextKey)
        {
            previousKey = currentKey = UnlockKey.NoneStart;
            nextKey = UnlockKey.NoneFinish;

            int index = _unlocks.FindIndex(data => data.level == level);

            if (index != -1)
            {
                currentKey = _unlocks[index].unlockKey;
                if (index != 0) previousKey = _unlocks[index - 1].unlockKey;
                if (index != _unlocks.Count - 1) nextKey = _unlocks[index + 1].unlockKey;

                return true;
            }
            else return false;
        }

        public bool IsAvailable(UnlockKey unlockKey) => IGameState.Level >= GetUnlockData(unlockKey).level;

        public int GetUnlockLevel(UnlockKey unlockKey)
        {
            const int ADD_LEVELS_FOR_NONE_FIHISH_KEY = 20;

            if (unlockKey == UnlockKey.NoneStart) return 1;
            else if (unlockKey == UnlockKey.NoneFinish) return _unlocks[^1].level + ADD_LEVELS_FOR_NONE_FIHISH_KEY;
            else return GetUnlockData(unlockKey).level;
        }


        private LevelUnlock GetUnlockData(UnlockKey key)
        {
            int index = _unlocks.FindIndex((content) => content.unlockKey == key);
            if (index >= 0) return _unlocks[index];

            else throw new Exception($"Unlocks doesn't contain key \"{key}\"");
        }



    }
}
