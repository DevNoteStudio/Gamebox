using System;
using UnityEngine;


namespace Gamebox
{
    public class ScoreController
    {
        public event Action OnScoreChanged;
        public event Action OnStarScoreChanged;

        public delegate int GetLevelStarsHandler();
        private GetLevelStarsHandler _getLevelStarsHandler; 


        public int CurrentScore { get; private set; } = 0;
        public int RequiredScore { get; private set; } = 1;


        public int CurrentStarScore { get; private set; }
        public int RequiredStarScore { get; private set; }


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

        public void SetStarScoreRequire(int require)
        {

        }

        public void SetScoreCompletedLevelStars(GetLevelStarsHandler handler) 
            => _getLevelStarsHandler = handler;

        public void AddScore(int score)
        {
            if (!levelController.IsLevelPlaying) return;

            CurrentScore = Mathf.Clamp(CurrentScore + score, 0, RequiredScore);
            OnScoreChanged?.Invoke();

            if (CurrentScore == RequiredScore && _getLevelStarsHandler != null)
                levelController.CompleteCurrentLevel(_getLevelStarsHandler.Invoke());

        }

        public void AddParticleScore(int score, int particles, Vector3 fromWorldPosition)
        {
            if (!levelController.IsLevelPlaying) return;
            levelController.GameplayScreen.ScoreView.AnimateParticleScore(score, particles, fromWorldPosition);
        }





    }
}
