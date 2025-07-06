using UnityEngine;

namespace DevNote.Modules.Levels
{
    [System.Serializable]
    public struct LocationData
    {
        public string nameLocalizationKey;
        public Sprite previewSprite;
        public Color backgroundColor;
        public int levelsAmount;
        public int difficulty;
        public int starsToUnlock;
    }
}