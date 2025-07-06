using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DevNote.Modules.Levels
{
    public class LocationsScreenView : MonoBehaviour
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

        [Inject] private readonly LevelController levelController;
        [Inject] private readonly ScreenController screenController;


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
            var locationData = Configs.Levels.LocationDataList[_currentLocationIndex];

            DisplayDifficulty(locationData.difficulty);

            _starsAmountText.text = GameState.LevelProgress.TotalStarsAmount.ToString();
            _locationNameText.text = Localization.GetLocalizedText(locationData.nameLocalizationKey);
            _locationPreviewImage.sprite = locationData.previewSprite;

            int completedLevelsCount = levelController.GetCompletedLocationLevelsAmount(locationIndex);

            _completedLevelsText.text = $"{Localization.GetLocalizedText("levels_completed")}\n{completedLevelsCount}/{locationData.levelsAmount}";

            _backgroundImage.color = locationData.backgroundColor;

            _previousLocationButton.interactable = locationIndex > 0;
            _nextLocationButton.interactable = locationIndex < Configs.Levels.LocationDataList.Count - 1;
        }


        private void DisplayDifficulty(int value)
        {
            int maxDifficulty = Configs.Levels.MaxDifficulty;

            if (value > maxDifficulty)
                throw new Exception($"Difficulty more than Max Difficulty! Your value: {value}, Max value: {maxDifficulty}");

            if (_difficultyPointImages == null)
            {
                _difficultyPointImages = new List<Image> { _difficultyPointImagePrefab };
                for (int i = 1; i < maxDifficulty; i++)
                {
                    var difficultyPointImage = SceneInjector.InstantiateFromPrefabComponent(_difficultyPointImagePrefab, _difficultyPointContainer);
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
            screenController.ShowLevelsScreen(_currentLocationIndex);
            screenController.HideLocationsScreen();
        }

        private void OnPlayButtonClick()
        {
            screenController.HideLocationsScreen();

            int levelIndex = levelController.GetLastAvailableLevelIndexInsideLocation(_currentLocationIndex);
            levelController.StartLevel(_currentLocationIndex, levelIndex);
        }


    }
}