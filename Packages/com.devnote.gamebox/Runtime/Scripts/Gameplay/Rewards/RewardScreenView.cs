using System;
using Coffee.UIExtensions;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class RewardScreenView : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _takeButton;
        [SerializeField] private RectTransform _chestRect;
        [SerializeField] private RectTransform _itemRect;
        [SerializeField] private Image _itemImage;
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private UIParticle _chestParticle;

        private Tween _currentTween;
        private ItemPack _itemPack;

        private readonly Holder<RewardController> rewardController = new();
        private readonly Holder<RollupController> rollupController = new();


        private void Start()
        {
            _openButton.onClick.AddListener(OnOpenButtonClick);
            _takeButton.onClick.AddListener(OnTakeButtonClick);
        }


        public RewardScreenView Display(ItemPack itemPack)
        {
            _itemPack = itemPack;
            _itemImage.LoadSprite(AssetLoader.LoadItemSprite(itemPack.itemKey));

            _itemNameText.text = 
                $"{IConfigs.Gamebox.GetItemName(itemPack.itemKey)} <size=75%>x</size>{itemPack.amount}";

            return this;
        }

        public void AnimateShow()
        {
            //Sound.Play(SoundName.RewardShow);

            _chestRect.gameObject.SetActive(true);
            _itemRect.gameObject.SetActive(false);

            _currentTween?.Kill();
            _currentTween = TweenHub.Show(_chestRect);
        }

        public void AnimateHide(Action onCompleted)
        {
            _currentTween?.Kill();
            _currentTween = TweenHub.Hide(_itemRect).OnComplete(() => onCompleted?.Invoke());
        }


        private void AnimateChestOpening()
        {
            _itemRect.gameObject.SetActive(true);
            _chestParticle.Play();

            //Sound.Play(SoundName.RewardOpen);

            _currentTween?.Kill();
            _currentTween = DOTween.Sequence()
                .Append(TweenHub.Hide(_chestRect))
                .Join(TweenHub.Show(_itemRect));
        }


        private void OnTakeButtonClick()
        {
            IGameState.Items.Add(_itemPack.itemKey, _itemPack.amount);
            rewardController.Item.HideRewardScreen();

            if (_itemPack.itemKey == ItemKey.Coins)
                rollupController.Item.RollupCoins(_itemPack.amount);
        }

        private void OnOpenButtonClick() => AnimateChestOpening();



    }
}

