using System.Collections.Generic;
using UnityEngine;

namespace DevNote.Gamebox
{
    [CreateAssetMenu(fileName = "[LevelUp]", menuName = "LevelUp/Main Config")]
    public partial class LevelUpConfig : ScriptableObject // Main
    {
        [SerializeField] private ResourcesData _resources;

        [SerializeField] private List<LocationData> _locations;

















    }


}
