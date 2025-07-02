using DevNote;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelsView : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private RectTransform _levelItemsParent;

    [Inject] private readonly LevelController levelController;

    private void Start()
    {
        _closeButton.onClick.AddListener(() => Destroy(gameObject));
        levelController.LevelStarted += OnLevelStarted;
    }

    public void Display(int locationNumber, int lastAvailableLevel)
    {
        _backgroundImage.color = Configs.Locations.List[locationNumber].BackgroundColor;

        List<LevelItemView> levelItems = new List<LevelItemView>();

        for (int i = 0; i < Configs.Locations.List[locationNumber].LevelCount; i++)
        {
            levelItems.Add(SceneInjector.InstantiateFromPrefabComponent(Configs.LevelsUI.LevelItemPrefab, _levelItemsParent));

            levelItems[i].Display(locationNumber, i, i <= lastAvailableLevel);
        }
    }

    private void OnLevelStarted()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        levelController.LevelStarted -= OnLevelStarted;
    }
}