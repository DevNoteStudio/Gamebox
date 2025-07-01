using DevNote;
using UnityEngine;

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

    public void Validate()
    {
        if (Difficulty > Configs.Locations.MaxDifficulty)
            Difficulty = _lastDifficultyInput;
        else
            _lastDifficultyInput = Difficulty;
    }
}
