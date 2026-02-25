using System.Collections.Generic;
using AssetKits.ParticleImage;
using Coffee.UIExtensions;
using Cysharp.Threading.Tasks;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private UIParticle _scoreFlashParticle;
        [SerializeField] private ParticleImage _particleScorePrefab;
        [SerializeField] private RectTransform _particleContainer;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Slider _scoreSlider;
        [SerializeField] private Image _shineImage;

        private float _previousProgress;

        private bool _flashParticleIsPlaying = false;
        private Tween _textTween;
        private Tween _sliderTween;
        private Tween _shineTween;
        private Pool<ParticleImage> _particleScorePool;
        private Dictionary<ParticleImage, int> _particleScores = new();

        private readonly Holder<ScoreController> scoreController = new();
        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<SoundController> soundController = new();

        private const float FLASH_PARTICLE_DURATION = 0.3f;
        private const float PROGRESS_DURATION = 0.5f;
        private const float PROGRESS_EFFECT_DURATION = 0.25f;
        private const float TEXT_UPSCALE = 1.5f;


        private void Awake()
        {
            _particleScorePool = new(_particleScorePrefab, transform);
            _shineImage.color = _shineImage.color.SetAlpha(0f);
        }

        private void OnEnable()
        {
            scoreController.Item.OnScoreChanged += OnScoreChanged;
            levelController.Item.OnLevelStarted += OnLevelStarted;
            OnScoreChanged();
            OnLevelStarted();
        }

        private void OnDisable()
        {
            scoreController.Item.OnScoreChanged -= OnScoreChanged;
            levelController.Item.OnLevelStarted -= OnLevelStarted;
        }

        

        public void AnimateParticleScore(int score, int particles, Vector3 fromWorldPosition)
        {
            var scoreParticle = _particleScorePool.Get(container: _particleContainer);

            (scoreParticle.transform as RectTransform).anchoredPosition
                = Utils.WorldToCanvas(fromWorldPosition, UI.Canvas, GameboxSceneContext.MainCamera);

            scoreParticle.RemoveBurst(0);
            scoreParticle.AddBurst(0, particles);

            scoreParticle.Play();

            _particleScores[scoreParticle] = score;

            scoreParticle.OnFirstParticleFinished += OnFirstParticleFinished;
            scoreParticle.OnLastParticleFinished += OnLastParticleFinished;
        }

        private void OnLastParticleFinished(ParticleImage particleImage)
        {
            particleImage.Stop();
            _particleScorePool.Return(particleImage);
            particleImage.OnLastParticleFinished -= OnLastParticleFinished;
        }

        private void OnLevelStarted()
        {
            _levelText.text = (levelController.Item.CurrentLevelIndex + 1).ToString();
        }


        private void OnFirstParticleFinished(ParticleImage particleImage)
        {
            var particleScore = _particleScores[particleImage];
            scoreController.Item.AddScore(particleScore);
            _particleScores.Remove(particleImage);

            particleImage.OnFirstParticleFinished -= OnFirstParticleFinished;
        }

        private void OnScoreChanged()
        {
            int currentScore = scoreController.Item.CurrentScore;
            int requiredScore = scoreController.Item.RequiredScore;
            float progress = (float)currentScore / requiredScore;

            bool isScoreIncrease = progress > _previousProgress && currentScore != 0;

            if (isScoreIncrease)
            {
                soundController.Item.PlayScoreFill(_previousProgress, progress);

                if (!_flashParticleIsPlaying)
                {
                    _scoreFlashParticle.Play();
                    _flashParticleIsPlaying = true;
                }

                _scoreFlashParticle.StartEmission();

                _shineTween?.Kill();
                _shineTween = _shineImage.DOFade(1f, PROGRESS_EFFECT_DURATION);
            }
                
            _textTween?.Kill();
            _textTween = _scoreText.transform.DOScale(TEXT_UPSCALE, PROGRESS_EFFECT_DURATION);

            _sliderTween?.Kill();
            _sliderTween = _scoreSlider.DOValue(progress, PROGRESS_DURATION)
            .OnUpdate(() =>
            {
                int score = Mathf.RoundToInt(_scoreSlider.value * requiredScore);
                _scoreText.text = $"{score}/<size=85%>{requiredScore}";
            })
            .OnComplete(() =>
            {
                _scoreFlashParticle.StopEmission();
                _scoreText.text = $"{currentScore}/<size=85%>{requiredScore}";

                _shineTween = _shineImage.DOFade(0f, PROGRESS_EFFECT_DURATION);
                _textTween = _scoreText.transform.DOScale(1f, PROGRESS_EFFECT_DURATION);
            });

            _previousProgress = progress;
        }



    }
}
