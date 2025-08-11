using System.Collections.Generic;
using UnityEngine;

namespace DevNote.LevelUp
{
    [CreateAssetMenu(fileName = "[LevelUp]", menuName = "LevelUp/Main Config")]
    public partial class LevelUpConfig : ScriptableObject // Main
    {
        [SerializeField] private ResourcesData _resources;
        


        [field: SerializeField] public int MaxDifficulty { get; private set; }
        [field: SerializeField] public List<LocationData> LocationDataList { get; private set; } 

















    }


}
