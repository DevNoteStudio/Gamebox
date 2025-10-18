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
        private static Tween _windowFadeTween;
        private static int _windowFadePoints = 0;

        public static RectTransform Container { get; private set; }
        public static RectTransform FadeContainer { get; private set; }


        private static readonly Viewer<Image> screenFadeViewer = new(IConfigs.Gamebox.ScreenFadePrefab);
        private static readonly Viewer<Image> windowFadeViewer = new(IConfigs.Gamebox.WindowFadePrefab);

        public readonly static Vector2 TARGET_RESOLUTION = new Vector2(1080, 1920);
        private const float SCREEN_FADE_DURATION = 0.2f;
        private const float WINDOW_FADE_DURATION = 0.3f;


        public UI(RectTransform mainContainer, RectTransform fadeContainer)
        {
            Container = mainContainer;
            FadeContainer = fadeContainer;
        }

        public static void AddWindowFadePoint()
        {
            if (_windowFadePoints <= 0)
            {
                _windowFadeTween?.Kill();
                var image = windowFadeViewer.ShowExpand(Container);
                _windowFadeTween = TweenHub.Fade(image, WINDOW_FADE_DURATION);

                _windowFadePoints = 1;
            }
            else _windowFadePoints++;
        }

        public static void RemoveWindowFadePoint(bool forceHide = false)
        {
            if (_windowFadePoints == 1)
            {
                _windowFadeTween?.Kill();

                if (forceHide) windowFadeViewer.Hide();
                else
                {
                    _windowFadeTween = TweenHub.Unfade(windowFadeViewer.View, WINDOW_FADE_DURATION)
                        .OnComplete(() => windowFadeViewer.Hide());
                }

                _windowFadePoints = 0;
            }
            else _windowFadePoints--;
        }


        public static void ScreenFade(Action onCompleted = null)
        {
            var fadeImage = screenFadeViewer.ShowExpand(FadeContainer);

            _screenFadeTween?.Kill();
            _screenFadeTween = TweenHub.Fade(fadeImage, SCREEN_FADE_DURATION);
            _screenFadeTween.onComplete += () =>
            {
                onCompleted?.Invoke();

                _screenFadeTween = TweenHub.Unfade(screenFadeViewer.View, SCREEN_FADE_DURATION)
                    .OnComplete(screenFadeViewer.Hide);

                _screenFadeTween.SetUpdate(true);
            };

            _screenFadeTween.SetUpdate(true);
        }



    }
}

