using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocationsView : MonoBehaviour
{
    private const float ANIMATION_DURATION = 0.4f;

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private RectTransform _contentParent;
    [SerializeField] private TextMeshProUGUI _starCountText;
    [SerializeField] private TextMeshProUGUI _locationNameText;
    [SerializeField] private RectTransform _difficultyItemsShadowParent;
    [SerializeField] private RectTransform _difficultyItemsParent;
    [SerializeField] private Image _levelExampleImage;
    [SerializeField] private Button _previousLocationButton;
    [SerializeField] private Button _nextLocationButton;
    [SerializeField] private TextMeshProUGUI _completedLevelsText;
    [SerializeField] private Button _levelsButton;
    [SerializeField] private Button _playButton;

    private int _currentLocationIndex = 0;
    private Tween _backgroundTween;
    private Tween _contentTween;

    private void Start()
    {
        _previousLocationButton.onClick.AddListener(OnPreviousLevelButtonClicked);
        _nextLocationButton.onClick.AddListener(OnNextLevelButtonClicked);
        _levelsButton.onClick.AddListener(OnLevelsButtonClicked);
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        GameState.StarCount.OnChanged += OnStarCountChanged;

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
        _starCountText.text = GameState.StarCount.Value.ToString();
        _locationNameText.text = Localization.GetLocalizedText(data.Type.ToString());

        foreach (Transform child in _difficultyItemsShadowParent)
            Destroy(child.gameObject);
        
        foreach (Transform child in _difficultyItemsParent)
            Destroy(child.gameObject);

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
            item.fillAmount = data.Difficulty - i;
        }

        _levelExampleImage.sprite = data.Image;
        _completedLevelsText.text = $"{0} / {data.LevelCount}";
        _completedLevelsText.text = $"{GameState.CompletedLevels.Value[_currentLocationIndex]} / {data.LevelCount}";
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
        
    }

    private void OnPlayButtonClicked()
    {

    }

    private void OnStarCountChanged()
    {
        _starCountText.text = GameState.StarCount.Value.ToString();
    }
}
