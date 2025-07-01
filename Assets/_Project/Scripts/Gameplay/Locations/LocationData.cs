using DevNote;
using UnityEngine;

[System.Serializable]
public class LocationData
{
    public LocationType Type;
    [Min(1)] public int LevelCount;
    public Sprite Image;
    public Color Background;
    [Min(0)] public int Difficulty;
    [Min(0)] public int StarsToUnlock;

    private int _lastDifficultyInput = 0;

    public void Validate()
    {
        if (Difficulty > Configs.Locations.MaxDifficulty)
            Difficulty = _lastDifficultyInput;
        else
            _lastDifficultyInput = Difficulty;
    }
}
