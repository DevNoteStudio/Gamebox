using System.Collections.Generic;
using UnityEngine;


namespace DevNote.Gamebox
{
    [CreateAssetMenu(menuName = "Gamebox/Config", fileName = "[Gamebox]")]
    public partial class GameboxConfig : ScriptableObject // Main
    {
        [SerializeField] private ResourcesData _resources;
        [SerializeField] private List<ItemData> _items;
        [SerializeField] private List<LocationData> _locations;



    }
}


