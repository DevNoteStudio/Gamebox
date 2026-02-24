using DevNote;
using UnityEngine;
using UnityEngine.UI;


namespace Gamebox
{
    public class GameplayScreenView : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;

        [field: SerializeField] public ScoreView ScoreView { get; private set; }


        private readonly Holder<PauseController> pauseController = new();


        private void Start()
        {
            _pauseButton.onClick.AddListener(OnPauseButtonClick);
        }

        public void Display(int levelIndex)
        {

        }

        private void OnPauseButtonClick()
        {
            pauseController.Item.ShowPauseWindow();
        }


        


        




    }
}
