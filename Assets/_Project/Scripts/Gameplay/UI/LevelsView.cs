using DevNote;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsView : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private RectTransform _levelItemsParent;

    private void Start()
    {
        _closeButton.onClick.AddListener(() => Destroy(gameObject));
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
}