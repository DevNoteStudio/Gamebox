using Coffee.UIExtensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class BoxView : MonoBehaviour
    {
        [SerializeField] private Image _closedImage;
        [SerializeField] private Image _openedImage;
        [SerializeField] private Image _openedFrontImage;
        [SerializeField] private UIParticle _shineParticle;
        [SerializeField] private UIParticle _flashParticle;
        [SerializeField] private RectTransform _startPositionRect;
        [SerializeField] private RectTransform _openedPositionRect;

        private RectTransform RectTransform => transform as RectTransform;

        private Tween _currentTween;


        private const float SHOW_DURATION = 0.8f;
        private const float MOVE_DURATION = 1f;
        private const float MOVED_BOX_SCALE = 0.7f;
        private const float SHAKE_DURATION = 0.7f;
        private const float BOX_PUSH_DURATION = 0.5f;


        public void Display(ItemKey boxItemKey)
        {
            _closedImage.LoadSprite(AssetLoader.LoadBoxSprite(boxItemKey, AssetLoader.BoxSpriteType.Closed));
            _openedImage.LoadSprite(AssetLoader.LoadBoxSprite(boxItemKey, AssetLoader.BoxSpriteType.Opened));
            _openedFrontImage.LoadSprite(AssetLoader.LoadBoxSprite(boxItemKey, AssetLoader.BoxSpriteType.FrontOpened));

            _closedImage.gameObject.SetActive(true);
            _openedImage.gameObject.SetActive(false);
            _openedFrontImage.gameObject.SetActive(false);
            _shineParticle.gameObject.SetActive(false);

        }

        public void AnimateShow()
        {
            RectTransform.SetParent(_startPositionRect);
            RectTransform.localPosition = Vector3.zero;
            RectTransform.localScale = Vector3.zero;

            _currentTween = RectTransform.DOScale(1f, SHOW_DURATION).SetEase(Ease.OutBack);
            _currentTween.OnKill(() => RectTransform.localScale = Vector3.one);

        }


        public void AnimateOpen()
        {
            RectTransform.SetParent(_openedPositionRect);

            _currentTween?.Kill();
            _currentTween = DOTween.Sequence()
                .Append(RectTransform.DOShakeAnchorPos(SHAKE_DURATION, strength: 55, vibrato: 20, fadeOut: false))
                .AppendCallback(() =>
                {
                    _closedImage.gameObject.SetActive(false);
                    _openedImage.gameObject.SetActive(true);
                    _openedFrontImage.gameObject.SetActive(true);
                    _shineParticle.gameObject.SetActive(true);
                    _flashParticle.Play();
                })
                .Append(RectTransform.DOLocalMove(Vector3.zero, MOVE_DURATION).SetEase(Ease.OutFlash))
                .Join(RectTransform.DOScale(MOVED_BOX_SCALE, MOVE_DURATION).SetEase(Ease.OutFlash));

            _currentTween.OnKill(() => RectTransform.localPosition = Vector3.zero);
        }

        public void AnimatePushCard()
        {
            _flashParticle.Play();

            _currentTween?.Kill();
            _currentTween = RectTransform.DOLocalJump(Vector3.zero, jumpPower: 150, numJumps: 1, BOX_PUSH_DURATION);
        }


    }
}
