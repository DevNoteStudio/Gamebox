using System;
using UnityEngine;

namespace DevNote.LevelUp
{
    [Serializable] public struct LocationData
    {
        public string nameLocalizationKey;
        public Sprite previewSprite;
        public Color backgroundColor;
        public int levels;
        public int difficulty;
        public int starsToUnlock;
    }
}