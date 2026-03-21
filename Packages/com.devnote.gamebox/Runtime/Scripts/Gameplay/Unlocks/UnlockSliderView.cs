using Coffee.UIExtensions;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class UnlockSliderView : MonoBehaviour
    {
        [SerializeField] private RectTransform _arrowRect;
        [SerializeField] private RectTransform _currentContentRect;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _currentContentBackgroundImage;
        [SerializeField] private Image _previousContentIconImage;
        [SerializeField] private Image _currentContentIconImage;
        [SerializeField] private Image _nextContentIconImage;
        [SerializeField] private TextMeshProUGUI _previousUnlockLevelText;
        [SerializeField] private TextMeshProUGUI _currentUnlockLevelText;
        [SerializeField] private TextMeshProUGUI _nextUnlockLevelText;
        [SerializeField] private UIParticle _unlockParticle;


        private readonly Vector2 ARROW_START_POSITION = new Vector2(-300f, -120f);
        private readonly Color FADE_COLOR = new Color(0f, 0f, 0f, 0.5f);



        public void Display(UnlockKey previuosKey, UnlockKey currentKey, UnlockKey nextKey)
        {
            _previousContentIconImage.LoadSprite(AssetLoader.LoadUnlockSprite(previuosKey));
            _currentContentIconImage.LoadSprite(AssetLoader.LoadUnlockSprite(currentKey));
            _nextContentIconImage.LoadSprite(AssetLoader.LoadUnlockSprite(nextKey));

            int previousLevel = IConfigs.Gamebox.GetUnlockLevel(previuosKey);
            _previousUnlockLevelText.text = Localization.GetLocalizedText("level_short")
                .Replace("{VALUE}", previousLevel.ToString());

            int currentLevel = IConfigs.Gamebox.GetUnlockLevel(currentKey);
            _currentUnlockLevelText.text = Localization.GetLocalizedText("level_short")
                .Replace("{VALUE}", currentLevel.ToString());

            int nextLevel = IConfigs.Gamebox.GetUnlockLevel(nextKey);
            _nextUnlockLevelText.text = Localization.GetLocalizedText("level_short")
                .Replace("{VALUE}", nextLevel.ToString());


        }


        public Tween AnimateUnlockNewContent()
        {
            const float ARROW_MOVE_DURATION = 0.7f;
            const float FILL_DURATION = 0.5f;
            const float CONTENT_TO_SCALE = 1.5f;
            const float CONTENT_UP_SCALE = 2f;
            const float CONTENT_SCALE_DURATION = 0.5f;



            _arrowRect.localPosition = ARROW_START_POSITION;
            _fillImage.fillAmount = 0f;
            _currentContentBackgroundImage.fillAmount = 0f;
            _currentContentRect.localScale = Vector3.one;
            _currentContentIconImage.color = FADE_COLOR;


            return DOTween.Sequence()
                .AppendCallback(() => IConfigs.AudioHub.UnlockWindowProgressSliderFilling?.Play())
                .Append(_arrowRect.DOLocalMoveX(0f, ARROW_MOVE_DURATION).SetEase(Ease.OutFlash))
                .Join(_fillImage.DOFillAmount(1f, ARROW_MOVE_DURATION).SetEase(Ease.OutFlash))
                .AppendCallback(() => IConfigs.AudioHub.UnlockWindowProgressIconFilling?.Play())
                .Append(_currentContentBackgroundImage.DOFillAmount(1f, FILL_DURATION).SetEase(Ease.OutFlash))
                .AppendCallback(() => 
                {
                    _currentContentIconImage.color = Color.white;
                    _unlockParticle.Play();
                    IConfigs.AudioHub.UnlockWindowShowProgressIcon?.Play();
                })
                .Append(_currentContentRect.DOScale(CONTENT_UP_SCALE, CONTENT_SCALE_DURATION / 2).SetEase(Ease.OutQuad))
                .Append(_currentContentRect.DOScale(CONTENT_TO_SCALE, CONTENT_SCALE_DURATION / 2).SetEase(Ease.InQuad));
        }




    }
}
