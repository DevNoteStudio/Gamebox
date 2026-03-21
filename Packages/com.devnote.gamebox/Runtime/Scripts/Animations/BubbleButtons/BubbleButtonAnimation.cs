using DevNote;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gamebox
{
    [RequireComponent(typeof(Button))]
    public class BubbleButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private enum ButtonSoundType { Click, Open, None }

        [SerializeField] private ButtonSoundType _soundType;

        private Button _button;
        private Tween _pointerTween;
        private Tween _clickTween;

        private const float TO_SCALE = 1.07f;
        private const float DURATION = 0.2f;


        private bool NewAnimationIsNotAvailable
        {
            get
            {
                bool clickTweenIsActive = _clickTween != null ? _clickTween.IsActive() && _clickTween.IsPlaying() : false;
                return clickTweenIsActive || !_button.interactable || !_button.image.raycastTarget;
            }
        }
            


        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _pointerTween?.Kill();
        }




        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (NewAnimationIsNotAvailable) return;

            IConfigs.AudioHub.PointerEnter?.Play();

            _pointerTween?.Kill();
            _pointerTween = transform.DOScale(TO_SCALE, DURATION).SetEase(Ease.OutFlash).SetUpdate(true);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (NewAnimationIsNotAvailable) return;

            _pointerTween?.Kill();
            _pointerTween = transform.DOScale(1f, DURATION).SetEase(Ease.OutFlash).SetUpdate(true);
        }





        private void OnButtonClick()
        {
            var soundUnit = _soundType == ButtonSoundType.Click ? IConfigs.AudioHub.Click : IConfigs.AudioHub.OpenClick;

            if (_soundType != ButtonSoundType.None)
                soundUnit?.Play();

            if (_clickTween.IsActive() && _clickTween.IsPlaying()) return;

            _clickTween?.Kill();
            _clickTween = TweenHub.PopDown(transform).SetUpdate(true);
        }

    }
}


