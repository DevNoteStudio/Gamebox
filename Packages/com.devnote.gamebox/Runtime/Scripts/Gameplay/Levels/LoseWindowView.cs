using System;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Gamebox
{
    public class LoseWindowView : MonoBehaviour
    {
        [SerializeField] private RectTransform _windowRect;
        [SerializeField] private RectTransform _titleRect;
        [SerializeField] private Button _reviveButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _bottomRestartButton;

        private Tween _currentTween;

        private const float SHOW_TITLE_TO_LOCAL_Y = 275f;
        private const float TITLE_SHOW_DURATION = 0.6f;
        private const float TITLE_MOVE_DURATION = 0.4f;
        private const float DELAY_BEFORE_SHOW_SKIP_BUTTON = 2.5f;
        private const float HIDE_DURATION = 0.3f;

        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<IAds> ads = new();


        private void Start()
        {
            _restartButton.onClick.AddListener(OnRestartButtonClick);
            _bottomRestartButton.onClick.AddListener(OnRestartButtonClick);
            _reviveButton.onClick.AddListener(OnReviveButtonClick);
        }

        public LoseWindowView Display()
        {
            bool reviveAvailable = IConfigs.Gamebox.ReviveAvailable;

            _bottomRestartButton.gameObject.SetActive(reviveAvailable);
            _reviveButton.gameObject.SetActive(reviveAvailable);
            _restartButton.gameObject.SetActive(!reviveAvailable);

            return this;
        }



        public void AnimateShow()
        {
            IConfigs.AudioHub.LoseShow?.Play();

            _windowRect.localScale = Vector3.zero;
            _bottomRestartButton.transform.localScale = Vector3.zero;
            _titleRect.localPosition = Vector3.zero;
            _titleRect.localScale = new Vector3(0f, 1f, 1f);

            _currentTween?.Kill();
            _currentTween = DOTween.Sequence().Attach(gameObject)

                .Append(_titleRect.DOScaleX(1f, TITLE_SHOW_DURATION).SetEase(Ease.OutBack))

                .AppendCallback(() => IConfigs.AudioHub.ShowWindow?.Play())
                .Append(TweenHub.Show(_windowRect))
                .Join(_titleRect.DOLocalMoveY(SHOW_TITLE_TO_LOCAL_Y, TITLE_MOVE_DURATION).SetEase(Ease.InOutFlash))
                

                .AppendInterval(DELAY_BEFORE_SHOW_SKIP_BUTTON)
                .Append(TweenHub.Show(_bottomRestartButton.transform));
        }

        public void AnimateHide(Action onCompleted)
        {
            _reviveButton.GetComponent<IAnimation>().Stop();

            _currentTween?.Kill();
            _currentTween = DOTween.Sequence().Attach(gameObject)
                .Append(_windowRect.DOScale(0f, HIDE_DURATION).SetEase(Ease.OutFlash))
                .Join(_titleRect.DOScale(0f, HIDE_DURATION).SetEase(Ease.OutFlash))
                .OnComplete(() => onCompleted?.Invoke());
        }


        private void OnReviveButtonClick()
        {
            ads.Item.ShowRewarded(AdKey.LevelRevive, onRewarded: () =>
            {
                levelController.Item.Revive();
                levelController.Item.HideLoseWindow(forceHide: false);
            }); 
        }

        private void OnRestartButtonClick()
        {
            UI.ScreenFade(onCompleted: () =>
            {
                int locationIndex = levelController.Item.CurrentLocationIndex;
                int levelIndex = levelController.Item.CurrentLevelIndex;

                levelController.Item.StartLevel(IGameState.Level, isRestart: true);
                levelController.Item.HideLoseWindow(forceHide: true);
            });
        }


    }

}

