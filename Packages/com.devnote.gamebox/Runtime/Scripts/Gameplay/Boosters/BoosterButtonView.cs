using System;
using Coffee.UIExtensions;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class BoosterButtonView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _progressFillImage;
        [SerializeField] private TextMeshProUGUI _counterText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private GameObject _lockObject;
        [SerializeField] private GameObject _priceObject;
        [SerializeField] private GameObject _counterObject;
        [SerializeField] private Button _useButton;
        [SerializeField] private UIParticle _spendParticle;
        [SerializeField] private RectTransform _animatedRect;

        private ItemKey _boosterItemKey;

        private readonly Holder<BoosterController> boosterController = new();


        private void OnEnable()
        {
            boosterController.Item.OnBoosterUsingFinished += OnBoosterUsingFinished;
        }

        private void OnBoosterUsingFinished(bool success)
        {
            if (success && _boosterItemKey == boosterController.Item.CurrentUsingBoosterKey)
                AnimateBoosterApply();
                
        }


        private void Start()
        {
            _useButton.onClick.AddListener(OnUseButtonClick);
        }

        public void Display(ItemKey boosterItemKey, int boosterNumber)
        {
            _boosterItemKey = boosterItemKey;

            _iconImage.LoadSprite(AssetLoader.LoadItemSprite(boosterItemKey));
            
            var contentType = boosterNumber switch
            {
                1 => ContentKey.UnlockBooster1,
                2 => ContentKey.UnlockBooster2,
                3 => ContentKey.UnlockBooster3,
                4 => ContentKey.UnlockBooster4,
                _ => throw new Exception($"Wrong booster number: {boosterNumber}. It must be from 1 to 4.")
            };

            bool isUnlocked = IConfigs.Gamebox.ContentPipeline.IsUnlocked(contentType);

            _lockObject.SetActive(!isUnlocked);
            _counterObject.SetActive(isUnlocked);
            _priceObject.SetActive(isUnlocked);
            _useButton.image.raycastTarget = isUnlocked;

            if (isUnlocked)
            {
                bool hasBooster = IGameState.Items.Has(boosterItemKey);
                
                _priceObject.SetActive(!hasBooster);

                _counterObject.SetActive(hasBooster);
                if (hasBooster) _counterText.text =IGameState.Items.Get(boosterItemKey).ToString();

            }
            
        }

        public void AnimateProgress()
        {

        }


        private void OnUseButtonClick()
        {
            boosterController.Item.StartBoosterUsing(_boosterItemKey);
        }


        private void AnimateBoosterApply()
        {
            const float DURATION = 0.4f;
            const float TO_SCALE = 1.4f;


            _spendParticle.Play();
            Sound.Play(SoundName.BoosterApplied);
            _useButton.image.raycastTarget = false;

            DOTween.Sequence()
                .Append(_animatedRect.DOScale(TO_SCALE, DURATION / 2f).SetEase(Ease.OutQuad))
                .Append(_animatedRect.DOScale(1f, DURATION / 2f).SetEase(Ease.InQuad))
                .OnComplete(() => _useButton.image.raycastTarget = true);
        }
        



    }
}
