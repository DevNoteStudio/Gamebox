using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace DevNote.Modules.Levels
{
    public class LevelItemView : MonoBehaviour
    {
        [SerializeField] private RectTransform _starsParent;
        [SerializeField] private TextMeshProUGUI _levelNumberText;
        [SerializeField] private Button _playButton;

        [Inject] private readonly LevelController levelController;

        private int _locationNumber;
        private int _levelNumber;
        private List<Image> _stars;

        private void Start()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        public void Display(int locationNumber, int levelNumber, bool isActive)
        {
            _locationNumber = locationNumber;
            _levelNumber = levelNumber;
            _levelNumberText.text = (levelNumber + 1).ToString();
            _playButton.interactable = isActive;
            _starsParent.gameObject.SetActive(isActive);
            _stars = _starsParent.GetComponentsInChildren<Image>(true).ToList();

            int stars = GameState.CompletedLevels.Value[locationNumber][levelNumber];

            for (int i = 0; i < _stars.Count; i++)
                _stars[i].color = i < stars ? Color.yellow : Color.black;
        }

        private void OnPlayButtonClicked()
        {
            levelController.StartLevel(_locationNumber, _levelNumber);
        }
    }
}