using UnityEngine;
using UnityEngine.UI;

namespace DevNote.Gamebox
{
    public class LevelsScreenView : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private LevelWidgetView _levelWidgetPrefab;
        [SerializeField] private Transform _levelWidgetContainer;

        private Pool<LevelWidgetView> _levelWidgetPool;
        private int _locationIndex;

        private readonly Holder<MenuController> menuController = new();

        private void Awake()
        {
            _levelWidgetPool = new(_levelWidgetPrefab, _levelWidgetContainer);
        }

        private void Start()
        {
            _backButton.onClick.AddListener(OnBackButtonClick);
        }

        public void Display(int locationIndex)
        {
            _locationIndex = locationIndex;

            _levelWidgetPool.Clear();

            int levels = Configs.Gamebox.GetLocationLevelsAmount(locationIndex);

            for (int levelIndex = 0; levelIndex < levels; levelIndex++)
                _levelWidgetPool.Get().Display(locationIndex, levelIndex);
        }


        private void OnBackButtonClick()
        {
            ScreenFade.Fade(onCompleted: () =>
            {
                menuController.Item.HideLevelsScreen();
                menuController.Item.ShowLocationsScreen(_locationIndex);
            });
        }

    }
}