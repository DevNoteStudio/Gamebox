using System.Collections.Generic;
using Coffee.UIExtensions;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
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
        [SerializeField] private ItemWidgetView _rewardItemPrefab;
        [SerializeField] private UnlockedContentWidgetView _unlockedLocationWidget;
        [SerializeField] private UnlockedContentWidgetView _unlockedItemWidget;


        private Pool<ItemWidgetView> _rewardItemWidgetsPool;
        private List<ItemPack> _rewards;

        private readonly Holder<LeagueController> leagueController = new();
        private readonly Holder<RollupController> rollupController = new();

        private const float SHAKE_DURATION = 1.5f;
        private const float SHOW_DELAY_1 = 1f;
        private const float SHOW_DELAY_2 = 1.5f;

        private void Awake()
        {
            _rewardItemWidgetsPool = new(_rewardItemPrefab, _rewardItemPrefab.transform.parent);
        }


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
            bool showUnlockedLocation = IConfigs.Gamebox.TryGetUnlockedLocation
                (nextLeague, out int unlockedLocationIndex);

            _unlockedLocationWidget.gameObject.SetActive(showUnlockedLocation);

            if (showUnlockedLocation)
                _unlockedLocationWidget.DisplayUnlockedLocation(unlockedLocationIndex);

            _rewards = IConfigs.Gamebox.GetLeagueRewardItems(nextLeague);

            _rewardItemWidgetsPool.Clear();
            foreach (var rewardItem in _rewards)
                _rewardItemWidgetsPool.Get().Display(rewardItem.itemKey, rewardItem.amount);
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
