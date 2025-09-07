using System.Drawing;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace DevNote.Gamebox
{
    public static class TweenHub
    {

        private const float POP_SHOW_DURATION = 0.7f;
        public static Tween PopShow(Transform transform)
        {
            transform.localScale = Vector3.zero;
            return transform.DOScale(1f, POP_SHOW_DURATION).SetEase(Ease.OutBack);
        }


        private const float FADE_DURATION = 0.5f;
        public static Tween Fade(Image image)
        {
            image.color = image.color.SetAlpha(0f);
            return image.DOFade(1f, FADE_DURATION).SetEase(Ease.OutFlash);
        }



        private const float SHOW_FROM_FADE_ROTATE_ROTATION_Z = 30f;
        private const float SHOW_FROM_FADE_ROTATE_DURATION = 0.5f;
        private const float SHOW_FROM_FADE_ROTATE_SCALE = 2f;
        public static Tween ShowFromFadeRotate(Image image)
        {
            var transform = image.transform;

            image.color = image.color.SetAlpha(0f);
            transform.localScale = Vector3.one * SHOW_FROM_FADE_ROTATE_SCALE;
            transform.localRotation = Quaternion.Euler(0f, 0f, -SHOW_FROM_FADE_ROTATE_ROTATION_Z);

            return DOTween.Sequence()
                .Append(image.DOFade(1f, SHOW_FROM_FADE_ROTATE_DURATION).SetEase(Ease.InQuad))
                .Join(transform.DOScale(1f, SHOW_FROM_FADE_ROTATE_DURATION).SetEase(Ease.InQuad))
                .Join(transform.DOLocalRotate(Vector3.zero, SHOW_FROM_FADE_ROTATE_DURATION).SetEase(Ease.Linear));

        }



    }
}


