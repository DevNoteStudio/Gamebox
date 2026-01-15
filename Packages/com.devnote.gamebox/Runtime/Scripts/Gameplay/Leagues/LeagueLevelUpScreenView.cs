using System.Collections.Generic;
using Coffee.UIExtensions;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class LeagueLevelUpScreenView : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _leagueIconImage;
        [SerializeField] private RectTransform _leagueRect;
        [SerializeField] private TextMeshProUGUI _leagueStageText;
        [SerializeField] private TextMeshProUGUI _leagueNameText;
        [SerializeField] private UIParticle _flashParticle;
        [SerializeField] private UIParticle _shineParticle;
        [SerializeField] private Button _takeButton;
        [SerializeField] private RectTransform _titleRect;
        [SerializeField] private RectTransform _rewardRect;
        [SerializeField] private SoundUnit _levelUpSound;
        [Space]
        [SerializeField] private LeagueRewardView _gemRewardView;
        [SerializeField] private LeagueRewardView _boxRewardView;
        [SerializeField] private LeagueRewardView _unlockedBoosterView;
        [SerializeField] private LeagueRewardView _unlockedLocationView;

        
        private List<ItemPack> _rewards;

        private readonly Holder<LeagueController> leagueController = new();
        private readonly Holder<RollupController> rollupController = new();

        private const float SHAKE_DURATION = 1.5f;
        private const float SHOW_DELAY_1 = 1f;
        private const float SHOW_DELAY_2 = 1.5f;


        private void Start()
        {
            _takeButton.onClick.AddListener(OnTakeButtonClick);
        }

        public void AnimateDisplay(LeagueType nextLeague)
        {
            var config = IConfigs.Gamebox;
            var previousLeague = nextLeague - 1;

            DisplayRewards(nextLeague);

            _shineParticle.Clear();
            _shineParticle.Stop();
            _levelUpSound.Play();

            _leagueStageText.text = config.GetLeagueStageSymbol(previousLeague);
            _leagueIconImage.sprite = config.GetLeagueSprite(previousLeague);
            _leagueNameText.text = config.GetLeagueName(previousLeague);

            var leagueIconRect = _leagueIconImage.transform;

            var sequence = DOTween.Sequence();

            sequence.Append(leagueIconRect.DOShakePosition
                (SHAKE_DURATION, strength: 30, vibrato: 20, fadeOut: false, randomnessMode: ShakeRandomnessMode.Harmonic));

            sequence.Join(TweenHub.Show(_leagueRect, SHAKE_DURATION));
            sequence.Join(TweenHub.Fade(_backgroundImage));
                
            sequence.AppendCallback(() => 
            {
                _flashParticle.Play();
                _shineParticle.Play();
                _leagueIconImage.sprite = config.GetLeagueSprite(nextLeague);
                _leagueStageText.text = config.GetLeagueStageSymbol(nextLeague);
                _leagueNameText.text = config.GetLeagueName(nextLeague);
            });

            sequence.Append(TweenHub.Show(_titleRect))
                .AppendInterval(SHOW_DELAY_1)
                .Append(TweenHub.Show(_rewardRect, playSound: true))
                .AppendInterval(SHOW_DELAY_2)
                .Append(TweenHub.Show(_takeButton.transform, playSound: true));


        }

        private void DisplayRewards(LeagueType nextLeague)
        {
            // Unlocked location
            if (IConfigs.Gamebox.TryGetUnlockedLocation(nextLeague, out int unlockedLocationIndex))
            {
                _unlockedLocationView.gameObject.SetActive(true);
                _unlockedLocationView.DisplayUnlockedLocation(unlockedLocationIndex);
            }
            else _unlockedLocationView.gameObject.SetActive(false);

            // Gems
            _rewards = IConfigs.Gamebox.GetLeagueRewardItems(nextLeague);
            if (_rewards.TryFind(itemPack => itemPack.itemKey == ItemKey.Gems, out var gemPack))
            {
                _gemRewardView.gameObject.SetActive(true);
                _gemRewardView.DisplayRewardItem(ItemKey.Gems, gemPack.amount);
            }
            else _gemRewardView.gameObject.SetActive(false);

            // Box
            if (_rewards.TryFind(itemPack => itemPack.itemKey.IsBox(), out var boxPack))
            {
                _boxRewardView.gameObject.SetActive(true);
                _boxRewardView.DisplayRewardBox(boxPack.itemKey);
            }
            else _boxRewardView.gameObject.SetActive(false);

            // Unlocked booster
            if (_rewards.TryFind(itemPack => itemPack.itemKey.IsBooster(), out var boosterPack))
            {
                _unlockedBoosterView.gameObject.SetActive(true);
                _unlockedBoosterView.DisplayUnlockedBooster(boosterPack.itemKey);
            }
            else _unlockedBoosterView.gameObject.SetActive(false);
        }



        private void OnTakeButtonClick()
        {
            leagueController.Item.HideLeagueLevelUpScreen();

            
            if (_rewards.Exists((itemPack) => itemPack.itemKey == ItemKey.Coins))
            {
                int coins = _rewards.Find((itemPack) => itemPack.itemKey == ItemKey.Coins).amount;
                rollupController.Item.RollupCoins(coins);
            }
            
        }

    }
}
