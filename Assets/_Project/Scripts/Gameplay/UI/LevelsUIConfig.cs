using UnityEngine;

[CreateAssetMenu(fileName = "LevelsUI", menuName = "Configs/LevelsUI")]
public class LevelsUIConfig : ScriptableObject
{
    [SerializeField] private LevelsView _levelsViewPrefab;
    [SerializeField] private LevelItemView _levelItemPrefab;

    public LevelsView LevelsViewPrefab => _levelsViewPrefab;
    public LevelItemView LevelItemPrefab => _levelItemPrefab;
}