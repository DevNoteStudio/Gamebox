using System;
using System.Collections.Generic;
using System.Reflection;
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
        [SerializeField] private ItemWidgetView _rewardItemWidgetPrefab;
        [SerializeField] private RectTransform _rewardContainer;
        [SerializeField] private RouletteView _roulette;
        [SerializeField] private Button _skipButton;
        [SerializeField] private Button _takeButton;
        [SerializeField] private Button _stopRouletteButton;


        private Pool<ItemWidgetView> _rewardItemWidgetPool;
        private ItemWidgetView _coinItemWidget;
        private Tween _currentTween;
        private int _stars;
        private bool _showBonus;
        private int _originRewardCoins;

        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<IAds> ads = new();

        private const float TITLE_DURATION = 0.6f;
        private const float TITLE_OFFSET_Y = 200f;
        private const float DELAY_AFTER_STARS = 1f;
        private const float DELAY_AFTER_REWARDS = 0.5f;
        private const float DELAY_AFTER_ROULETTE = 2.5f;

        


        private void Awake()
        {
            _rewardItemWidgetPool = new(_rewardItemWidgetPrefab, _rewardContainer);
        }

        private void Start()
        {
            _skipButton.onClick.AddListener(OnTakeButtonClick);
            _takeButton.onClick.AddListener(OnTakeButtonClick);
            _stopRouletteButton.onClick.AddListener(OnStopRouletteButtonClick);
        }

        public void Display(int stars, List<(ItemType, int)> rewards, bool showBonus)
        {
            _stars = stars;
            _showBonus = showBonus;
            _bonusRect.gameObject.SetActive(showBonus);

            _rewardItemWidgetPool.Clear();
            foreach (var reward in rewards)
            {
                var widget = _rewardItemWidgetPool.Get();
                widget.Display(reward.Item1, reward.Item2);

                if (reward.Item1 == ItemType.Coins)
                {
                    _originRewardCoins = reward.Item2;
                    _coinItemWidget = widget;
                }
            }
        }


        public void AnimateShow()
        {
            _confettiParticles.ForEach(particle => particle.Play());

            _takeButton.gameObject.SetActive(false);

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
                bool showStar = i < _stars;
                _starShineParticles[i].Stop();

                if (showStar)
                {
                    int index = i;
                    _starImages[i].gameObject.SetActive(true);
                    sequence.Append(TweenHub.ShowFromFadeRotate(_starImages[i]));
                    sequence.AppendCallback(() =>
                    {
                        _starFlashParticles[index].Play();
                        _starShineParticles[index].Play();
                    });
                }
                else _starImages[i].gameObject.SetActive(false);
            }

            sequence.AppendInterval(DELAY_AFTER_STARS);
            sequence.Append(TweenHub.PopShow(_rewardsRect));
            sequence.AppendInterval(DELAY_AFTER_REWARDS);

            _skipButton.gameObject.SetActive(_showBonus);
            _stopRouletteButton.gameObject.SetActive(_showBonus);
            _stopRouletteButton.interactable = _showBonus;
            _takeButton.gameObject.SetActive(!_showBonus);

            if (_showBonus)
            {
                sequence.AppendCallback(() => _roulette.Start());
                sequence.Append(TweenHub.PopShow(_bonusRect));
                sequence.AppendInterval(DELAY_AFTER_ROULETTE);
                sequence.Append(TweenHub.PopShow(_skipButton.transform));
            }
            else
            {
                sequence.Append(TweenHub.PopShow(_takeButton.transform));
            }

            
        }


        private void OnTakeButtonClick()
        {
            ScreenFade.Fade(onCompleted: () =>
            {
                levelController.Item.HideVictoryScreen();
                levelController.Item.StartNextLevelOrShowLevelSelection();
            });
        }

        private void OnStopRouletteButtonClick()
        {
            ads.Item.ShowRewarded(AdKey.LevelRevive, onRewarded: () =>
            {
                _roulette.Stop(out int sectorIndex);
                ApplyRouletteBonus(sectorIndex);
            });
        }


        private void ApplyRouletteBonus(int sectorIndex)
        {
            _skipButton.gameObject.SetActive(false);
            _stopRouletteButton.interactable = false;

            _stopRouletteButton.GetComponent<IAnimation>().Stop();

            DOTween.Sequence().Attach(gameObject)
                .Append(TweenHub.Hide(_stopRouletteButton.transform))
                .AppendCallback(() =>
                {
                    _stopRouletteButton.gameObject.SetActive(false);
                    _takeButton.gameObject.SetActive(true);
                })
                .Append(TweenHub.PopShow(_takeButton.transform));

            int multiplier = sectorIndex switch
            {
                0 or 4 => 2,
                1 or 3 => 3,
                2 => 4,

                _ => 0
            };

            GameState.Items.Add(ItemType.Coins, _originRewardCoins * (multiplier - 1));
            _coinItemWidget.AnimateIncrease(_originRewardCoins * multiplier);
        }



    }

}


