using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace DevNote.Gamebox
{
    public class LoseWindowView : MonoBehaviour
    {
        [SerializeField] private RectTransform _windowRect;
        [SerializeField] private RectTransform _titleRect;
        [SerializeField] private Button _reviveButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _skipButton;
        [SerializeField] private Image _fadeImage;

        private Tween _currentTween;

        private const float SHOW_TITLE_TO_LOCAL_Y = 275f;
        private const float TITLE_SHOW_DURATION = 0.6f;
        private const float TITLE_MOVE_DURATION = 0.4f;
        private const float DELAY_BEFORE_SHOW_SKIP_BUTTON = 2.5f;


        private void Start()
        {
            
        }


        public void Display(bool showRevive)
        {
            _skipButton.gameObject.SetActive(showRevive);
            _reviveButton.gameObject.SetActive(showRevive);
            _restartButton.gameObject.SetActive(!showRevive);
        }


        public void AnimateShow()
        {
            _windowRect.localScale = Vector3.zero;
            _skipButton.transform.localScale = Vector3.zero;
            _titleRect.localPosition = Vector3.zero;
            _titleRect.localScale = new Vector3(0f, 1f, 1f);
            _fadeImage.color = _fadeImage.color.SetAlpha(0f); 

            _currentTween?.Kill();
            _currentTween = DOTween.Sequence().Attach(gameObject)

                .Append(TweenHub.Fade(_fadeImage))
                .Join(_titleRect.DOScaleX(1f, TITLE_SHOW_DURATION).SetEase(Ease.OutBack))

                .Append(TweenHub.PopShow(_windowRect))
                .Join(_titleRect.DOLocalMoveY(SHOW_TITLE_TO_LOCAL_Y, TITLE_MOVE_DURATION).SetEase(Ease.InOutFlash))

                .AppendInterval(DELAY_BEFORE_SHOW_SKIP_BUTTON)
                .Append(TweenHub.PopShow(_skipButton.transform));
        }




    }

}

