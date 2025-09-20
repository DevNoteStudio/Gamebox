using DevNote;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gamebox
{
    [RequireComponent(typeof(Button))]
    public class PulcedBubbleButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IAnimation
    {
        private enum ButtonSoundType { Click, Open }

        [SerializeField] private ButtonSoundType _soundType;



        private Button _button;
        private Tween _currentTween;

        private readonly Vector2 MIN_MAX_PULCE_SCALE = new Vector2(0.85f, 1f);
        private readonly float MAX_POINTER_ENTER_SCALE = 1.05f;
        private const float BUBBLE_DURATION = 0.2f;
        private readonly float PULCE_DURATION = 1.2f;


        private void Awake() => _button = GetComponent<Button>();

        private void Start() => _button.onClick.AddListener(OnButtonClick);

        private void OnEnable() => AnimatePulce();

        private void OnDisable() => _currentTween?.Kill();



        private void AnimatePulce()
        {
            transform.localScale = MIN_MAX_PULCE_SCALE.x * Vector3.one;

            _currentTween.Kill();
            _currentTween = DOTween.Sequence()
                .Append(transform.DOScale(MIN_MAX_PULCE_SCALE.y, PULCE_DURATION / 2f).SetEase(Ease.InOutFlash))
                .Append(transform.DOScale(MIN_MAX_PULCE_SCALE.x, PULCE_DURATION / 2f).SetEase(Ease.InOutFlash))
                .SetLoops(-1, LoopType.Restart);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!_button.interactable) return;

            Configs.Gamebox.PointerEnterSound.Play();

            _currentTween?.Kill();
            _currentTween = transform.DOScale(MAX_POINTER_ENTER_SCALE, BUBBLE_DURATION).SetEase(Ease.OutFlash);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (!_button.interactable) return;

            _currentTween?.Kill();
            _currentTween = transform.DOScale(MIN_MAX_PULCE_SCALE.x, PULCE_DURATION / 2f).SetEase(Ease.OutFlash)
                .OnComplete(() => AnimatePulce());
        }


        private void OnButtonClick()
        {
            var sound = _soundType == ButtonSoundType.Click ?
                Configs.Gamebox.ClickSound : Configs.Gamebox.OpenClickSound;

            sound.Play();
        }

        void IAnimation.Play() => AnimatePulce();

        void IAnimation.Stop() => _currentTween?.Kill();

    }

}

