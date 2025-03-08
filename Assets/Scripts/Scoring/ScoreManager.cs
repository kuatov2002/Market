using System;
using UnityEngine;

namespace Game.Scoring
{
    /// <summary>
    /// Manages the game's scoring system
    /// </summary>
    public class ScoreManager : MonoBehaviour, IScoreManager
    {
        private int _currentScore = 0;

        public int CurrentScore => _currentScore;

        public event Action<int> OnScoreChanged;

        public void AddPoint()
        {
            _currentScore++;
            OnScoreChanged?.Invoke(_currentScore);
        }
    }
}