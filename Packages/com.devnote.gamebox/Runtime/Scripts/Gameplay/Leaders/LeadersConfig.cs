using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Gamebox
{

    [Serializable] public struct LeaderEntry
    {
        public string playerName;
        public int rating;
    }


    [CreateAssetMenu(menuName = "Gamebox/Leaders", fileName = "Leaders")]
    public class LeadersConfig : ScriptableObject
    {
        [SerializeField] private List<LeaderEntry> _entries; public IReadOnlyList<LeaderEntry> Entries => _entries;


        [Button("Sort")] private void SortEntries() => _entries.Sort((a, b) => b.rating.CompareTo(a.rating));

        public int GetRank(int rating)
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                if (_entries[i].rating <= rating)
                    return i + 1;
            }
            return _entries.Count + 1;
        }


    }
}
