using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamebox
{

    [Serializable] public struct LeaderboardEntry
    {
        public string playerName;
        public int rating;
    }


    [CreateAssetMenu(menuName = "Gamebox/Leaderboard", fileName = "Leaderboard")]
    public class LeaderboardConfig : ScriptableObject
    {
        [SerializeField] private List<LeaderboardEntry> _entries; public IReadOnlyList<LeaderboardEntry> Entries => _entries;



    }
}
