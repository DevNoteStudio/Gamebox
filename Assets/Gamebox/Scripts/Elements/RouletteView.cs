using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace DevNote.Gamebox
{
    public class RouletteView : MonoBehaviour
    {
        [Serializable] private struct SectorData
        {
            public Image image;
            public Color originColor;
            public Color highlightColor;
        }


        [SerializeField] private SoundUnit _tickSound;
        [SerializeField] private RectTransform _pointer;
        [SerializeField] private RectTransform _spinAreaRect;
        [SerializeField] private float _leftRightPadding;
        [SerializeField] private List<SectorData> _sectors;

        private Tween _currentTween;
        private int _currentIndex = 0;

        private const float LOOP_DURATION = 1f;
        private const int SECTORS_AMOUNT = 5;

        public void StartSpin()
        {
            float rightX = _spinAreaRect.sizeDelta.x - _leftRightPadding;
            float leftX = _leftRightPadding;

            Vector2 rightAnchorPosition = _pointer.anchoredPosition.SetX(rightX);
            Vector2 leftAnchorPosition = _pointer.anchoredPosition.SetX(leftX);

            _pointer.anchoredPosition = leftAnchorPosition;

            _currentTween?.Kill();
            _currentTween = DOTween.Sequence().Attach(gameObject)
                .Append(_pointer.DOAnchorPos(rightAnchorPosition, LOOP_DURATION).SetEase(Ease.InOutFlash))
                .Append(_pointer.DOAnchorPos(leftAnchorPosition, LOOP_DURATION).SetEase(Ease.InOutFlash))
                .SetLoops(-1, LoopType.Restart);

            _currentTween.OnUpdate(() =>
            {
                int currentIndex = GetCurrentSecrorIndex();

                if (currentIndex != _currentIndex)
                {
                    _currentIndex = currentIndex;
                    _tickSound.Play();
                }

                for (int i = 0; i < _sectors.Count; i++)
                {
                    bool highlight = i == currentIndex;
                    _sectors[i].image.color = highlight ? _sectors[i].highlightColor : _sectors[i].originColor;
                }
            });

        }

        public void Stop(out int sectorIndex)
        {
            sectorIndex = GetCurrentSecrorIndex();
            _currentTween?.Kill();
        }


        private int GetCurrentSecrorIndex()
        {
            float sectorWidth = _spinAreaRect.sizeDelta.x / SECTORS_AMOUNT;
            float pointerX = _pointer.anchoredPosition.x;

            return (int)(pointerX / sectorWidth);
        }



    }
}


