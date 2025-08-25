using System;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevNote.Gamebox
{
    public class LocationCatalogScreenView : MonoBehaviour
    {
        [Header("Main:")]
        [SerializeField] private Image _locationPreviewImage;
        [SerializeField] private TextMeshProUGUI _starsAmountText;
        [SerializeField] private TextMeshProUGUI _locationNameText;
        [SerializeField] private TextMeshProUGUI _completedLevelsText;

        [Header("Buttons:")]
        [SerializeField] private Button _previousLocationButton;
        [SerializeField] private Button _nextLocationButton;
        [SerializeField] private Button _levelsButton;
        [SerializeField] private Button _playButton;

        [SerializeField] private SimpleScrollSnap _scrollSnap;



        private List<Image> _difficultyPointImages;
        private int _currentLocationIndex = 0;
        private Sequence _showSequence;

        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<ScreenController> screenController = new();


        private const float COLOR_ANIMATION_DURATION = 0.4f;

        private void Start()
        {
            //_previousLocationButton.onClick.AddListener(OnPreviousLevelButtonClick);
            //_nextLocationButton.onClick.AddListener(OnNextLevelButtonClick);
            //_levelsButton.onClick.AddListener(OnLevelsButtonClick);
            //_playButton.onClick.AddListener(OnPlayButtonClick);

            
        }


        private void OnDisable()
        {
            _showSequence?.Kill();
        }

        public void Display(int locationIndex)
        {
            
        }


        private void OnScrollPanelCentered(int centeredPanelIndex)
        {
            print($"{centeredPanelIndex}");
            /*
            for (int i = 0; i < _scrollSnap.Panels.Length; i++)
            {
                if (i == centeredPanelIndex)
                    _scrollSnap.Panels[i].transform.DOScale(1f, 0.5f);

                else _scrollSnap.Panels[i].transform.DOScale(0.7f, 0.5f);
            }
            */
            

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

            int levelIndex = levelController.Item.GetLastLevelIndex(_currentLocationIndex);
            levelController.Item.StartLevel(_currentLocationIndex, levelIndex);
        }


    }
}