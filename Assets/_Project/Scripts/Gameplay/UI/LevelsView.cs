using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DevNote.Modules.Levels
{
    public class LevelsView : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private RectTransform _levelItemsParent;

        [Inject] private readonly LevelController levelController;

        private void Start()
        {
            _closeButton.onClick.AddListener(() => Destroy(gameObject));
        }

        private void OnEnable()
        {
            levelController.OnLevelStarted += OnLevelStarted;
        }

        private void OnDisable()
        {
            levelController.OnLevelStarted -= OnLevelStarted;
        }

        public void Display(int locationNumber, int lastAvailableLevel)
        {
            _backgroundImage.color = Configs.Locations.List[locationNumber].BackgroundColor;

            List<LevelItemView> levelItems = new List<LevelItemView>();

            for (int i = 0; i < Configs.Locations.List[locationNumber].LevelCount; i++)
            {
                LevelItemView levelItem =
                    SceneInjector.InstantiateFromPrefabComponent(Configs.LevelsUI.LevelItemPrefab, _levelItemsParent);

                levelItems.Add(levelItem);
                levelItem.Display(locationNumber, i, i <= lastAvailableLevel);
            }
        }

        private void OnLevelStarted()
        {
            Destroy(gameObject);
        }
    }
}