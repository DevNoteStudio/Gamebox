using DG.Tweening;
using UnityEngine;

namespace Gamebox
{
    public class ScaleAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 _fromScale;
        [SerializeField] private Vector3 _toScale;
        [SerializeField] private float _duration;

        private Tween _tween;


        private void OnEnable()
        {
            transform.localScale = _fromScale;

            _tween?.Kill();
            _tween = DOTween.Sequence()
                .Append(transform.DOScale(_toScale, _duration).SetEase(Ease.InOutQuad))
                .Append(transform.DOScale(_fromScale, _duration).SetEase(Ease.InOutQuad))
                .SetLoops(-1);
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }



    }
}
