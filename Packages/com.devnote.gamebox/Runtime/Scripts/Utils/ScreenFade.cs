using System;
using DevNote;
using DG.Tweening;
using UnityEngine.UI;

namespace Gamebox
{
    public static class ScreenFade
    {
        private static Tween _currentTween;

        private static readonly Viewer<Image> fadeViewer = new(IConfigs.Gamebox.ScreenFadePrefab);

        private const float FADE_DURATION = 0.2f;

        public static void Fade(Action onCompleted = null)
        {
            var fadeImage = fadeViewer.Show(UI.FadeContainer);

            _currentTween?.Kill();
            _currentTween = TweenHub.Fade(fadeImage, FADE_DURATION);
            _currentTween.onComplete += () =>
            {
                onCompleted?.Invoke();
                Unfade();
            };

            _currentTween.SetUpdate(true);
        }

        private static void Unfade()
        {
            if (!fadeViewer.ViewExists) return;

            _currentTween?.Kill();
            _currentTween = TweenHub.Unfade(fadeViewer.View, FADE_DURATION).OnComplete(fadeViewer.Hide);
            _currentTween.SetUpdate(true);
        }

    }
}

