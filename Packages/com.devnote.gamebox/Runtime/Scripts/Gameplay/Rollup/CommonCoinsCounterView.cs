using System;
using DG.Tweening;
using UnityEngine;

namespace Gamebox
{
    public class CommonCoinsCounterView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector2 _secondPosition;
        [field: SerializeField] public ItemCounterView ItemCounter { get; private set; }

        private Vector2 _originPosition;  
        private Tween _currentTween;

        private const float MOVE_DURATION = 1.2f;
        private const float DELAY_DURATION = 5f;


        private void Awake()
        {
            _originPosition = _rectTransform.anchoredPosition;
        }


        public void AnimateShowAndHide(Action onCompleted)
        {
            _currentTween?.Kill();

            _currentTween = DOTween.Sequence()
                .Append(_rectTransform.DOAnchorPos(_secondPosition, MOVE_DURATION).SetEase(Ease.OutBack))
                .AppendInterval(DELAY_DURATION)
                .Append(_rectTransform.DOAnchorPos(_originPosition, MOVE_DURATION)).SetEase(Ease.OutBack)
                .OnComplete(() => onCompleted?.Invoke());
        }


    }
}
