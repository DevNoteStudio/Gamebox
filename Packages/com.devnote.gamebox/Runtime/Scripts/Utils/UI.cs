using DevNote;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class UI
    {
        private static Tween _screenFadeTween;
        private static Tween _fadeTween;
        private static int _fadePoints = 0;

        public static RectTransform Container { get; private set; }
        public static RectTransform FadeContainer { get; private set; }


        private static readonly Viewer<Image> screenFadeViewer = new(IConfigs.Gamebox.ScreenFadePrefab);
        private static readonly Viewer<Image> windowFadeViewer = new(IConfigs.Gamebox.WindowFadePrefab);

        public readonly static Vector2 TARGET_RESOLUTION = new Vector2(1080, 1920);
        private const float SCREEN_FADE_DURATION = 0.2f;
        private const float SCREEN_FADE_DELAY = 0.2f;
        private const float WINDOW_FADE_DURATION = 0.3f;


        public UI(RectTransform mainContainer, RectTransform fadeContainer)
        {
            Container = mainContainer;
            FadeContainer = fadeContainer;
        }

        public static void AddFadePoint()
        {
            if (_fadePoints <= 0)
            {
                _fadeTween?.Kill();
                var image = windowFadeViewer.ShowExpand(Container);
                _fadeTween = TweenHub.Fade(image, WINDOW_FADE_DURATION);

                _fadePoints = 1;
            }
            else _fadePoints++;
        }

        public static void RemoveFadePoint(bool forceHide = false)
        {
            if (_fadePoints == 1)
            {
                _fadeTween?.Kill();

                if (forceHide) windowFadeViewer.Hide();
                else
                {
                    _fadeTween = TweenHub.Unfade(windowFadeViewer.View, WINDOW_FADE_DURATION)
                        .OnComplete(() => windowFadeViewer.Hide());
                }

                _fadePoints = 0;
            }
            else _fadePoints--;
        }


        public static void ScreenFade(Action onCompleted = null)
        {
            var fadeImage = screenFadeViewer.ShowExpand(FadeContainer);

            _screenFadeTween?.Kill();
            _screenFadeTween = TweenHub.Fade(fadeImage, SCREEN_FADE_DURATION);
            _screenFadeTween.onComplete += () =>
            {
                onCompleted?.Invoke();

                _screenFadeTween = DOTween.Sequence().AppendInterval(SCREEN_FADE_DELAY)
                    .Append(TweenHub.Unfade(screenFadeViewer.View, SCREEN_FADE_DURATION))
                    .OnComplete(screenFadeViewer.Hide);

                _screenFadeTween.SetUpdate(true);
            };

            _screenFadeTween.SetUpdate(true);
        }



    }
}

