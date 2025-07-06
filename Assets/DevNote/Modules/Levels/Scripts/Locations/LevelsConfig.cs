using System.Collections.Generic;
using UnityEngine;

namespace DevNote.Modules.Levels
{
    [CreateAssetMenu(fileName = "[Levels]", menuName = "DevNote/Modules/Levels")]
    public class LevelsConfig : ScriptableObject
    {

        [field: SerializeField] public LevelsScreenView LevelsScreenPrefab { get; private set; }
        [field: SerializeField] public LocationsScreenView LocationsScreenPrefab { get; private set; }
        [field: SerializeField] public int MaxDifficulty { get; private set; }
        [field: SerializeField] public List<LocationData> LocationDataList { get; private set; } 


    }


}
