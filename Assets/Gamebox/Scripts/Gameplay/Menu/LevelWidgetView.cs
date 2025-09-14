using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevNote.Gamebox
{
    public class LevelWidgetView : MonoBehaviour
    {
        [SerializeField] private GameObject _starsObject;
        [SerializeField] private GameObject _lockObject;
        [SerializeField] private TextMeshProUGUI _levelNumberText;
        [SerializeField] private Button _playButton;
        [SerializeField] private Color _fadeStarColor;
        [SerializeField] private Material _availableMaterial;
        [SerializeField] private Material _notAvailableMaterial;
        [SerializeField] private List<Image> _starImages;

        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<MenuController> menuController = new();

        private int _locationIndex;
        private int _levelIndex;

        private void Start()
        {
            _playButton.onClick.AddListener(OnPlayButtonClick);
        }

        public void Display(int locationIndex, int levelIndex)
        {
            _locationIndex = locationIndex;
            _levelIndex = levelIndex;

            int stars = GameState.Levels.GetLevelStars(locationIndex, levelIndex);
            bool isFirstLevel = levelIndex == 0;
            
            if (isFirstLevel) SetState(available: true, stars);
            else
            {
                bool previousLevelCompleted = GameState.Levels.GetLevelStars(locationIndex, levelIndex - 1) > 0;
                bool available = previousLevelCompleted || stars > 0;
                SetState(available, stars);
            }
        }

        
        private void SetState(bool available, int stars = 0)
        {
            _starsObject.SetActive(available);
            _levelNumberText.gameObject.SetActive(available);
            _lockObject.SetActive(!available);
            _playButton.interactable = available;
            _playButton.image.material = available ? _availableMaterial : _notAvailableMaterial;

            if (available)
            {
                _levelNumberText.text = (_levelIndex + 1).ToString();
                for (int i = 0; i < _starImages.Count; i++)
                {
                    bool starFilled = i < stars;
                    _starImages[i].color = starFilled ? Color.white : _fadeStarColor;
                }
            }
        }

        private void OnPlayButtonClick()
        {
            ScreenFade.Fade(onCompleted: () =>
            {
                menuController.Item.HideLevelsScreen();
                levelController.Item.StartLevel(_locationIndex, _levelIndex);
            });
        }


    }
}