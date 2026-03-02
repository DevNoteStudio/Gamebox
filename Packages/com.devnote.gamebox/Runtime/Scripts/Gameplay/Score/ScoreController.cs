using System;
using UnityEngine;


namespace Gamebox
{
    public class ScoreController
    {
        public event Action OnScoreChanged;
        public event Action OnScoreCompleted;

        public delegate int GetLevelStarsHandler();
        private GetLevelStarsHandler _getLevelStarsHandler; 


        public int CurrentScore { get; private set; } = 0;
        public int RequiredScore { get; private set; } = 1;


        private readonly LevelController levelController;


        public ScoreController(LevelController levelController)
        {
            this.levelController = levelController;
        }


        public void SetScoreRequire(int require)
        {
            CurrentScore = 0;
            RequiredScore = require;
            OnScoreChanged?.Invoke();
        }

        public void SetScoreCompletedLevelStars(GetLevelStarsHandler handler) 
            => _getLevelStarsHandler = handler;

        public void AddScore(int score)
        {
            if (!levelController.IsLevelPlaying) return;

            int previousScore = CurrentScore;
            CurrentScore = Mathf.Clamp(CurrentScore + score, 0, RequiredScore);

            if (previousScore != CurrentScore)
            {
                OnScoreChanged?.Invoke();

                if (CurrentScore == RequiredScore)
                {
                    if (_getLevelStarsHandler != null)
                        levelController.CompleteCurrentLevel(_getLevelStarsHandler.Invoke());

                    OnScoreCompleted?.Invoke();
                }
            }
        }

        public void AddParticleScore(int score, int particles, Vector3 fromWorldPosition)
        {
            if (!levelController.IsLevelPlaying) return;
            levelController.GameplayScreen.ScoreView.AnimateParticleScore(score, particles, fromWorldPosition);
        }





    }
}
