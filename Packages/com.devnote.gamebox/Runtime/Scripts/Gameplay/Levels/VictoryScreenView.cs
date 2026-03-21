using System.Collections.Generic;
using Coffee.UIExtensions;
using Cysharp.Threading.Tasks;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Gamebox
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
        private int _totalRewardCoins;
        private int _fromRating;
        private int _toRating;
        

        private readonly Holder<LevelController> levelController = new();
        private readonly Holder<IAds> ads = new();
        private readonly Holder<LeagueController> leagueController = new();
        private readonly Holder<RollupController> rollupController = new();
        private readonly Holder<PopupController> popupController = new();
        private readonly Holder<CurrencyController> currencyController = new();

        private const float FADE_DURATION = 0.8f;
        private const float DELAY_BEFORE_VICTORY_SOUND = 0.3f;
        private const float TITLE_DURATION = 0.75f;
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

        public void Display(int stars, int fromRating, int toRating, List<ItemPack> rewards)
        {
            bool showBonus = levelController.Item.CompletedLevel >= IConfigs.Gamebox.WinRouletteFromLevel;

            _stars = stars;
            _showBonus = showBonus;
            _bonusRect.gameObject.SetActive(showBonus);

            _fromRating = fromRating; 
            _toRating = toRating;

            _rewardItemWidgetPool.Clear();
            foreach (var reward in rewards)
            {
                var widget = _rewardItemWidgetPool.Get();
                widget.Display(reward.itemKey, reward.amount);

                if (reward.itemKey == ItemKey.Coins)
                {
                    _originRewardCoins = reward.amount;
                    _totalRewardCoins = reward.amount;
                    _coinItemWidget = widget;
                }
            }
        }


        public void AnimateShow()
        {
            DOVirtual.DelayedCall(DELAY_BEFORE_VICTORY_SOUND, () => IConfigs.AudioHub.WinShow?.Play());

            _takeButton.gameObject.SetActive(false);

            _titleRect.localScale = new Vector3(0f, 1f, 1f);
            _titleRect.localPosition = new Vector2(0f, -UI.TARGET_RESOLUTION.y / 2f + TITLE_OFFSET_Y);
            _starFlashParticles.ForEach(particle => particle.Stop());
            _starShineParticles.ForEach(particle => particle.Stop());

            _currentTween?.Kill();
            var sequence = DOTween.Sequence().SetLink(gameObject, LinkBehaviour.KillOnDisable)
                .Append(TweenHub.Fade(_fadeImage, FADE_DURATION))
                .Append(_titleRect.DOScaleX(1f, TITLE_DURATION).SetEase(Ease.OutBack))

                .AppendCallback(() =>
                {
                    IConfigs.AudioHub.WinConfetti?.Play();
                    _confettiParticles.ForEach(particle => particle.Play());
                })

                .Append(_titleRect.DOLocalMoveY(0f, TITLE_DURATION).SetEase(Ease.InOutFlash))
                .Append(TweenHub.Show(_starsRect));

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
                        IConfigs.AudioHub.WinStars[index]?.Play();
                    });
                }
                else _starImages[i].gameObject.SetActive(false);
            }

            sequence.AppendInterval(DELAY_AFTER_STARS);

            sequence.Append(TweenHub.Show(_rewardsRect, playSound: true));
            sequence.AppendInterval(DELAY_AFTER_REWARDS);

            _skipButton.gameObject.SetActive(_showBonus);
            _stopRouletteButton.gameObject.SetActive(_showBonus);
            _stopRouletteButton.interactable = _showBonus;
            _takeButton.gameObject.SetActive(!_showBonus);

            if (_showBonus)
            {
                sequence.AppendCallback(() => _roulette.StartSpin());
                sequence.AppendCallback(() => IConfigs.AudioHub.ShowElement?.Play());
                sequence.Append(TweenHub.Show(_bonusRect));
                sequence.AppendInterval(DELAY_AFTER_ROULETTE);
                sequence.Append(TweenHub.Show(_skipButton.transform));
            }
            else
            {
                sequence.Append(TweenHub.Show(_takeButton.transform));
            }

            
        }

        private void OnTakeButtonClick()
        {
            UI.ScreenFade(onCompleted: () =>
            {
                levelController.Item.HideWinScreen();
                levelController.Item.StartLevel(IGameState.Level);
            });
        }

        private void OnStopRouletteButtonClick()
        {
            _roulette.Stop(out int sectorIndex);

            ads.Item.ShowRewarded(AdKey.VictoryRoulette, onRewarded: () =>
            {
                IConfigs.AudioHub.RouletteStop?.Play();
                ApplyRouletteBonus(sectorIndex);
            }, 
            callback: (status) =>
            {
                if (status != AdShowStatus.Success)
                    _roulette.StartSpin();
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
                .Append(TweenHub.Show(_takeButton.transform));

            int multiplier = sectorIndex switch
            {
                0 or 4 => 2,
                1 or 3 => 3,
                2 => 4,

                _ => 0
            };

            IGameState.Items.Add(ItemKey.Coins, _originRewardCoins * (multiplier - 1));
            _coinItemWidget.AnimateIncrease(_originRewardCoins * multiplier);

            _totalRewardCoins = _originRewardCoins * multiplier;
        }



    }

}


