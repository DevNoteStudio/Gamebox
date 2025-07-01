using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Locations", menuName = "Configs/Locations")]
public class LocationsConfig : ScriptableObject
{
    [SerializeField] private int _maxDifficulty;
    [SerializeField] private List<LocationData> _locations = new List<LocationData>();

    public int MaxDifficulty => _maxDifficulty;

    public IReadOnlyList<LocationData> Locations => _locations;

    private void OnValidate()
    {
        foreach (var location in _locations)
            location.Validate();
    }
}
