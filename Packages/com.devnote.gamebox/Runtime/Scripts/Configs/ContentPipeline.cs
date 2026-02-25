using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamebox
{

    [Serializable] public class ContentPipeline
    {
        [Serializable] private struct LevelContent
        {
            public int levelNumber;
            public ContentKey contentKey;
        }

        [SerializeField] private List<LevelContent> _content;


        public bool IsNow(ContentKey contentKey)
        {
            int currentLevelNumber = IGameState.Levels.CompletedLevels + 1;
            return currentLevelNumber == GetContent(contentKey).levelNumber;
        }

        public bool IsUnlocked(ContentKey contentKey)
        {
            int currentLevelNumber = IGameState.Levels.CompletedLevels + 1;
            return currentLevelNumber >= GetContent(contentKey).levelNumber;
        }

        public int GetLevel(ContentKey contentKey) => GetContent(contentKey).levelNumber;


        private LevelContent GetContent(ContentKey key)
        {
            int index = _content.FindIndex((content) => content.contentKey == key);
            if (index >= 0) return _content[index];

            else throw new Exception($"Content pipeline doesn't contain key \"{key}\"");
        }






    }
}
