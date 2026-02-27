using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamebox
{

    [Serializable] public class ContentPipeline
    {
        [Serializable] private struct LevelContent
        {
            public int level;
            public ContentKey contentKey;
        }

        [SerializeField] private List<LevelContent> _content;


        public bool IsNow(ContentKey contentKey) => IGameState.Level == GetContent(contentKey).level;

        public bool IsAvailable(ContentKey contentKey) => IGameState.Level >= GetContent(contentKey).level;

        public int GetLevel(ContentKey contentKey) => GetContent(contentKey).level;


        private LevelContent GetContent(ContentKey key)
        {
            int index = _content.FindIndex((content) => content.contentKey == key);
            if (index >= 0) return _content[index];

            else throw new Exception($"Content pipeline doesn't contain key \"{key}\"");
        }






    }
}
