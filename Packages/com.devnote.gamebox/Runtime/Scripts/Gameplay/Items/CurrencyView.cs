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

        private Pool<ParticleImage> _coinsParticlePool;
        private int _previousCoins;
        private Sequence _coinsTextSequence;

        private readonly Holder<LevelController> levelController = new();


        private void Awake()
        {
            _coinsParticlePool = new(_coinsParticlePrefab, transform);
        }

        private void Start() => Display();


        public void Display()
        {
            int coins = IGameState.Items.Get(ItemKey.Coins);
            _previousCoins = coins;
            _coinsText.text = $"<sprite=0>{coins}";
        }

        public void AnimateDisplayWithSpending()
        {
            int coins = IGameState.Items.Get(ItemKey.Coins);
            _coinsTextSequence = AnimateAmountText(_previousCoins, coins, _coinsText, _spendCurrencyColor);
            _previousCoins = coins;
        }

        public void AnimateDisplayWithRollup(int particles, Vector2 fromCanvasPosition)
        {
            int coins = IGameState.Items.Get(ItemKey.Coins);
            var coinsParticle = _coinsParticlePool.Get(container: _particleContainer);

            (coinsParticle.transform as RectTransform).anchoredPosition = fromCanvasPosition;

            coinsParticle.RemoveBurst(0);
            coinsParticle.AddBurst(0, particles);

            coinsParticle.Play();
            IConfigs.AudioHub.CoinsRollupStart?.Play();

            coinsParticle.OnFirstParticleFinished += OnFirstParticleFinished;
            coinsParticle.OnLastParticleFinished += OnLastParticleFinished;

            void OnLastParticleFinished()
            {
                coinsParticle.Stop();
                _coinsParticlePool.Return(coinsParticle);

                coinsParticle.OnLastParticleFinished -= OnLastParticleFinished;
            }

            void OnFirstParticleFinished()
            {
                IConfigs.AudioHub.CoinsRollupFinish?.Play();
                AnimateAmountText(_previousCoins, coins, _coinsText, _earnCurrencyColor);
                _previousCoins = coins;
                
                coinsParticle.OnFirstParticleFinished -= OnFirstParticleFinished;
            }
        }


        private Sequence AnimateAmountText(int from, int to, TextMeshProUGUI text, Color color)
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


    }
}
