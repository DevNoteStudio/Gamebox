using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DevNote.Modules.Levels
{
    public class LevelsScreenView : MonoBehaviour
    {
        [SerializeField] private LevelWidgetView _levelWidgetPrefab;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Button _closeButton;
        [SerializeField] private RectTransform _levelWidgetContainer;

        private List<LevelWidgetView> _levelWidgets = new();
        private int _locationIndex;

        [Inject] private readonly ScreenController screenController;


        private void Start()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        public void Display(int locationIndex)
        {
            _locationIndex = locationIndex;
            var locationData = Configs.Levels.LocationDataList[locationIndex];
            _backgroundImage.color = locationData.backgroundColor;

            foreach (var levelWidget in _levelWidgets)
                levelWidget.gameObject.SetActive(false);

            for (int levelIndex = 0; levelIndex < locationData.levelsAmount; levelIndex++)
                DisplayLevelWidget(locationIndex, levelIndex);


        }

        private void OnCloseButtonClick()
        {
            screenController.HideLevelsScreen();
            screenController.ShowLocationsScreen(_locationIndex);
        }


        private void DisplayLevelWidget(int locationIndex, int levelIndex)
        {
            if (_levelWidgets.Count == 0)
                _levelWidgets.Add(_levelWidgetPrefab);

            while (_levelWidgets.Count <= levelIndex)
            {
                var widget = SceneInjector.InstantiateFromPrefabComponent(_levelWidgetPrefab, _levelWidgetContainer);
                _levelWidgets.Add(widget);
            }

            _levelWidgets[levelIndex].Display(locationIndex, levelIndex);
        }

    }
}