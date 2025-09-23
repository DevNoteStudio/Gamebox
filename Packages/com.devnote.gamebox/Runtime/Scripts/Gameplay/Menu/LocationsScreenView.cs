using DanielLochner.Assets.SimpleScrollSnap;
using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class LocationsScreenView : MonoBehaviour
    {
        [SerializeField] private Image _previewPrefab;
        [SerializeField] private TextMeshProUGUI _locationNameText;
        [SerializeField] private TextMeshProUGUI _locationNumberText;
        [SerializeField] private TextMeshProUGUI _starProgressText;
        [SerializeField] private TextMeshProUGUI _completedLevelsText;
        [SerializeField] private TextMeshProUGUI _lockedText;
        [SerializeField] private Slider _starProgressSlider;
        [SerializeField] private Transform _previewContainer;
        [SerializeField] private SimpleScrollSnap _scrollSnap;
        [SerializeField] private GameObject _lockObject;
        [SerializeField] private GameObject _availableObject;
        [SerializeField] private Button _previousButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _levelsButton;

        private Pool<Image> _previewPool;
        private int _locationIndex;

        private readonly Holder<MenuController> menuController = new();
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
            for (int index = 0; index < IConfigs.Gamebox.LocationsAmount; index++)
                _previewPool.Get().sprite = IConfigs.Gamebox.GetLocationPreviewSprite(index);

            _scrollSnap.StartingPanel = locationIndex;

            DisplayLocationInfo(locationIndex);
        }

        private void OnLevelsButtonClick()
        {
            ScreenFade.Fade(onCompleted: () =>
            {
                menuController.Item.ShowLevelsScreen(_locationIndex);
                menuController.Item.HideLocationsScreen();
            });
            
        }

        private void OnPlayButtonClick()
        {
            ScreenFade.Fade(onCompleted: () =>
            {
                int levelIndex = IGameState.Levels.GetLastLevelIndexForPlay(_locationIndex);

                menuController.Item.HideLocationsScreen();
                levelController.Item.StartLevel(_locationIndex, levelIndex);
            });

        }


        private void DisplayLocationInfo(int locationIndex)
        {
            _locationNameText.text = IConfigs.Gamebox.GetLocationName(locationIndex);

            _locationNumberText.text = Localization.GetLocalizedText
                ("location_number").Replace("{NUMBER}", (locationIndex + 1).ToString());

            int maxStars = IGameState.Levels.GetLocationMaxStars(locationIndex);
            int currentStars = IGameState.Levels.GetLocationCurrentStars(locationIndex);

            _starProgressText.text = $"{currentStars}/{maxStars}";
            _starProgressSlider.value = (float)currentStars / maxStars;

            _previousButton.interactable = locationIndex != 0;
            _nextButton.interactable = locationIndex != IConfigs.Gamebox.LocationsAmount - 1;

            int completedLevels = IGameState.Levels.GetCompletedLevels(locationIndex);
            int levelsAmount = IConfigs.Gamebox.GetLocationLevelsAmount(locationIndex);

            _completedLevelsText.text = Localization.GetLocalizedText("location_progress")
                .Replace("{CURRENT}", completedLevels.ToString())
                .Replace("{MAX}", levelsAmount.ToString());

            int starsRequire = IConfigs.Gamebox.GetLocationStarRequire(locationIndex);
            bool locationAvailable = IGameState.Items.Value(IItemKey.Stars) >= starsRequire;

            _lockObject.SetActive(!locationAvailable);
            _availableObject.SetActive(locationAvailable);
            _playButton.gameObject.SetActive(locationAvailable);
            _levelsButton.gameObject.SetActive(locationAvailable);

            _lockedText.text = Localization.GetLocalizedText("location_locked")
                .Replace("{STARS}", starsRequire.ToString());
        }

        private void OnPanelSelected(int index)
        {
            int locationIndex = _scrollSnap.CenteredPanel;
            _locationIndex = locationIndex;
            DisplayLocationInfo(locationIndex);
        }


    }
}