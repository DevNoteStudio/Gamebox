using System;
using System.Collections.Generic;
using DevNote;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace Gamebox
{
    public class TutorialPointerView : MonoBehaviour
    {
        [SerializeField] private float _fadeAlpha;
        [SerializeField] private GameObject _pointerObject;
        [SerializeField] private Image _fadeImage;
        [SerializeField] private List<Image> _fadeFillImages;


        private const float FADE_SCREEN_UPSCALE_MULTIPLIER = 2f;
        private const float TO_SCALE_POINTER_ANIMATION = 0.8f;
        private const float POINTER_ANIMATION_DURATION = 1f;
        private const float FADE_DURATION = 0.5f;

        private Tween _pointerTween;
        private Tween _fadeTween;

        private Camera _mainCamera;


        private void Awake()
        {
            _mainCamera = Camera.main;
        }


        public void AnimatePointer(Transform target, Vector2 localPositionOffset, TutorialPointer.PointerType pointerType)
        {
            if (_fadeImage.gameObject.activeSelf) AnimateUnfade();

            _pointerObject.SetActive(true);
            _pointerObject.transform.localScale = Vector3.one;

            if (pointerType == TutorialPointer.PointerType.Up)
                _pointerObject.transform.rotation = Quaternion.Euler(0f, 0f, -90f);

            else if (pointerType == TutorialPointer.PointerType.Down)
                _pointerObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);


            _pointerTween?.Kill();
            _pointerTween = DOTween.Sequence()
                .Append(_pointerObject.transform.DOScale(TO_SCALE_POINTER_ANIMATION, POINTER_ANIMATION_DURATION / 2f).SetEase(Ease.OutFlash))
                .Append(_pointerObject.transform.DOScale(1f, POINTER_ANIMATION_DURATION / 2f).SetEase(Ease.OutFlash))
                .SetLoops(-1);

            _pointerTween.onUpdate += () =>
            {
                var position = WorldToCanvas(target, UI.Canvas);
                _pointerObject.transform.localPosition = position + localPositionOffset;
            };
        }



        public void AnimateFadePointer(Transform target, float viewportSize, Vector2 localPositionOffset, TutorialPointer.PointerType pointerType)
        {
            AnimatePointer(target, localPositionOffset, pointerType);

            _pointerTween.onUpdate += () =>
            {
                var position = WorldToCanvas(target, UI.Canvas);
                _fadeImage.rectTransform.localPosition = position;
            };

            _fadeImage.gameObject.SetActive(true);

            float maxScreenSize = Mathf.Max(Screen.width, Screen.height);
            _fadeImage.rectTransform.sizeDelta = Vector2.one * maxScreenSize * FADE_SCREEN_UPSCALE_MULTIPLIER;
            SetFadeAplha(0f);

            _fadeTween?.Kill();
            _fadeTween = DOTween.Sequence()
                .Append(_fadeImage.rectTransform.DOSizeDelta(Vector2.one * viewportSize, FADE_DURATION).SetEase(Ease.OutFlash))
                .Join(_fadeImage.DOFade(_fadeAlpha, FADE_DURATION).SetEase(Ease.OutFlash))
                .OnUpdate(() => SetFadeAplha(_fadeImage.color.a));
        }


        public void HidePointer(Action onCompleted = null)
        {
            _pointerTween?.Kill();
            _pointerObject.gameObject.SetActive(false);

            if (_fadeImage.gameObject.activeSelf)
                AnimateUnfade(onCompleted);

            else onCompleted?.Invoke();
        }
        
        private void AnimateUnfade(Action onCompleted = null)
        {
            float maxScreenSize = Mathf.Max(Screen.width, Screen.height);

            _fadeTween?.Kill();
            _fadeTween = DOTween.Sequence()
                .Append(_fadeImage.rectTransform.DOSizeDelta(Vector2.one * maxScreenSize * FADE_SCREEN_UPSCALE_MULTIPLIER, FADE_DURATION).SetEase(Ease.OutFlash))
                .Join(_fadeImage.DOFade(0f, FADE_DURATION).SetEase(Ease.OutFlash))
                .OnUpdate(() => SetFadeAplha(_fadeImage.color.a));

            _fadeTween.OnComplete(() =>
            {
                _fadeImage.gameObject.SetActive(false);
                onCompleted?.Invoke();
            });
        }

        private void SetFadeAplha(float alpha)
        {
            _fadeImage.color = _fadeImage.color.SetAlpha(alpha);
            _fadeFillImages.ForEach((image) => image.color = _fadeImage.color);
        }


        public Vector2 WorldToCanvas(Transform transform, Canvas canvas)
        {
            bool isOverlay = canvas.renderMode == RenderMode.ScreenSpaceOverlay;

            Vector3 screenPosition = _mainCamera.WorldToScreenPoint(transform.position);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rect: canvas.transform as RectTransform,
                screenPoint: screenPosition,
                cam: isOverlay ? null : _mainCamera,
                out Vector2 canvasPosition
            );

            return canvasPosition;
        }



    }
}


