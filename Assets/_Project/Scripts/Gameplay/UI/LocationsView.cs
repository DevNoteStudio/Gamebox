using DevNote;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LocationsView : MonoBehaviour
{
    private const float ANIMATION_DURATION = 0.4f;

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private RectTransform _contentParent;
    [SerializeField] private TextMeshProUGUI _starCountText;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _locationNameText;
    [SerializeField] private RectTransform _difficultyItemsShadowParent;
    [SerializeField] private RectTransform _difficultyItemsParent;
    [SerializeField] private Image _levelExampleImage;
    [SerializeField] private Button _previousLocationButton;
    [SerializeField] private Button _nextLocationButton;
    [SerializeField] private TextMeshProUGUI _completedLevelsText;
    [SerializeField] private Button _levelsButton;
    [SerializeField] private Button _playButton;

    [Inject] private readonly LevelController levelController;

    private List<Image> _difficultyItems = new List<Image>();
    private int _currentLocationIndex = 0;
    private Tween _backgroundTween;
    private Tween _contentTween;

    private void Awake()
    {
        for (int i = 0; i < Configs.Locations.MaxDifficulty; i++)
        {
            var shadowItem = SceneInjector.InstantiateFromPrefabComponent(
                Configs.Locations.DifficultyItemPrefab,
                _difficultyItemsShadowParent);

            shadowItem.color = Color.black;
            shadowItem.fillAmount = 1f;

            var item = SceneInjector.InstantiateFromPrefabComponent(
                Configs.Locations.DifficultyItemPrefab,
                _difficultyItemsParent);

            item.color = Color.white;
            _difficultyItems.Add(item);
        }
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        _previousLocationButton.onClick.AddListener(OnPreviousLevelButtonClicked);
        _nextLocationButton.onClick.AddListener(OnNextLevelButtonClicked);
        _levelsButton.onClick.AddListener(OnLevelsButtonClicked);
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        levelController.OnLevelStarted += () => gameObject.SetActive(false);
        GameState.StarCount.OnChanged += OnStarCountChanged;
    }

    private void OnEnable()
    {
        Display();
    }

    private void OnDisable()
    {
        _backgroundTween?.Kill();
        _contentTween?.Kill();
    }

    private void Display()
    {
        var data = Configs.Locations.List[_currentLocationIndex];

        _backgroundImage.color = new Color(data.BackgroundColor.r, data.BackgroundColor.g, data.BackgroundColor.b, 0f);
        _backgroundTween = _backgroundImage.DOFade(1f, ANIMATION_DURATION);
        _contentParent.localScale = Vector3.zero;
        _contentTween = _contentParent.DOScale(Vector3.one, ANIMATION_DURATION).SetEase(Ease.OutBack);

        ChangeContent(data);
    }

    private void OnLocationChanged()
    {
        var data = Configs.Locations.List[_currentLocationIndex];

        _backgroundTween = _backgroundImage.DOColor(data.BackgroundColor, ANIMATION_DURATION);

        ChangeContent(data);
    }

    private void ChangeContent(LocationData data)
    {
        for (int i = 0; i < _difficultyItems.Count; i++)
            _difficultyItems[i].fillAmount = data.Difficulty - i;

        _starCountText.text = GameState.StarCount.Value.ToString();
        _locationNameText.text = Localization.GetLocalizedText(data.Type.ToString());
        _levelExampleImage.sprite = data.Image;

        if (GameState.CompletedLevels.Value[_currentLocationIndex].FindIndex(stars => stars == 0) == -1)
        {
            _completedLevelsText.text =
                $"{data.LevelCount} / {data.LevelCount}";
        }
        else
        {
            _completedLevelsText.text =
                $"{GameState.CompletedLevels.Value[_currentLocationIndex].FindIndex(stars => stars == 0)} / {data.LevelCount}";
        }
    }

    private void OnPreviousLevelButtonClicked()
    {
        if (--_currentLocationIndex < 0)
            _currentLocationIndex = Configs.Locations.List.Count - 1;

        OnLocationChanged();
    }

    private void OnNextLevelButtonClicked()
    {
        _currentLocationIndex = (_currentLocationIndex + 1) % Configs.Locations.List.Count;
        OnLocationChanged();
    }

    private void OnLevelsButtonClicked()
    {
        if (GameState.CompletedLevels.Value[_currentLocationIndex].FindIndex(stars => stars == 0) == -1)
        {
            var levelsView = SceneInjector.InstantiateFromPrefabComponent(Configs.LevelsUI.LevelsViewPrefab, transform.parent);
            int lastAvailableLevel = Configs.Locations.List[_currentLocationIndex].LevelCount - 1;
            levelsView.Display(_currentLocationIndex, lastAvailableLevel);
        }
        else
        {
            SceneInjector.InstantiateFromPrefabComponent(Configs.LevelsUI.LevelsViewPrefab, transform.parent).
                Display(
                    _currentLocationIndex,
                    GameState.CompletedLevels.Value[_currentLocationIndex].FindIndex(stars => stars == 0));
        }
    }

    private void OnPlayButtonClicked()
    {
        if (GameState.CompletedLevels.Value[_currentLocationIndex].FindIndex(stars => stars == 0) == -1)
        {
            levelController.StartLevel(_currentLocationIndex, Configs.Locations.List[_currentLocationIndex].LevelCount - 1);
        }
        else
        {
            levelController.StartLevel(_currentLocationIndex,
                GameState.CompletedLevels.Value[_currentLocationIndex].FindIndex(stars => stars == 0));
        }
    }

    private void OnStarCountChanged()
    {
        _starCountText.text = GameState.StarCount.Value.ToString();
    }
}
