using AssetKits.ParticleImage;
using Coffee.UIExtensions;
using Cysharp.Threading.Tasks;
using DevNote;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamebox
{
    public class LeadersButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _shineImage;
        [SerializeField] private UIParticle _flashParticle;
        [SerializeField] private ParticleImage _ratingParticle;
        [SerializeField] private TextMeshProUGUI _rankText;

        private Tween _buttonTween;

        private readonly Holder<LeadersController> leadersController = new();
        private readonly Holder<LevelController> levelController = new();


        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            leadersController.Item.ShowLeadersWindow();
        }

        public void Display()
        {
            _rankText.text = IConfigs.Leaders.GetRank(IGameState.Level).ToString();
        }


        public async void AnimateUpdateRank(float delay)
        {
            _button.image.raycastTarget = false;

            _ratingParticle.OnFirstParticleFinished -= OnRatingParticleFinished;
            _ratingParticle.OnFirstParticleFinished += OnRatingParticleFinished;

            await UniTask.WaitForSeconds(delay);

            Sound.Play(SoundName.ShowLeaderParticle);
            _ratingParticle.Play();
        }


        private void OnRatingParticleFinished(ParticleImage particleImage)
        {
            const float DURATION = 0.5f;
            const float TO_SCALE = 1.4f;

            particleImage.OnFirstParticleFinished -= OnRatingParticleFinished;

            _flashParticle.Play();

            Sound.Play(SoundName.LeaderParticle);

            _buttonTween?.Kill();
            _buttonTween = DOTween.Sequence()
                .Append(_button.transform.DOScale(TO_SCALE, DURATION / 2f).SetEase(Ease.OutQuad))
                .Append(_button.transform.DOScale(1f, DURATION / 2f).SetEase(Ease.InQuad))
                .OnComplete(() => _button.image.raycastTarget = true);

            Display();
        }
    }
}
