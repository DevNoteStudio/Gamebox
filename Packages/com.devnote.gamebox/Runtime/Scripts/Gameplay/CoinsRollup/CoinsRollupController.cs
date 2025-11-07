using System.Collections.Generic;
using AssetKits.ParticleImage;
using DevNote;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gamebox
{
    public class CoinsRollupController
    {
        private List<ItemCounterView> _coinsCounters = new();
        private ParticleImage _coinsParticle;
        private ItemCounterView _coinsCounter;
        private int _fromValue, _toValue;

        private readonly Viewer<CommonCoinsCounterView> commonCoinsCounterViewer;


        public CoinsRollupController()
        {
            commonCoinsCounterViewer = new(IConfigs.GetViewPrefab<CommonCoinsCounterView>());
        }


        public void AddCoinsRollupTarget(ItemCounterView coinsCounter)
        {
            if (!coinsCounter.transform.parent.TryGetComponent<CommonCoinsCounterView>(out _))
                _coinsCounters.Add(coinsCounter);
        }
        

        public void RemoveCoinsRollupTarget(ItemCounterView coinsCounter)
            => _coinsCounters.Remove(coinsCounter);


        public void RollupCoins(int addAmount)
        {
            int targetAmount = IGameState.Items.Get(ItemKey.Coins);

            IConfigs.Gamebox.CoinsRollupStartSound.Play();

            _fromValue = targetAmount - addAmount;
            _toValue = targetAmount;

            _coinsParticle = Object.Instantiate(IConfigs.Gamebox.CoinsRollupParticlePrefab, UI.Container);
            _coinsParticle.onLastParticleFinished.AddListener(OnLastCoinParticleFinished);
            _coinsParticle.onAnyParticleFinished.AddListener(OnAnyCoinParticleFinished);
            _coinsParticle.onFirstParticleFinished.AddListener(OnFirstCoinParticleFinished);

            _coinsCounter = null;

            foreach (var coinsCounter in _coinsCounters)
            {
                if ((coinsCounter.transform as RectTransform).IsCoveredByOtherElement() == false)
                {
                    _coinsCounter = coinsCounter;
                    break;
                }
            }
            if (_coinsCounter == null)
            {
                var coinsCounter = commonCoinsCounterViewer.ShowExpand(UI.Container);
                coinsCounter.AnimateShowAndHide(onCompleted: commonCoinsCounterViewer.Hide);
                _coinsCounter = coinsCounter.ItemCounter;
            }

            _coinsCounter.Display(_fromValue);
            _coinsParticle.attractorTarget = _coinsCounter.transform;
        }

        private void OnFirstCoinParticleFinished()
        {
            IConfigs.Gamebox.CoinsRollupFinishSound.Play();
        }

        private void OnAnyCoinParticleFinished()
        {
            float progress = 1f - (float)_coinsParticle.particleCount / _coinsParticle.Bursts[0].count;
            int currentAmount = (int)Mathf.Lerp(_fromValue, _toValue, progress);
            _coinsCounter.Display(currentAmount);
        }

        private void OnLastCoinParticleFinished()
        {
            Object.Destroy(_coinsParticle.gameObject);
            _coinsCounter.Display(_toValue);
        }
        




    }
}
