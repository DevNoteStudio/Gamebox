using DevNote;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace Gamebox
{
    public static class TweenHub
    {
        public static Tween Show(Transform transform, float duration = 0.45f, bool playSound = false)
        {
            transform.localScale = Vector3.zero;
            return transform.DOScale(1f, duration).SetEase(Ease.OutBack).OnStart(() => 
            {
                if (playSound) IConfigs.Gamebox.ShowSound.Play();
            });
        }

        public static Tween DelayedShow(Transform transform, float delay = 2f, float duration = 0.7f)
        {
            transform.localScale = Vector3.zero;

            return DOTween.Sequence().Attach(transform.gameObject)
                .AppendInterval(delay)
                .Append(transform.DOScale(1f, duration).SetEase(Ease.OutBack));
        }


        public static Tween Hide(Transform transform, float duration = 0.3f, bool playSound = false)
        {
            return transform.DOScale(0f, duration).SetEase(Ease.OutFlash).OnStart(() =>
            {
                if (playSound) IConfigs.Gamebox.HideSound.Play();
            });
        }




        private const float POP_DOWN_TO_SCALE = 0.8f;
        public static Tween PopDown(Transform transform, float duration = 0.5f) => DOTween.Sequence()
            .Append(transform.DOScale(POP_DOWN_TO_SCALE, duration / 2f))
            .Append(transform.DOScale(1f, duration / 2f).SetEase(Ease.OutBack));

        private const float POP_UP_TO_SCALE = 1.25f;
        public static Tween PopUp(Transform transform, float duration = 0.5f) => DOTween.Sequence()
            .Append(transform.DOScale(POP_UP_TO_SCALE, duration / 2f).SetEase(Ease.OutFlash))
            .Append(transform.DOScale(1f, duration / 2f).SetEase(Ease.OutBack));


        public static Tween Fade(Image image, float duration = 0.5f)
        {
            image.color = image.color.SetAlpha(0f);
            return image.DOFade(1f, duration).SetEase(Ease.OutFlash);
        }
        public static Tween Unfade(Image image, float duration = 0.5f)
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


