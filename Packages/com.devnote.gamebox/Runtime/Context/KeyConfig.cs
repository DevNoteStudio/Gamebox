using System.Collections.Generic;
using UnityEngine;

namespace Gamebox
{
    [CreateAssetMenu(menuName = "Gamebox/Item Key Store", fileName = "Items")]
    public class KeyConfig : ScriptableObject
    {
        [field: SerializeField] public List<string> Keys { get; private set; }
    }
}

