using DevNote;
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


    }
}
