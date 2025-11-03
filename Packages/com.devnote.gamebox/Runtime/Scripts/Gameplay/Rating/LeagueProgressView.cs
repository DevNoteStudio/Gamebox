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


        private void OnEnable()
        {
            IGameState.Rating.OnChanged += OnRatingChanged;
            Display();
        }
        private void OnDisable() => IGameState.Rating.OnChanged -= OnRatingChanged;

        private void OnRatingChanged() => Display();


        private void Display()
        {
            int rating = IGameState.Rating.Value;
            var leagueType = IConfigs.Gamebox.GetLeagueType(rating);
            var config = IConfigs.Gamebox;

            _iconImage.sprite = config.GetLeagueSprite(leagueType);
            _frameImage.color = config.GetLeagueFrameColor(leagueType);

            _stageText.text = config.GetLeagueStage(leagueType) switch
            {
                1 => "I", 2 => "II", 3 => "III",
                _ => throw null,
            };

            int currentRating = config.GetCurrentLeagueRating(rating);
            int ratingRequire = config.GetLeagueRatingRequire(leagueType);

            _progressText.text = $"{currentRating}/{ratingRequire}";
            _slider.value = (float)currentRating / ratingRequire;



        }


    }
}
