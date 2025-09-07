using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace DevNote.Gamebox
{
    public class VictoryScreenView : MonoBehaviour
    {
        [SerializeField] private List<UIParticle> _confettiParticles;
        [SerializeField] private List<UIParticle> _starFlashParticles;
        [SerializeField] private List<UIParticle> _starShineParticles;
        [SerializeField] private List<Image> _starImages;
        [SerializeField] private RectTransform _titleRect;
        [SerializeField] private Image _fadeImage;
        [SerializeField] private RectTransform _starsRect;
        [SerializeField] private RectTransform _rewardsRect;
        [SerializeField] private RectTransform _bonusRect;
        [SerializeField] private RectTransform _bottomButtonRect;
        [SerializeField] private ItemWidgetView _rewardItemWidgetPrefab;
        [SerializeField] private RectTransform _rewardContainer;
        [SerializeField] private RouletteView _roulette;


        private Pool<ItemWidgetView> _rewardItemWidgetPool;
        private Tween _currentTween;
        private int _stars;
        private bool _showBonus;

        private const float TITLE_DURATION = 0.6f;
        private const float TITLE_OFFSET_Y = 200f;
        private const float DELAY_AFTER_STARS = 1f;
        private const float DELAY_AFTER_REWARDS = 0.5f;
        private const float DELAY_AFTER_ROULETTE = 2.5f;


        private void Awake()
        {
            _rewardItemWidgetPool = new(_rewardItemWidgetPrefab);
        }


        public void Display(int stars, List<(ItemType, int)> rewards, bool showBonus)
        {
            _stars = stars;
            _showBonus = showBonus;

            _rewardItemWidgetPool.Clear();
            foreach (var reward in rewards)
                _rewardItemWidgetPool.Get(_rewardContainer).Display(reward.Item1, reward.Item2);




        }


        public void AnimateShow()
        {
            _confettiParticles.ForEach(particle => particle.Play());

            _titleRect.localScale = new Vector3(0f, 1f, 1f);
            _titleRect.localPosition = new Vector2(0f, -UI.TARGET_RESOLUTION.y / 2f + TITLE_OFFSET_Y);
            _starFlashParticles.ForEach(particle => particle.Stop());
            _starShineParticles.ForEach(particle => particle.Stop());

            _currentTween?.Kill();
            var sequence = DOTween.Sequence().SetLink(gameObject, LinkBehaviour.KillOnDisable)
                .Append(TweenHub.Fade(_fadeImage))
                .Join(_titleRect.DOScaleX(1f, TITLE_DURATION).SetEase(Ease.OutBack))

                .Append(_titleRect.DOLocalMoveY(0f, TITLE_DURATION).SetEase(Ease.InOutFlash))
                .Append(TweenHub.PopShow(_starsRect));

            for (int i = 0; i < _starImages.Count; i++)
            {
                int index = i;
                sequence.Append(TweenHub.ShowFromFadeRotate(_starImages[i]));
                sequence.AppendCallback(() =>
                {
                    _starFlashParticles[index].Play();
                    _starShineParticles[index].Play();
                });
            }

            sequence.AppendInterval(DELAY_AFTER_STARS);
            sequence.Append(TweenHub.PopShow(_rewardsRect));
            sequence.AppendInterval(DELAY_AFTER_REWARDS);

            sequence.AppendCallback(() => _roulette.Start());

            sequence.Append(TweenHub.PopShow(_bonusRect));
            sequence.AppendInterval(DELAY_AFTER_ROULETTE);

            sequence.Append(TweenHub.PopShow(_bottomButtonRect));



        }




    }

}


