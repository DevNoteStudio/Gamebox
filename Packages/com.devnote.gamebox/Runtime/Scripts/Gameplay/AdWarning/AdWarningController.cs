using System;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    public class AdWarningController
    {
        private readonly Viewer<AdWarningWindowView> adWarningWindowViewer;
        private readonly Holder<IAds> ads = new();

        private Action _onWindowClosed;


        public AdWarningController(LevelController levelController)
        {
            adWarningWindowViewer = new(IConfigs.GetViewPrefab<AdWarningWindowView>());

            levelController.OnLevelCompleted += OnLevelFinished;
            levelController.OnLevelLost += OnLevelFinished;
            levelController.OnLevelExit += OnLevelFinished;
        }

        private void OnLevelFinished() => HideAdWarningWindow(showInterstitial: false);

        public bool TryShowAdWarningWindow(Action onWindowClosed = null)
        {
            bool interstitialsUnlocked = IGameState.Level >= IConfigs.Gamebox.InterstitialsFromLevel;

            if (!DevNote.IGameState.NoAdsPurchased && ads.Item.InterstitialAvailable && interstitialsUnlocked)
            {
                _onWindowClosed = onWindowClosed;
                adWarningWindowViewer.ShowFaded(UI.Container).Display().AnimateShow();
                return true;
            }
            else return false;
        }


        public void HideAdWarningWindow(bool showInterstitial)
        {
            adWarningWindowViewer.View.AnimateHide(onCompleted: () =>
            {
                adWarningWindowViewer.ForceFadedHide();

                if (showInterstitial)
                {
                    ads.Item.ShowInterstitial(AdKey.DuringLevelInterstitial, callback: (status) =>
                    {
                        _onWindowClosed?.Invoke();
                    });
                }
                else _onWindowClosed?.Invoke();
            });
        }




    }
}
