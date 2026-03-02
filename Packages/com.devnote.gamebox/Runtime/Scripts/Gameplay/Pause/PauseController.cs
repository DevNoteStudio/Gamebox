using System;
using DevNote;
using UnityEngine;

namespace Gamebox
{
    public class PauseController
    {
        public event Action OnGameplayPaused;
        public event Action OnGameplayResumed;

        public bool IsGameplayPaused => _pausePoints > 0;

        private int _pausePoints = 0;

        private readonly Viewer<PauseWindowView> pauseWindowViewer;
        private readonly IEnvironment environment;


        public PauseController(IEnvironment environment)
        {
            pauseWindowViewer = new(IConfigs.GetViewPrefab<PauseWindowView>());
            this.environment = environment;
        }

        public void AddPauseGameplay()
        {
            _pausePoints++;

            if (_pausePoints == 1)
            {
                environment.StopGameplay();
                OnGameplayPaused?.Invoke();
            }
        }

        public void RemovePauseGameplay()
        {
            _pausePoints = Mathf.Max(_pausePoints - 1, 0);

            if (_pausePoints == 0)
            {
                environment.StartGameplay();
                OnGameplayResumed?.Invoke();
            }
                
        }


        public void ShowPauseWindow()
        {
            
            pauseWindowViewer.ShowFaded(UI.Container).Display().AnimateShow();
        }


        public void HidePauseWindow(bool force = false)
        {
            if (force) pauseWindowViewer.ForceFadedHide();
            else pauseWindowViewer.AnimateFadedHide(pauseWindowViewer.View.AnimateHide);
        }




    }
}


