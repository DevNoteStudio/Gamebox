using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DevNote.Gamebox
{
    public class UI
    {
        private static RectTransform _screenContainer;
        private static RectTransform _windowContainer;
        private static Image _fadeImage;

        public static RectTransform ScreenContainer => WindowFadeEnabled ? _windowContainer : _screenContainer;
        public static RectTransform WindowContainer => _windowContainer;

        private static bool WindowFadeEnabled => _fadeImage.gameObject.activeSelf;

        private const float FADE_BLENDING = 0.4f;
        private const float FADE_BLUR_STRENGTH = 25f;
        private const float FADE_DURATION = 0.3f;
        private const float UNFADE_DURATION = 0.3f;


        public UI(RectTransform mainContainer, RectTransform windowContainer, Image fadeImage)
        {
            _screenContainer = mainContainer;
            _windowContainer = windowContainer;
            _fadeImage = fadeImage;
            _fadeImage.gameObject.SetActive(false);
        }


        public static void ShowFade()
        {
            _fadeImage.gameObject.SetActive(true);
            _fadeImage.color = _fadeImage.color.GetColorAlpha(0f);
            _fadeImage.DOFade(1f, FADE_DURATION).SetEase(Ease.OutQuad);
        }

        public static void HideFade()
        {
            _fadeImage.color = _fadeImage.color.GetColorAlpha(1f);
            _fadeImage.DOFade(0f, UNFADE_DURATION).SetEase(Ease.InQuad).OnComplete(() =>
            {
                _fadeImage.gameObject.SetActive(false);
            });
        }


    }
}

