using System.Drawing;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace DevNote.Gamebox
{
    public static class TweenHub
    {

        private const float POP_SHOW_DURATION = 0.7f;
        public static Tween PopShow(Transform transform, float duration = POP_SHOW_DURATION)
        {
            transform.localScale = Vector3.zero;
            return transform.DOScale(1f, duration).SetEase(Ease.OutBack);
        }

        private const float HIDE_DURATION = 0.3f;
        public static Tween Hide(Transform transform, float duration = HIDE_DURATION) 
            => transform.DOScale(0f, duration).SetEase(Ease.OutFlash);


        private const float POP_DURATION = 0.5f;

        private const float POP_DOWN_TO_SCALE = 0.8f;
        public static Tween PopDown(Transform transform) => DOTween.Sequence()
            .Append(transform.DOScale(POP_DOWN_TO_SCALE, POP_DURATION / 2f))
            .Append(transform.DOScale(1f, POP_DURATION / 2f).SetEase(Ease.OutBack));

        private const float POP_UP_TO_SCALE = 1.25f;
        public static Tween PopUp(Transform transform) => DOTween.Sequence()
            .Append(transform.DOScale(POP_UP_TO_SCALE, POP_DURATION / 2f).SetEase(Ease.OutFlash))
            .Append(transform.DOScale(1f, POP_DURATION / 2f).SetEase(Ease.OutBack));






        private const float FADE_DURATION = 0.5f;
        public static Tween Fade(Image image, float duration = FADE_DURATION)
        {
            image.color = image.color.SetAlpha(0f);
            return image.DOFade(1f, duration).SetEase(Ease.OutFlash);
        }
        public static Tween Unfade(Image image, float duration = FADE_DURATION)
        {
            image.color = image.color.SetAlpha(1f);
            return image.DOFade(0f, duration).SetEase(Ease.OutFlash);
        }



        private const float SHOW_FROM_FADE_ROTATE_ROTATION_Z = 30f;
        private const float SHOW_FROM_FADE_ROTATE_DURATION = 0.6f;
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


