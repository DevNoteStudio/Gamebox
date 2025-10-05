using System;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class NoAdsWindowView : MonoBehaviour
    {
        [SerializeField] private Image _fadeImage;
        [SerializeField] private RectTransform _windowRect;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _purchaseButton;
        [SerializeField] private Button _closeButton;

        private Tween _currentTween;

        private readonly Holder<PopupController> popupController = new();
        private readonly Holder<IPurchase> purchase = new();


        private void Start()
        {
            _purchaseButton.onClick.AddListener(OnPurchaseButtonClick);
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        public NoAdsWindowView Display()
        {
            _priceText.text = purchase.Item.GetPriceString(ProductKey.NoAds);
            return this;
        }

        private void OnCloseButtonClick()
        {
            popupController.Item.HideNoAdsWindow();
        }

        private void OnPurchaseButtonClick()
        {
            purchase.Item.Purchase(ProductKey.NoAds, 
                onSuccess: popupController.Item.HideNoAdsWindow);
        }


        public void AnimateShow()
        {
            TimeMode.SetActive(TimeMode.Mode.Pause, true);

            _currentTween?.Kill();
            _currentTween = DOTween.Sequence().Attach(gameObject)
                .Append(TweenHub.Show(_windowRect, playSound: true))
                .Join(TweenHub.Fade(_fadeImage))
                .Append(TweenHub.DelayedShow(_closeButton.transform))
                .SetUpdate(true);
        }


        public void AnimateHide(Action onCompleted)
        {
            TimeMode.SetActive(TimeMode.Mode.Pause, false);

            _currentTween?.Kill();
            _currentTween = DOTween.Sequence().Attach(gameObject)
                .Append(TweenHub.Hide(_windowRect))
                .Join(TweenHub.Unfade(_fadeImage)).OnComplete(() => onCompleted?.Invoke())
                .SetUpdate(true);
        }



    }
}

