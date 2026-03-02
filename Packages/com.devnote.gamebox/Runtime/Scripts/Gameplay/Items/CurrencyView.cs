using System.Collections.Generic;
using AssetKits.ParticleImage;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Gamebox
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private ParticleImage _coinsParticlePrefab;
        [SerializeField] private RectTransform _particleContainer;
        [SerializeField] private RectTransform _scalableRect;
        [SerializeField] private Color _earnCurrencyColor;
        [SerializeField] private Color _spendCurrencyColor;

        private Dictionary<ParticleImage, int> _particleCoins = new();
        private Pool<ParticleImage> _coinsParticlePool;
        private int _previousCoins;
        private Sequence _coinsTextSequence;

        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<CurrencyController> currencyController = new();


        private void Awake()
        {
            _coinsParticlePool = new(_coinsParticlePrefab, transform);
        }


        private void OnEnable()
        {
            levelController.Item.OnLevelStarted += Display;
            currencyController.Item.OnCoinsEarned += OnCoinsEarned;
            currencyController.Item.OnCoinsSpent += OnCoinsSpent;
            Display();
        }

        private void OnDisable()
        {
            currencyController.Item.OnCoinsSpent -= OnCoinsSpent;
            currencyController.Item.OnCoinsEarned -= OnCoinsEarned;
            levelController.Item.OnLevelStarted -= Display;
        }

        private void Display()
        {
            int coins = currencyController.Item.DisplayedCoins;
            _previousCoins = coins;
            _coinsText.text = $"<sprite=0>{coins}";
        }


        private void OnCoinsEarned()
        {
            int newValue = currencyController.Item.DisplayedCoins;
            _coinsTextSequence = AnimateCurrencyText(_previousCoins, newValue, _coinsText, _earnCurrencyColor);
            _previousCoins = newValue;
        }

        private void OnCoinsSpent()
        {
            int newValue = currencyController.Item.DisplayedCoins;
            _coinsTextSequence = AnimateCurrencyText(_previousCoins, newValue, _coinsText, _spendCurrencyColor);
            _previousCoins = newValue;
        }

        private Sequence AnimateCurrencyText(int from, int to, TextMeshProUGUI text, Color color)
        {
            const float DURATION = 0.3f;
            const float SCALE_UP = 1.4f;

            

            int currentValue = from;
            int endValue = to;

            var sequence = DOTween.Sequence();

            text.color = color;

            // <-- Text animation -->
            sequence.Join(
                DOTween.To(
                    () => currentValue,
                    x =>
                    {
                        currentValue = x;
                        text.text = $"<sprite=0>{currentValue}";
                        _previousCoins = currentValue;
                    },
                    endValue,
                    DURATION
                ).SetEase(Ease.Linear)
            );

            // <-- Scale up -->
            sequence.Join(
                _scalableRect
                    .DOScale(SCALE_UP, DURATION * 0.5f)
                    .SetEase(Ease.OutBack)
            );

            // <-- Scale down -->
            sequence.Append(
                _scalableRect
                    .DOScale(1f, DURATION * 0.5f)
                    .SetEase(Ease.InBack)
            );

            sequence.OnComplete(() => text.color = Color.white);

            return sequence;
        }





        public void AnimateCoinsRollup(int coins, int particles, Vector2 fromCanvasPosition)
        {
            var coinsParticle = _coinsParticlePool.Get(container: _particleContainer);

            (coinsParticle.transform as RectTransform).anchoredPosition = fromCanvasPosition;

            coinsParticle.RemoveBurst(0);
            coinsParticle.AddBurst(0, particles);

            coinsParticle.Play();
            Sound.Play(SoundName.ShowCoinsParticles);

            _particleCoins[coinsParticle] = coins;

            coinsParticle.OnFirstParticleFinished += OnFirstParticleFinished;
            coinsParticle.OnLastParticleFinished += OnLastParticleFinished;
        }

        private void OnLastParticleFinished(ParticleImage particleImage)
        {
            particleImage.Stop();
            _coinsParticlePool.Return(particleImage);
            particleImage.OnLastParticleFinished -= OnLastParticleFinished;
        }

        private void OnFirstParticleFinished(ParticleImage particleImage)
        {
            Sound.Play(SoundName.CoinsParticlesApplyed);

            var particleCoins = _particleCoins[particleImage];
            currencyController.Item.EarnDisplayedCoins(particleCoins);
            _particleCoins.Remove(particleImage);

            particleImage.OnFirstParticleFinished -= OnFirstParticleFinished;
        }


    }
}
