using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class LeaderEntryView : MonoBehaviour
    {
        public enum DisplayType { Top3, Player, Other }


        [SerializeField] private TextMeshProUGUI _stageText;
        [SerializeField] private TextMeshProUGUI _playerNameText;
        [SerializeField] private TextMeshProUGUI _ratingText;
        [SerializeField] private Image _panelImage;
        [SerializeField] private Color _playerColor;
        [SerializeField] private Color _originColor;


        public void Display(int stage, LeaderEntry entry, DisplayType displayType)
        {
            _stageText.text = stage.ToString();
            _playerNameText.text = entry.playerName;
            _ratingText.text = $"<sprite=0>{entry.rating}";

            if (displayType == DisplayType.Player)
                _panelImage.color = _playerColor;

            else if (displayType == DisplayType.Other)
                _panelImage.color = _originColor;


        }




    }
}
