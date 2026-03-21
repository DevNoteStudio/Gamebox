using System;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Gamebox
{
    public class LeagueProgressView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _frameImage;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private TextMeshProUGUI _stageText;
        [SerializeField] private Slider _slider;

        public const float FILL_DURATION = 1.2f;


        private void OnEnable()
        {
            IGameState.Rating.OnChanged += OnRatingChanged;
            Display(IGameState.Rating.Value);
        }
        private void OnDisable() => IGameState.Rating.OnChanged -= OnRatingChanged;

        private void OnRatingChanged() => Display(IGameState.Rating.Value);


        public void Display(int rating)
        {
            var leagueType = IConfigs.Gamebox.GetLeagueType(rating);
            var config = IConfigs.Gamebox;

            _iconImage.sprite = config.GetLeagueSprite(leagueType);
            _frameImage.color = config.GetLeagueFrameColor(leagueType);

            _stageText.text = config.GetLeagueStageSymbol(leagueType);

            bool nextLeagueExists = !config.IsLastLeague(leagueType);
            int currentRating = config.GetCurrentLeagueRating(rating);

            if (nextLeagueExists)
            {
                var nextLeague = (LeagueType)((int)leagueType + 1);
                int nextRequire = config.GetLeagueRatingRequire(nextLeague);

                _progressText.text = $"{currentRating}<size=80%>/{nextRequire}";
                _slider.value = (float)currentRating / nextRequire;
            }
            else
            {
                _progressText.text = $"+{currentRating}";
                _slider.value = 1f;
            }


        }


        public void AnimateProgressFill(int fromRating, int toRating, Action<LeagueType> onNextLeagueReached = null)
        {
            var config = IConfigs.Gamebox;

            var fromLeagueType = config.GetLeagueType(fromRating);
            int fromLeagueRating = config.GetCurrentLeagueRating(fromRating);

            if (config.IsLastLeague(fromLeagueType))
            {
                /*
                //var fillAudioSource = await Sound.PlayAsync(SoundName.RatingFill);

                int toLeagueRating = config.GetCurrentLeagueRating(toRating);
                int currentRating = fromLeagueRating;
                Debug.Log(fromLeagueRating + " " + toLeagueRating);

                DOTween.To(() => currentRating, x => currentRating = x, toLeagueRating, FILL_DURATION)
                .SetEase(Ease.Linear)
                .OnUpdate(() => _progressText.text = $"+{currentRating}")
                .OnComplete(() => 
                {
                    _progressText.text = $"+{toLeagueRating}";
                    fillAudioSource.Stop();
                });
                */
            }

            else
            {
                var toLeagueType = config.GetLeagueType(toRating);

                int requiredLeagueRating = config.GetLeagueRatingRequire(fromLeagueType + 1);
                float fromFill = (float)fromLeagueRating / requiredLeagueRating;

                if (fromLeagueType == toLeagueType) // Same league
                {
                    int toLeagueRating = config.GetCurrentLeagueRating(toRating);
                    float toFill = (float)toLeagueRating / requiredLeagueRating;

                    AnimateFillInsideOneLeague(fromFill, toFill, fromLeagueType, FILL_DURATION);
                }
                else // Reach next league
                {
                    AnimateFillInsideOneLeague(fromFill, 1f, fromLeagueType, FILL_DURATION,
                    onCompleted: () =>
                    {
                        Display(toRating);
                        onNextLeagueReached?.Invoke(toLeagueType);
                    });
                }
            }
        }

        private void AnimateFillInsideOneLeague
            (float fromFill, float toFill, LeagueType leagueType, float duration, Action onCompleted = null)
        {
            //var fillAudioSource = await Sound.PlayAsync(SoundName.RatingFill);
            int requiredRating = IConfigs.Gamebox.GetLeagueRatingRequire(leagueType + 1);

            _slider.value = fromFill;
            _slider.DOValue(toFill, duration).SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                int currentRating = (int)(_slider.value * requiredRating);
                _progressText.text = $"{currentRating}<size=80%>/{requiredRating}";
            })
            .OnComplete(() =>
            {
                int completedRating = Mathf.RoundToInt(_slider.value * requiredRating);
                _progressText.text = $"{completedRating}<size=80%>/{requiredRating}";

                //fillAudioSource.Stop();
                onCompleted?.Invoke();
            });

        }





    }
}
