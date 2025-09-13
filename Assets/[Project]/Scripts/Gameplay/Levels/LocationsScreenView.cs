using System;
using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevNote.Gamebox
{
    public class LocationsScreenView : MonoBehaviour
    {
        [SerializeField] private Image _previewPrefab;
        [SerializeField] private TextMeshProUGUI _locationNameText;
        [SerializeField] private TextMeshProUGUI _locationNumberText;
        [SerializeField] private TextMeshProUGUI _starProgressText;
        [SerializeField] private Slider _starProgressSlider;
        [SerializeField] private Transform _previewContainer;
        [SerializeField] private SimpleScrollSnap _scrollSnap;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _levelsButton;

        private Pool<Image> _previewPool;
        private int _locationIndex;

        private readonly Holder<LevelController> levelController = new();

        private void Awake()
        {
            _previewPool = new(_previewPrefab, _previewContainer);
        }

        private void Start()
        {
            _scrollSnap.OnPanelSelected.AddListener(OnPanelSelected);
            _playButton.onClick.AddListener(OnPlayButtonClick);
            _levelsButton.onClick.AddListener(OnLevelsButtonClick);
        }

        public void Display(int locationIndex)
        {
            _locationIndex = locationIndex;

            _previewPool.Clear();
            for (int index = 0; index < Configs.Gamebox.LocationsAmount; index++)
                _previewPool.Get().sprite = Configs.Gamebox.GetLocationPreviewSprite(index);

            _scrollSnap.StartingPanel = locationIndex;

            DisplayLocationInfo(locationIndex);
        }

        private void OnLevelsButtonClick()
        {
            ScreenFade.Fade(onCompleted: () =>
            {
                levelController.Item.ShowLevelsScreen(_locationIndex);
                levelController.Item.HideLocationsScreen();
            });
            
        }

        private void OnPlayButtonClick()
        {
            throw new NotImplementedException();
        }


        private void DisplayLocationInfo(int locationIndex)
        {
            _locationNameText.text = Configs.Gamebox.GetLocationName(locationIndex);

            _locationNumberText.text = Localization.GetLocalizedText
                ("location_number").Replace("{NUMBER}", (locationIndex + 1).ToString());

            int maxStars = StarsCalculator.GetLocationMaxStars(locationIndex);
            int currentStars = StarsCalculator.GetLocationCurrentStars(locationIndex);

            _starProgressText.text = $"{currentStars}/{maxStars}";
            _starProgressSlider.value = (float)currentStars / maxStars;

        }

        private void OnPanelSelected(int index)
        {
            int locationIndex = _scrollSnap.CenteredPanel;
            _locationIndex = locationIndex;
            DisplayLocationInfo(locationIndex);
        }


    }
}