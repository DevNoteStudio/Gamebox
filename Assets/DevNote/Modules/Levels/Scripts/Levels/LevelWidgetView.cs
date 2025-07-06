using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DevNote.Modules.Levels
{
    public class LevelWidgetView : MonoBehaviour
    {
        [SerializeField] private GameObject _starPanel;
        [SerializeField] private TextMeshProUGUI _levelNumberText;
        [SerializeField] private Button _playButton;
        [SerializeField] private List<Image> _starImages;


        [Inject] private readonly LevelController levelController;
        [Inject] private readonly ScreenController screenController;


        private int _locationIndex;
        private int _levelIndex;

        private void Start()
        {
            _playButton.onClick.AddListener(OnPlayButtonClick);
        }

        public void Display(int locationIndex, int levelIndex)
        {
            gameObject.SetActive(true);

            int stars = GameState.LevelProgress.GetLevelStarsAmount(locationIndex, levelIndex);

            _locationIndex = locationIndex;
            _levelIndex = levelIndex;
            _levelNumberText.text = (levelIndex + 1).ToString();

            bool isActive = stars > 0 || levelController.GetLastAvailableLevelIndexInsideLocation(_locationIndex) == levelIndex;

            _playButton.interactable = isActive;
            _starPanel.SetActive(isActive);


            for (int i = 0; i < _starImages.Count; i++)
                _starImages[i].color = i < stars ? Color.yellow : Color.black;
        }

        private void OnPlayButtonClick()
        {
            screenController.HideLevelsScreen();
            levelController.StartLevel(_locationIndex, _levelIndex);
        }
    }
}