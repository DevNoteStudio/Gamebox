using System;
using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Gamebox
{
    public class GameplayScreenView : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _leadersButton;
        [SerializeField] private TextMeshProUGUI _rankText;

        [field: SerializeField] public ScoreView ScoreView { get; private set; }
        [field: SerializeField] public CurrencyView CurrencyView { get; private set; }

        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<PauseController> pauseController = new();
        private readonly Holder<LeadersController> leadersController = new();


        private void Start()
        {
            _leadersButton.onClick.AddListener(OnLeadersButtonClick);
            _pauseButton.onClick.AddListener(OnPauseButtonClick);
        }

        private void OnEnable()
        {
            levelController.Item.OnLevelStarted += Display;
        }

        private void OnDisable()
        {
            levelController.Item.OnLevelStarted -= Display;
        }

        private void Display()
        {
            CurrencyView.gameObject.SetActive(IGameState.Level >= 2);
            _rankText.text = IConfigs.Leaderboard.GetRank(IGameState.Level).ToString();
        }

        private void OnLeadersButtonClick()
        {
            leadersController.Item.ShowLeadersWindow();
        }

        private void OnPauseButtonClick()
        {
            pauseController.Item.ShowPauseWindow();
        }


        


        




    }
}
