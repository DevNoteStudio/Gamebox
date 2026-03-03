using System;
using DevNote;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class PauseWindowView : MonoBehaviour
    {
        [SerializeField] private WindowAnimation _windowAnimation;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;

        private readonly Holder<PauseController> pauseController = new();
        private readonly Holder<MenuController> menuController = new();
        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<IEnvironment> environment = new();


        private void Start()
        {
            Debug.Log(name);
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            _restartButton.onClick.AddListener(OnRestartButtonClick);
            _menuButton.onClick.AddListener(OnMenuButtonClick);
        }

        public PauseWindowView Display()
        {
            //_menuButton.gameObject.SetActive(IConfigs.Gamebox.MenuAvailable);
            return this;
        }

        public void AnimateShow() => _windowAnimation.AnimateShow();

        public void AnimateHide(Action onCompleted) => _windowAnimation.AnimateHide(onCompleted);


        private void OnMenuButtonClick()
        {
            UI.ScreenFade(onCompleted: () =>
            {
                int locationIndex = levelController.Item.CurrentLocationIndex;

                pauseController.Item.HidePauseWindow(force: true);
                levelController.Item.ExitLevel();
                menuController.Item.ShowLocationsScreen(locationIndex);
            });
        }

        private void OnRestartButtonClick()
        {
            UI.ScreenFade(onCompleted: () =>
            {
                int levelIndex = levelController.Item.CurrentLevelIndex;
                int locationIndex = levelController.Item.CurrentLocationIndex;

                pauseController.Item.HidePauseWindow(force: true);
                levelController.Item.StartLevel(IGameState.Level, isRestart: true);
            });
        }

        private void OnCloseButtonClick()
        {
            if (levelController.Item.IsLevelPlaying)
                environment.Item.StartGameplay();

            pauseController.Item.HidePauseWindow();
        }


    }
}

