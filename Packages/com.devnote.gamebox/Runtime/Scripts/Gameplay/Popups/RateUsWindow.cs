using DevNote;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class RateUsWindow : Window
    {
        [SerializeField] private Button _rateButton;
        [SerializeField] private Button _closeButton;

        private readonly Holder<PopupController> popupController = new();
        private readonly Holder<IReview> review = new();


        private void Start()
        {
            _rateButton.onClick.AddListener(OnRateButtonClick);
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        private void OnCloseButtonClick() => popupController.Item.HidePopup(PopupType.RateUs);


        private void OnRateButtonClick()
        {
            review.Item.Rate();
            popupController.Item.HidePopup(PopupType.RateUs);
        }

    }
}

