using System;
using Coffee.UIExtensions;
using Cysharp.Threading.Tasks;
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
        [SerializeField] private UIParticle _shineParticles;
        [SerializeField] private UIParticle _buyParticle;

        private ItemKey _boosterItemKey;
        private int _boosterNumber;

        private readonly Holder<BoosterController> boosterController = new();
        private readonly Holder<CurrencyController> currencyController = new();
        private readonly Holder<IAds> ads = new();

        private const float SHINE_SHOW_DURATION = 0.4f;

        private void Awake()
        {
            _shineParticles.Stop();
        }


        private void OnEnable()
        {
            boosterController.Item.OnBoosterUsingFinished += OnBoosterUsingFinished;
            boosterController.Item.OnBoosterUsingStarted += OnBoosterUsingStarted;
        }

        private void OnDisable()
        {
            boosterController.Item.OnBoosterUsingFinished -= OnBoosterUsingFinished;
            boosterController.Item.OnBoosterUsingStarted -= OnBoosterUsingStarted;
        }

        private void OnBoosterUsingStarted()
        {
            if (_boosterItemKey == boosterController.Item.CurrentUsingBoosterKey)
                AnimateBoosterStartUsing();
        }

        private void OnBoosterUsingFinished(bool success)
        {
            if (_boosterItemKey != boosterController.Item.CurrentUsingBoosterKey)
                return;

            if (success) AnimateBoosterApply();
            else AnimateBoosterCancelUsing();
        }


        private void Start()
        {
            _useButton.onClick.AddListener(OnUseButtonClick);
        }

        public void Display(ItemKey boosterItemKey, int boosterNumber)
        {
            _boosterItemKey = boosterItemKey;
            _boosterNumber = boosterNumber;

            _iconImage.LoadSprite(AssetLoader.LoadItemSprite(boosterItemKey));
            
            var contentType = boosterNumber switch
            {
                1 => ContentKey.UnlockBooster1,
                2 => ContentKey.UnlockBooster2,
                3 => ContentKey.UnlockBooster3,
                4 => ContentKey.UnlockBooster4,
                _ => throw new Exception($"Wrong booster number: {boosterNumber}. It must be from 1 to 4.")
            };

            bool isUnlocked = IConfigs.Gamebox.ContentPipeline.IsAvailable(contentType);

            _lockObject.SetActive(!isUnlocked);
            _counterObject.SetActive(isUnlocked);
            _priceObject.SetActive(isUnlocked);
            _useButton.image.raycastTarget = isUnlocked;

            if (isUnlocked)
            {
                bool hasBooster = IGameState.Items.Has(boosterItemKey);            

                _priceObject.SetActive(!hasBooster);
                if (!hasBooster) _priceText.text = $"<sprite=0>{IConfigs.Gamebox.GetBoosterPrice(_boosterItemKey)}";

                _counterObject.SetActive(hasBooster);
                if (hasBooster) _counterText.text = IGameState.Items.Get(boosterItemKey).ToString();

            }
            
        }

        public void AnimateProgress()
        {

        }


        private void OnUseButtonClick()
        {
            int boosters = IGameState.Items.Get(_boosterItemKey);

            // <-- Using -->
            if (boosters > 0)
            {
                if (boosterController.Item.IsUsingBooster)
                {
                    if (boosterController.Item.CurrentUsingBoosterKey == _boosterItemKey)
                        boosterController.Item.CancelBoosterUsing();

                    else
                    {
                        boosterController.Item.CancelBoosterUsing();
                        boosterController.Item.StartBoosterUsing(_boosterItemKey);
                    }
                }
                else boosterController.Item.StartBoosterUsing(_boosterItemKey);
            }

            // <-- No boosters -->
            else
            {
                int price = IConfigs.Gamebox.GetBoosterPrice(_boosterItemKey);

                // <-- Purchasing -->
                if (currencyController.Item.TrySpendCoins(price))
                {
                    Sound.Play(SoundName.BuyBooster);
                    _buyParticle.Play();
                    IGameState.Items.Add(_boosterItemKey, 1);
                }

                // <-- Show rewarded ads -->
                else
                {

                }
            }
        }


        private async void AnimateBoosterStartUsing()
        {
            await UniTask.NextFrame();

            if (boosterController.Item.IsUsingBooster)
            {
                if (_shineParticles.isPaused) _shineParticles.Play();
                _shineParticles.StartEmission();

                Sound.Play(SoundName.StartUsingBooster);
            }
        }

        private void AnimateBoosterCancelUsing()
        {
            _shineParticles.StopEmission();
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

            _shineParticles.StopEmission();
        }




    }
}
