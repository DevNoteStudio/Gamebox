using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Locations", menuName = "Configs/Locations")]
public class LocationsConfig : ScriptableObject
{
    [SerializeField] private int _maxDifficulty;
    [SerializeField] private Image _difficultyItemPrefab;
    [SerializeField] private List<LocationData> _locationsList = new List<LocationData>();

    public int MaxDifficulty => _maxDifficulty;
    public Image DifficultyItemPrefab => _difficultyItemPrefab;

    public IReadOnlyList<LocationData> List => _locationsList;

    private void OnValidate()
    {
        foreach (var location in _locationsList)
            location.Validate(MaxDifficulty);
    }
}
