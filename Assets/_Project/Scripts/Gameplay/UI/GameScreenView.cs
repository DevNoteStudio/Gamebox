using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using DevNote;
using DevNote.Modules.Levels;

public class GameScreenView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _locationNameText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private List<Image> _stars;
    [SerializeField] private Button _winWith1StarButton;
    [SerializeField] private Button _winWith2StarButton;
    [SerializeField] private Button _winWith3StarButton;
    [SerializeField] private Button _levelsButton;
    [SerializeField] private LocationsView _locations;

    [Inject] private readonly LevelController levelController;

    private void Start()
    {
        _winWith1StarButton.onClick.AddListener(WinWith1Star);
        _winWith2StarButton.onClick.AddListener(WinWith2Star);
        _winWith3StarButton.onClick.AddListener(WinWith3Star);
        _levelsButton.onClick.AddListener(OnLevelsButtonClicked);
        ChangeContent();
    }

    private void OnEnable()
    {
        GameState.CurrentLevel.OnChanged += ChangeContent;
    }

    private void OnDisable()
    {
        GameState.CurrentLevel.OnChanged -= ChangeContent;
    }

    private void WinWith1Star()
    {
        levelController.Win(1);
    }

    private void WinWith2Star()
    {
        levelController.Win(2);
    }

    private void WinWith3Star()
    {
        levelController.Win(3);
    }

    private void OnLevelsButtonClicked()
    {
        _locations.gameObject.SetActive(true);
    }

    private void ChangeContent()
    {
        _locationNameText.text = Localization.GetLocalizedText(
            Configs.Locations.List[GameState.CurrentLocation.Value].Type.ToString());

        _levelText.text = Localization.GetLocalizedText("Level") + " " + (GameState.CurrentLevel.Value + 1);

        int starCount = GameState.CompletedLevels.Value[GameState.CurrentLocation.Value][GameState.CurrentLevel.Value];

        for (int i = 0; i < _stars.Count; i++)
            _stars[i].color = i < starCount ? Color.yellow : Color.black;
    }
}
