using DG.Tweening;
using UnityEngine;

namespace Gamebox
{
    public class ShineAnimation : MonoBehaviour
    {
        [SerializeField] private Vector2 _fromToAlpha;
        [SerializeField] private float _loopDuration;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Tween _tween;

        private void OnEnable()
        {
            var sequence = DOTween.Sequence().Attach(gameObject);

            _canvasGroup.alpha = _fromToAlpha.x;

            _tween?.Kill();
            _tween = DOTween.Sequence()
                .Append(_canvasGroup.DOFade(_fromToAlpha.y, _loopDuration / 2f).SetEase(Ease.InOutQuad))
                .Append(_canvasGroup.DOFade(_fromToAlpha.x, _loopDuration / 2f).SetEase(Ease.InOutQuad))
                .SetLoops(-1);
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }


    }
}


