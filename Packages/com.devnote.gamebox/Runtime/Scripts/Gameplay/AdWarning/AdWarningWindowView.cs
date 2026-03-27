using DevNote;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class AdWarningWindowView : Window
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Button _noAdsButton;
        [SerializeField] private TextMeshProUGUI _noAdsButtonText;

        private float _secondsLeft;
        private bool _timerPaused = true;

        private readonly Holder<IAds> ads = new();
        private readonly Holder<IPurchase> purchase = new();
        private readonly Holder<IRemote> remote = new();
        private readonly Holder<AdWarningController> adWarningController = new();


        private void Start()
        {
            _noAdsButton.onClick.AddListener(OnPurchaseNoAdsButtonClick);
        }


        public AdWarningWindowView Display()
        {
            var showNoAdsPrice = remote.Item.GetBool(RemoteKey.ShowNoAdsPrice);

            string priceText = $"<sprite=0> {purchase.Item.GetPriceString(ProductKey.NoAds)}";
            string noPriceText = Localization.GetLocalizedText("ad_warning_disable_ads");

            _noAdsButtonText.text = showNoAdsPrice ? priceText : noPriceText;

            StartTimer();

            return this;
        }


        private void OnPurchaseNoAdsButtonClick()
        {
            _timerPaused = true;

            purchase.Item.Purchase(ProductKey.NoAds,
            onSuccess: () =>
            {
                adWarningController.Item.HideAdWarningWindow(showInterstitial: false);
            },
            onError: () =>
            {
                _timerPaused = false;
            });
        }



        private void StartTimer()
        {
            _timerText.gameObject.SetActive(true);

            _secondsLeft = remote.Item.GetFloat(RemoteKey.AdWarningDuration);
            _timerPaused = false;
        }


        private void Update()
        {
            if (_timerPaused) return;

            _secondsLeft -= Time.deltaTime;

            int secondsLeft = Mathf.CeilToInt(_secondsLeft);
            _timerText.text = $"{secondsLeft} {Localization.GetLocalizedText("ad_timer_seconds")}";

            if (secondsLeft <= 0)
            {
                _timerPaused = true;
                _timerText.gameObject.SetActive(false);
                adWarningController.Item.HideAdWarningWindow(showInterstitial: true);
            }

        }



    }
}
