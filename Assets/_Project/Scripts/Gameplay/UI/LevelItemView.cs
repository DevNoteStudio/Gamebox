using DevNote;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelItemView : MonoBehaviour
{
    [SerializeField] private RectTransform _starsParent;
    [SerializeField] private TextMeshProUGUI _levelNumberText;
    [SerializeField] private Button _playButton;

    [Inject] private readonly LevelController levelController;

    private int _locationNumber;
    private int _levelNumber;
    private List<Image> _stars;

    private void Start()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    public void Display(int locationNumber, int levelNumber, bool isActive)
    {
        _locationNumber = locationNumber;
        _levelNumber = levelNumber;
        _levelNumberText.text = (levelNumber + 1).ToString();

        _stars = _starsParent.GetComponentsInChildren<Image>(true).ToList();
        int stars = GameState.CompletedLevels.Value[locationNumber][levelNumber];

        for (int i = 0; i < _stars.Count; i++)
            _stars[i].color = i < stars ? Color.yellow : Color.black;

        if (isActive)
        {
            _playButton.interactable = true;
            _starsParent.gameObject.SetActive(true);
        }
        else
        {
            _playButton.interactable = false;
            _starsParent.gameObject.SetActive(false);
        }
    }

    private void OnPlayButtonClicked()
    {
        levelController.StartLevel(_locationNumber, _levelNumber);
    }
}
