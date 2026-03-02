using System.Collections.Generic;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Gamebox
{


    public class BoosterPanelView : MonoBehaviour
    {
        [SerializeField] private RectTransform _hintRect;
        [SerializeField] private TextMeshProUGUI _hintText;
        [SerializeField] private BoosterButtonView _boosterButtonPrefab;
        [SerializeField] private RectTransform _portraitContainer;
        [SerializeField] private RectTransform _landscapeContainer;


        private Tween _hintTween;
        private List<BoosterButtonView> _boosterButtons;

        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<BoosterController> boosterController = new();

        private const float SHOW_HINT_DURATION = 0.3f;

        private void Awake()
        {
            _hintRect.localScale = Vector3.zero;
            _hintRect.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (_boosterButtons == null) CreateBoosterButtons();

            ScreenState.OnOrientationChanged += OnOrientationChanged;
            levelController.Item.OnLevelStarted += Display;

            foreach (var boosterItemKey in IConfigs.Gamebox.GetAllBoosterKeys())
                IGameState.Items.Subscribe(boosterItemKey, Display);

            OnOrientationChanged();
            Display();
        }

        private void OnDisable()
        {
            ScreenState.OnOrientationChanged -= OnOrientationChanged;
            levelController.Item.OnLevelStarted -= Display;

            foreach (var boosterItemKey in IConfigs.Gamebox.GetAllBoosterKeys())
                IGameState.Items.Dispose(boosterItemKey, Display);
        }

        private void OnOrientationChanged()
        {
            var container = ScreenState.Orientation == Orientation.Portrait ? 
                _portraitContainer : _landscapeContainer;

            foreach (var boosterButton in _boosterButtons)
            {
                boosterButton.transform.SetParent(container, worldPositionStays: false);
            }
                
        }

        public BoosterButtonView GetButton(ItemKey boosterItemKey)
            => _boosterButtons.FindOrException((button) => button.BoosterItemKey == boosterItemKey);


        private void Display()
        {
            var boosterItemKeys = IConfigs.Gamebox.GetAllBoosterKeys();

            for (int i = 0; i < boosterItemKeys.Count; i++)
                _boosterButtons[i].Display(boosterItemKeys[i]);
        }


        private void CreateBoosterButtons()
        {
            _boosterButtons = new();

            foreach (var itemKey in IConfigs.Gamebox.GetAllBoosterKeys())
            {
                var boosterButton = Instantiate(_boosterButtonPrefab, _portraitContainer);
                _boosterButtons.Add(boosterButton);
            }
                
        }


        public void ShowBoosterHint(ItemKey boosterItemKey)
        {
            const float SHINE_LOOP_DURATION = 1.3f;
            const float SHINE_TO_SCALE = 1.1f;

            _hintRect.gameObject.SetActive(true);

            _hintTween?.Kill();
            _hintTween = _hintRect.DOScale(SHINE_TO_SCALE, SHOW_HINT_DURATION).SetEase(Ease.OutFlash);
            _hintTween.OnComplete(() =>
            {
                _hintTween = DOTween.Sequence()
                    .Append(_hintRect.DOScale(1f, SHINE_LOOP_DURATION / 2f).SetEase(Ease.InOutFlash))
                    .Append(_hintRect.DOScale(SHINE_TO_SCALE, SHINE_LOOP_DURATION / 2f).SetEase(Ease.InOutFlash))
                    .SetLoops(-1);
            });

            _hintText.text = IConfigs.Gamebox.GetBoosterHint(boosterItemKey);


        }

        public void HideBoosterHint()
        {
            if (!_hintRect.gameObject.activeSelf) return;

            _hintTween?.Kill();
            _hintTween = _hintRect.DOScale(0f, SHOW_HINT_DURATION).SetEase(Ease.InFlash);
            _hintTween.OnComplete(() => _hintRect.gameObject.SetActive(false));
        }


    }
}
