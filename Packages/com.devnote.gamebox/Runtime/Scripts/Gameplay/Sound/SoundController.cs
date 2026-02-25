using DevNote;
using DG.Tweening;
using UnityEngine;

namespace Gamebox
{
    public class SoundController
    {
        private int _boxCapacity;
        private int _currentBoxRewardIndex;


        private readonly Vector2 BOX_REWARD_PITCH_INTERVAL = new Vector2(0.8f, 1.1f);


        public void SetBoxCapacity(int value)
        {
            _currentBoxRewardIndex = 0;
            _boxCapacity = value;
        }


        public async void PlayOpenBoxReward()
        {
            var audioSource = await Sound.PlayAsync(SoundName.OpenCard);

            float interpolation = (float)_currentBoxRewardIndex / (_boxCapacity - 1);
            audioSource.pitch = Mathf.Lerp(BOX_REWARD_PITCH_INTERVAL.x, BOX_REWARD_PITCH_INTERVAL.y, interpolation);

            _currentBoxRewardIndex++;
        }

        private Tween _scoreFillSoundTween;
        public async void PlayScoreFill(float fromProgress, float toProgress)
        {
            const float MIN_PITCH = 1f;
            const float MAX_PITCH = 1.5f;
            const float INTERVAL = 0.06f;

            
            float addProgress = toProgress - fromProgress;
            float pitchInterval = MAX_PITCH - MIN_PITCH;
            float fromPitch = MIN_PITCH + pitchInterval * fromProgress;
            float toPitch = MIN_PITCH + pitchInterval * toProgress;

            int loops = 
                addProgress > 0.15f ? 5 :
                addProgress > 0.1f ? 4 : 3;

            float addPitch = (toPitch - fromPitch) / (loops - 1);

            float currentPitch = fromPitch;

            _scoreFillSoundTween?.Kill();

            var audioSource = await Sound.PlayAsync(SoundName.AddScore);
            
            _scoreFillSoundTween = DOTween.Sequence()
                .AppendInterval(INTERVAL)
                .AppendCallback(() =>
                {
                    currentPitch += addPitch;
                    audioSource.pitch = currentPitch;
                    audioSource.Play();
                })
                .SetLoops(loops - 1);
            

        }


    }
}
