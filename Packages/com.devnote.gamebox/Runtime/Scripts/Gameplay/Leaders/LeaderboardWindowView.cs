using System;
using System.Collections.Generic;
using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Gamebox.LeaderboardEntryView;

namespace Gamebox
{
    public class LeaderboardWindowView : Window
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private TextMeshProUGUI _betterPercentageText;
        [SerializeField] private List<LeaderboardEntryView> _topEntries3; // 3 entries
        [SerializeField] private List<LeaderboardEntryView> _otherEntries6; // 6 entries

        private const string PLAYER_NAME = "You";

        private readonly Holder<LeadersController> leadersController = new();

        private void Start()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        private void OnCloseButtonClick()
        {
            leadersController.Item.HideLeadersWindow();
        }

        public LeaderboardWindowView Display()
        {
            var entries = IConfigs.Leaderboard.Entries;
            int playerRank = IConfigs.Leaderboard.GetRank(IGameState.Level);
            var playerEntry = new LeaderEntry() { playerName = PLAYER_NAME, rating = IGameState.Level };

            Debug.Log(playerRank);

            // <-- Top entries -->
            for (int i = 0, rank = 1; rank <= _topEntries3.Count; rank++, i++)
            {
                var entry = playerRank == rank ? playerEntry : entries[rank - 1];
                _topEntries3[i].Display(rank, entry, DisplayType.Top3);
            }

            // <-- Near top display player entry -->
            if (playerRank < 8)
            {
                for (int i = 0, rank = 4; i < _otherEntries6.Count; rank++, i++)
                {
                    var entry = playerRank == rank ? playerEntry : entries[rank - 1];
                    var displayType = playerRank == rank ? DisplayType.Player : DisplayType.Other;

                    _otherEntries6[i].Display(rank, entry, displayType);
                }
            }

            // <-- Common display -->
            else
            {
                for (int i = 0, rank = playerRank - 4; i < _otherEntries6.Count; rank++, i++)
                {
                    int index = rank - 1;
                    Debug.Log(index);
                    var entry = playerRank == rank ? playerEntry : entries[index];
                    var displayType = playerRank == rank ? DisplayType.Player : DisplayType.Other;

                    _otherEntries6[i].Display(rank, entry, displayType);
                }
            }

            int betterPercentage = GetPlayerBetterPercentage(playerRank);

            _betterPercentageText.text = 
                Localization.GetLocalizedText("leaders_better").Replace("{VALUE}", $"{betterPercentage}");


            return this;
        }



        private int GetPlayerBetterPercentage(int playerRank)
        {
            if (playerRank == 1) return 100;
            else
            {
                int maxRank = IConfigs.Leaderboard.Entries.Count;
                return (int)((1f - (float)playerRank / maxRank) * 100f);
            }
        }


    }
}
