using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DevNote;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class CheatsView : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private GameObject _panelObject;
        [Space]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _previousLevelButton;
        [SerializeField] private Button _loseButton;
        [SerializeField] private Button _winButton;
        [SerializeField] private Button _unlockLevelsButton;
        [SerializeField] private Button _nextLeagueButton;
        [SerializeField] private Button _boosterButton;
        [SerializeField] private Button _currencyButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _maxRatingButton;
        [SerializeField] private Button _unlockCardsButton;
        [Space]
        [SerializeField] private Color _notActiveColor;
        [SerializeField] private Color _activeColor;


        private int _clicksToOpenPanel = 12;
        private int _originRating;
        private bool _maxRatingEnabled = false;
        private bool _maxBoostersEnabled = false;

        private Dictionary<ItemKey, int> _originBoosterAmounts = new();

        private readonly Holder<ISave> save = new();
        private readonly Holder<LevelController> levelController = new();

        private const int MAX_RATING = 10000000;


        private async void Awake()
        {
            if (IEnvironment.IsEditor) _clicksToOpenPanel = 1;

            _panelObject.SetActive(false);

            await UniTask.WaitUntil(() => ProjectContext.Initialized);

            levelController.Item.OnLevelStarted += Display;
            levelController.Item.OnLevelExit += Display;
            levelController.Item.OnLevelLost += Display;
            levelController.Item.OnRevive += Display;
        }

        private void Start()
        {
            _openButton.onClick.AddListener(OnOpenButtonClick);
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            _resetButton.onClick.AddListener(OnResetButtonClick);
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
            _previousLevelButton.onClick.AddListener(OnPreviousLevelButtonClick);
            _maxRatingButton.onClick.AddListener(OnMaxRatingButtonClick);
            _boosterButton.onClick.AddListener(OnBoosterButtonClick);
        }

        private void Display()
        {
            if (!_panelObject.activeInHierarchy) return;

            var config = IConfigs.Gamebox;

            int locationIndex = levelController.Item.CurrentLocationIndex;
            int levelIndex = levelController.Item.CurrentLevelIndex;

            _previousLevelButton.interactable = levelController.Item.IsLevelPlaying
                && !(locationIndex == 0 && levelIndex == 0);

            bool isLastLevel = locationIndex == config.LocationsAmount - 1 
                && levelIndex == config.GetLocationLevelsAmount(locationIndex) - 1;

            _nextLevelButton.interactable = levelController.Item.IsLevelPlaying && !isLastLevel;
        }

        private void OnBoosterButtonClick()
        {
            _maxBoostersEnabled = !_maxBoostersEnabled;
            _boosterButton.image.color = _maxBoostersEnabled ? _activeColor : _notActiveColor;

            var allBoosterKeys = IConfigs.Gamebox.GetAllBoosterKeys();

            if (_maxBoostersEnabled)
            {
                foreach (var itemKey in allBoosterKeys)
                {
                    _originBoosterAmounts[itemKey] = IGameState.Items.Get(itemKey);
                    IGameState.Items.Set(itemKey, 99);
                }
            }
            else
            {
                foreach (var itemKey in allBoosterKeys)
                     IGameState.Items.Set(itemKey, _originBoosterAmounts[itemKey]);
            }

        }

        private void OnMaxRatingButtonClick()
        {
            _maxRatingEnabled = !_maxRatingEnabled;
            _maxRatingButton.image.color = _maxRatingEnabled ? _activeColor : _notActiveColor;

            if (_maxRatingEnabled)
            {
                _originRating = IGameState.Rating.Value;
                IGameState.Rating.Value = MAX_RATING;
            }
            else IGameState.Rating.Value = _originRating;
        }

        private void OnPreviousLevelButtonClick()
        {
            int level = Mathf.Max(IGameState.Level - 1, 1);
            levelController.Item.StartLevel(level);
        }

        private void OnNextLevelButtonClick()
        {
            int level = IGameState.Level + 1;
            levelController.Item.StartLevel(level);
        }

        private void OnResetButtonClick()
        {
            save.Item.DeleteSaves();
            _resetButton.image.color = _activeColor;
            _resetButton.interactable = false;
        }

        private void OnCloseButtonClick()
        {
            _panelObject.SetActive(false);
        }

        private void OnOpenButtonClick()
        {
            _clicksToOpenPanel--;
            if (_clicksToOpenPanel > 0) return;

            _panelObject.SetActive(true);
            Display();
        }




    }
}


