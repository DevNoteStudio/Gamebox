using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class TestLevelView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelInfoText;
        [SerializeField] private Button _complete3Button;
        [SerializeField] private Button _complete2Button;
        [SerializeField] private Button _complete1Button;
        [SerializeField] private Button _loseButton;
        [SerializeField] private Button _exitButton;

        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<MenuController> menuController = new();

        private void Start()
        {
            _complete3Button.onClick.AddListener(OnComplete3ButtonClick);
            _complete2Button.onClick.AddListener(OnComplete2ButtonClick);
            _complete1Button.onClick.AddListener(OnComplete1ButtonClick);
            _loseButton.onClick.AddListener(OnLoseButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);
        }

        public void Display(int locationIndex, int levelIndex)
        {
            _levelInfoText.text = $"Location Index: {locationIndex}   Level Index: {levelIndex}";
        }


        private void OnComplete3ButtonClick()
        {
            levelController.Item.CompleteCurrentLevel(3);
        }

        private void OnComplete2ButtonClick()
        {
            levelController.Item.CompleteCurrentLevel(2);
        }

        private void OnComplete1ButtonClick()
        {
            levelController.Item.CompleteCurrentLevel(1);
        }

        private void OnExitButtonClick()
        {
            ScreenFade.Fade(onCompleted: () =>
            {
                levelController.Item.ExitLevel();
                menuController.Item.ShowLocationsScreen(levelController.Item.CurrentLocationIndex);
            });
            
        }

        private void OnLoseButtonClick()
        {
            levelController.Item.LoseCurrentLevel();
        }

        
    }
}

