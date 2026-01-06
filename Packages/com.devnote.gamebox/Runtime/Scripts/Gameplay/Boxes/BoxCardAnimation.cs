using DG.Tweening;
using UnityEngine;

namespace Gamebox
{
    public class BoxCardAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform _targetRect;
        [SerializeField] private RectTransform _titleRect;
        [SerializeField] private RectTransform _upgradeAvailableRect;


        private const float SCALE = 1.7f;
        private const float START_POSITION_Y = -500f;
        private const float ROTATE_DURATION = 0.8f;
        private const float SHOW_NAME_DURATION = 0.5f;



        private Tween _tween;


        public void AnimateShow(RarityType rarityType)
        {
            transform.SetParent(_targetRect);
            transform.localPosition = Vector3.up * START_POSITION_Y;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.zero;
            _titleRect.localScale = Vector3.zero;
            _upgradeAvailableRect.localScale = Vector3.zero;

            _tween?.Kill();
            _tween = DOTween.Sequence()
                .Append(transform.DOLocalRotate(Vector3.up * 360f, ROTATE_DURATION, RotateMode.LocalAxisAdd))
                .Join(transform.DOLocalMove(Vector3.zero, ROTATE_DURATION).SetEase(Ease.OutFlash))
                .Join(transform.DOScale(SCALE, ROTATE_DURATION).SetEase(Ease.OutFlash))
                .Append(_titleRect.DOScale(1f, SHOW_NAME_DURATION).SetEase(Ease.OutBack))
                .Append(_upgradeAvailableRect.DOScale(1f, SHOW_NAME_DURATION).SetEase(Ease.OutBack));


        }



    }
}
