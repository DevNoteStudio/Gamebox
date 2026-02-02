using System;
using DevNote;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Gamebox
{
    public class BoxItemAnimation : MonoBehaviour
    {
        [SerializeField] private bool _isCard;
        [SerializeField] private RectTransform _targetRect;
        [SerializeField] private RectTransform _titleRect;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _rarityText;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField, ShowIf(nameof(_isCard))] private RectTransform _upgradeAvailableRect;

        private Tween _tween;

        private readonly Holder<SoundController> soundController = new();

        private const float SCALE = 1.7f;
        private const float START_POSITION_Y = -500f;
        private const float ROTATE_DURATION = 0.8f;
        private const float SHOW_NAME_DURATION = 0.5f;


        

        public void AnimateShowCard(CardType cardType, int amount, int cardIndex, Action onCompleted = null)
        {
            if (cardIndex != 0) 
                soundController.Item.PlayOpenBoxReward();

            var rarityType = IConfigs.Gamebox.GetCardRarity(cardType);
            _nameText.text = IConfigs.Gamebox.GetCardName(cardType);
            _rarityText.text = IConfigs.Gamebox.GetRarityName(rarityType);
            _rarityText.color = IConfigs.Internal.GetRarityTextColor(rarityType);
            _amountText.text = $"x{amount}";

            _upgradeAvailableRect.gameObject.SetActive(IGameState.Cards.UpgradeAvailable(cardType));

            AnimateShow(onCompleted);
        }

        public void AnimateShowBooster(ItemKey boosterItemKey, int amount, Action onCompleted = null)
        {
            soundController.Item.PlayOpenBoxReward();

            _nameText.text = IConfigs.Gamebox.GetItemName(boosterItemKey);
            _rarityText.text = Localization.GetLocalizedText("booster");
            _amountText.text = $"x{amount}";

            AnimateShow(onCompleted);
        }


        private void AnimateShow(Action onCompleted)
        {
            transform.SetParent(_targetRect);
            transform.localPosition = Vector3.up * START_POSITION_Y;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.zero;
            _titleRect.localScale = Vector3.zero;
            _amountText.transform.localScale = Vector3.zero;

            if (_upgradeAvailableRect != null)
                _upgradeAvailableRect.localScale = Vector3.zero;

            _tween?.Kill();
            var sequence = DOTween.Sequence()
                .Append(transform.DOLocalRotate(Vector3.up * 360f, ROTATE_DURATION, RotateMode.LocalAxisAdd))
                .Join(transform.DOLocalMove(Vector3.zero, ROTATE_DURATION).SetEase(Ease.OutFlash))
                .Join(transform.DOScale(SCALE, ROTATE_DURATION).SetEase(Ease.OutFlash))
                .Append(_titleRect.DOScale(1f, SHOW_NAME_DURATION).SetEase(Ease.OutBack))
                .Join(_amountText.transform.DOScale(1f, SHOW_NAME_DURATION).SetEase(Ease.OutBack));

            if (_upgradeAvailableRect != null)
                sequence.Append(_upgradeAvailableRect.DOScale(1f, SHOW_NAME_DURATION).SetEase(Ease.OutBack));

            sequence.OnComplete(() => onCompleted?.Invoke());
            _tween = sequence;
        }


    }
}
