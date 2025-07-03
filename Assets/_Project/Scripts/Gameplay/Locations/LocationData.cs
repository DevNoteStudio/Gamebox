using UnityEngine;

namespace DevNote.Modules.Levels
{
    [System.Serializable]
    public class LocationData
    {
        public LocationType Type;
        public Sprite Image;
        public Color BackgroundColor;
        [Min(1)] public int LevelCount;
        [Min(0)] public float Difficulty;
        [Min(0)] public int StarsToUnlock;

        private float _lastDifficultyInput = 0;

        public void Validate(int maxDifficulty)
        {
            if (Difficulty > maxDifficulty)
                Difficulty = _lastDifficultyInput;
            else
                _lastDifficultyInput = Difficulty;
        }
    }
}