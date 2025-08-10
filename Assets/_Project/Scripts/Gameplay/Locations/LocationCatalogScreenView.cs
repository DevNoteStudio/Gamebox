using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevNote.LevelUp
{
    public class LocationCatalogScreenView : MonoBehaviour
    {
        [Header("Main:")]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _locationPreviewImage;
        [SerializeField] private TextMeshProUGUI _starsAmountText;
        [SerializeField] private TextMeshProUGUI _locationNameText;
        [SerializeField] private TextMeshProUGUI _completedLevelsText;

        [Header("Difficulty:")]
        [SerializeField] private Image _difficultyPointImagePrefab;
        [SerializeField] private RectTransform _difficultyPointContainer;

        [Header("Buttons:")]
        [SerializeField] private Button _previousLocationButton;
        [SerializeField] private Button _nextLocationButton;
        [SerializeField] private Button _levelsButton;
        [SerializeField] private Button _playButton;


        private List<Image> _difficultyPointImages;
        private int _currentLocationIndex = 0;
        private Sequence _showSequence;

        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<ScreenController> screenController = new();


        private const float COLOR_ANIMATION_DURATION = 0.4f;

        private void Start()
        {
            _previousLocationButton.onClick.AddListener(OnPreviousLevelButtonClick);
            _nextLocationButton.onClick.AddListener(OnNextLevelButtonClick);
            _levelsButton.onClick.AddListener(OnLevelsButtonClick);
            _playButton.onClick.AddListener(OnPlayButtonClick);
        }

        private void OnDisable()
        {
            _showSequence?.Kill();
        }

        public void Display(int locationIndex)
        {
            _currentLocationIndex = locationIndex;
            var locationData = Configs.LevelUp.LocationDataList[_currentLocationIndex];

            DisplayDifficulty(locationData.difficulty);

            _starsAmountText.text = GameState.Levels.TotalStars.ToString();
            _locationNameText.text = Localization.GetLocalizedText(locationData.nameLocalizationKey);
            _locationPreviewImage.sprite = locationData.previewSprite;

            int completedLevelsCount = levelController.Item.GetCompletedLevels(locationIndex);

            _completedLevelsText.text = $"{Localization.GetLocalizedText("levels_completed")}\n{completedLevelsCount}/{locationData.levels}";

            _backgroundImage.color = locationData.backgroundColor;

            _previousLocationButton.interactable = locationIndex > 0;
            _nextLocationButton.interactable = locationIndex < Configs.LevelUp.LocationDataList.Count - 1;
        }


        private void DisplayDifficulty(int value)
        {
            int maxDifficulty = Configs.LevelUp.MaxDifficulty;

            if (value > maxDifficulty)
                throw new Exception($"Difficulty more than Max Difficulty! Your value: {value}, Max value: {maxDifficulty}");

            if (_difficultyPointImages == null)
            {
                _difficultyPointImages = new List<Image> { _difficultyPointImagePrefab };
                for (int i = 1; i < maxDifficulty; i++)
                {
                    var difficultyPointImage = Instantiate(_difficultyPointImagePrefab, _difficultyPointContainer);
                    _difficultyPointImages.Add(difficultyPointImage);
                }
            }

            for (int number = 1; number <= _difficultyPointImages.Count; number++)
            {
                int index = number - 1;
                _difficultyPointImages[index].color = number <= value ? Color.white : Color.black;
            }
        }


        private void OnPreviousLevelButtonClick() => Display(_currentLocationIndex - 1);

        private void OnNextLevelButtonClick() => Display(_currentLocationIndex + 1);


        private void OnLevelsButtonClick()
        {
            screenController.Item.ShowLevelsScreen(_currentLocationIndex);
            screenController.Item.HideLocationsScreen();
        }

        private void OnPlayButtonClick()
        {
            screenController.Item.HideLocationsScreen();

            int levelIndex = levelController.Item.GetLastAvailableLevelIndexInsideLocation(_currentLocationIndex);
            levelController.Item.StartLevel(_currentLocationIndex, levelIndex);
        }


    }
}