using DevNote;
using UnityEngine;
using UnityEngine.UI;


namespace Gamebox
{
    public class GameplayScreenView : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;

        [field: SerializeField] public ScoreView ScoreView { get; private set; }
        [field: SerializeField] public CurrencyView CurrencyView { get; private set; }
        [field: SerializeField] public BoosterPanelView BoosterPanel { get; private set; }
        [field: SerializeField] public LeadersButtonView LeadersButton { get; private set; }


        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<PauseController> pauseController = new();



        private void Start()
        {
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
            
            LeadersButton.gameObject.SetActive(IConfigs.Gamebox.IsAvailable(UnlockKey.Leaderboard));
            BoosterPanel.gameObject.SetActive(IGameState.Level >= IConfigs.Gamebox.BoosterPanelFromLevel);

        }

        private void OnPauseButtonClick()
        {
            pauseController.Item.ShowPauseWindow();
        }


        


        




    }
}
