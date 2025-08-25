using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevNote.Gamebox
{
    public class LevelWidgetView : MonoBehaviour
    {
        [SerializeField] private GameObject _starPanel;
        [SerializeField] private TextMeshProUGUI _levelNumberText;
        [SerializeField] private Button _playButton;
        [SerializeField] private List<Image> _starImages;


        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<ScreenController> screenController = new();


        private int _locationIndex;
        private int _levelIndex;

        private void Start()
        {
            _playButton.onClick.AddListener(OnPlayButtonClick);
        }

        public void Display(int locationIndex, int levelIndex)
        {
            gameObject.SetActive(true);

            int stars = GameState.Levels.GetLevelStars(locationIndex, levelIndex);

            _locationIndex = locationIndex;
            _levelIndex = levelIndex;
            _levelNumberText.text = (levelIndex + 1).ToString();

            bool isActive = stars > 0 || levelController.Item.GetLastLevelIndex(_locationIndex) == levelIndex;

            _playButton.interactable = isActive;
            _starPanel.SetActive(isActive);


            for (int i = 0; i < _starImages.Count; i++)
                _starImages[i].color = i < stars ? Color.yellow : Color.black;
        }

        private void OnPlayButtonClick()
        {
            screenController.Item.HideLevelsScreen();
            levelController.Item.StartLevel(_locationIndex, _levelIndex);
        }
    }
}