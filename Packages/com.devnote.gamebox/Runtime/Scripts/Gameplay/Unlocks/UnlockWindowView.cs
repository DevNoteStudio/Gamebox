using System;
using Coffee.UIExtensions;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class UnlockWindowView : MonoBehaviour
    {
        [SerializeField] private UnlockSliderView _unlockSlider;
        [SerializeField] private Image _centerIconImage;
        [SerializeField] private RectTransform _centerRect;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private RectTransform _continueRect;
        [SerializeField] private Button _closeButton;
        [SerializeField] private RectTransform _contentRect;
        [SerializeField] private UIParticle _showParticle;


        private Tween _currentTween;

        private readonly Holder<UnlockController> unlockController = new();


        private readonly Vector2 BOTTOM_SLIDER_POSITION = new Vector2(0f, -450f);
        private readonly Vector2 CENTER_MOVE_FROM_TO_Y = new Vector2(-250f, 250f);


        private void Start()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        private void OnCloseButtonClick()
        {
            _closeButton.interactable = false;
            unlockController.Item.HideUnlockWindow();
        }

        public void AnimateDisplay(UnlockKey previuosKey, UnlockKey currentKey, UnlockKey nextKey)
        {
            const float SHOW_SLIDER_DURATION = 0.5f;
            const float MOVE_SLIDER_DURATION = 0.8f;
            const float TIME_INTERVAL_ON_SLIDER = 0.4f;
            const float SLIDER_TO_SCALE = 0.8f;

            const float CENTER_MOVE_DURATION = 1f;
            const float TEXT_SHOW_DURATION = 0.6f;
            const float DELAY_BEFORE_CONTINUE = 2.5f;


            _unlockSlider.Display(previuosKey, currentKey, nextKey);
            _centerIconImage.LoadSprite(AssetLoader.LoadUnlockSprite(currentKey));
            _nameText.text = IConfigs.Gamebox.GetUnlockName(currentKey);
            _descriptionText.text = IConfigs.Gamebox.GetUnlockDescription(currentKey);


            // <-- Set initial values -->
            _closeButton.interactable = false;
            _contentRect.transform.localScale = Vector3.one;
            _unlockSlider.transform.localScale = Vector3.zero;
            _unlockSlider.transform.localPosition = Vector3.zero;
            _centerRect.localPosition = _centerRect.localPosition.SetY(CENTER_MOVE_FROM_TO_Y.x);
            _centerRect.transform.localScale = Vector3.zero;
            _nameText.transform.localScale = Vector3.zero;
            _descriptionText.transform.localScale = Vector3.zero;
            _continueRect.transform.localScale = Vector3.zero;

            IConfigs.AudioHub.ShowWindow?.Play();

            // <-- Animation -->
            _currentTween?.Kill();
            _currentTween = DOTween.Sequence()
                .Append(_unlockSlider.transform.DOScale(1f, SHOW_SLIDER_DURATION).SetEase(Ease.OutBack))
                .Append(_unlockSlider.AnimateUnlockNewContent())
                .AppendInterval(TIME_INTERVAL_ON_SLIDER)

                // Move slider to bottom
                .Append(_unlockSlider.transform.DOLocalMove(BOTTOM_SLIDER_POSITION, MOVE_SLIDER_DURATION).SetEase(Ease.OutFlash))
                .Join(_unlockSlider.transform.DOScale(SLIDER_TO_SCALE, MOVE_SLIDER_DURATION).SetEase(Ease.OutFlash))

                // Show center icon
                .AppendCallback(() =>
                {
                    IConfigs.AudioHub.UnlockWindowShowCenterIcon?.Play();
                    _showParticle.Play();
                })
                .Append(_centerRect.DOLocalMoveY(CENTER_MOVE_FROM_TO_Y.y, CENTER_MOVE_DURATION).SetEase(Ease.OutBack))
                .Join(_centerRect.DOScale(1f, CENTER_MOVE_DURATION).SetEase(Ease.OutBack))

                // Show name and description
                .AppendCallback(() => IConfigs.AudioHub.ShowElement?.Play())
                .Append(_nameText.transform.DOScale(1f, TEXT_SHOW_DURATION).SetEase(Ease.OutFlash))
                .Join(_descriptionText.transform.DOScale(1f, TEXT_SHOW_DURATION).SetEase(Ease.OutFlash))

                // Show continue
                .AppendInterval(DELAY_BEFORE_CONTINUE)
                .AppendCallback(() => _closeButton.interactable = true)
                .Append(_continueRect.transform.DOScale(1f, TEXT_SHOW_DURATION).SetEase(Ease.OutBack));
        }



        public void AnimateHide(Action onCompleted)
        {
            TweenHub.Hide(_contentRect, playSound: true).OnComplete(() => onCompleted?.Invoke());
        }


        




    }
}
