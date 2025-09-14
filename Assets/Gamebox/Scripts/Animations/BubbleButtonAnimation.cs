using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DevNote.Gamebox
{
    [RequireComponent(typeof(Button))]
    public class BubbleButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Button _button;
        private Tween _pointerTween;
        private Tween _clickTween;

        private const float TO_SCALE = 1.07f;
        private const float DURATION = 0.2f;


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
            if (_clickTween.IsActive() && _clickTween.IsPlaying() || !_button.interactable) return;

            _pointerTween?.Kill();
            _pointerTween = transform.DOScale(TO_SCALE, DURATION).SetEase(Ease.OutFlash);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (_clickTween.IsActive() && _clickTween.IsPlaying() || !_button.interactable) return;

            _pointerTween?.Kill();
            _pointerTween = transform.DOScale(1f, DURATION).SetEase(Ease.OutFlash);
        }


        private void OnButtonClick()
        {
            if (_clickTween.IsActive() && _clickTween.IsPlaying()) return;

            _clickTween?.Kill();
            _clickTween = TweenHub.PopDown(transform);
        }




    }
}


