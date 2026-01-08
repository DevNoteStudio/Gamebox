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

        private const float SCALE = 1.7f;
        private const float START_POSITION_Y = -500f;
        private const float ROTATE_DURATION = 0.8f;
        private const float SHOW_NAME_DURATION = 0.5f;

        private Tween _tween;

        public void AnimateShowCard(CardType cardType, int amount)
        {
            var rarityType = IConfigs.Gamebox.GetCardRarity(cardType);
            _nameText.text = IConfigs.Gamebox.GetCardName(cardType);
            _rarityText.text = IConfigs.Gamebox.GetRarityName(rarityType);
            _rarityText.color = IConfigs.Internal.GetRarityTextColor(rarityType);
            _amountText.text = $"x{amount}";

            AnimateShow();
        }

        public void AnimateShowBooster(ItemKey boosterItemKey, int amount)
        {
            _nameText.text = IConfigs.Gamebox.GetItemName(boosterItemKey);
            _rarityText.text = Localization.GetLocalizedText("booster");
            _amountText.text = $"x{amount}";

            AnimateShow();
        }


        private void AnimateShow()
        {
            transform.SetParent(_targetRect);
            transform.localPosition = Vector3.up * START_POSITION_Y;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.zero;
            _titleRect.localScale = Vector3.zero;
            _amountText.transform.localScale = Vector3.zero;

            if (_isCard) _upgradeAvailableRect.localScale = Vector3.zero;

            _tween?.Kill();
            var sequence = DOTween.Sequence()
                .Append(transform.DOLocalRotate(Vector3.up * 360f, ROTATE_DURATION, RotateMode.LocalAxisAdd))
                .Join(transform.DOLocalMove(Vector3.zero, ROTATE_DURATION).SetEase(Ease.OutFlash))
                .Join(transform.DOScale(SCALE, ROTATE_DURATION).SetEase(Ease.OutFlash))
                .Append(_titleRect.DOScale(1f, SHOW_NAME_DURATION).SetEase(Ease.OutBack))
                .Join(_amountText.transform.DOScale(1f, SHOW_NAME_DURATION).SetEase(Ease.OutBack));

            if (_isCard)
                sequence.Append(_upgradeAvailableRect.DOScale(1f, SHOW_NAME_DURATION).SetEase(Ease.OutBack));

            _tween = sequence;
        }


    }
}
