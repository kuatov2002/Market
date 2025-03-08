using UnityEngine;
using System;

namespace Game.Scoring
{
    // ScoreManager to handle the game's scoring logic
    public class ScoreManager : MonoBehaviour
    {
        // Event that fires when the score changes
        public event Action<int> OnScoreChanged;
        
        // Current score
        private int _currentScore = 0;
        
        // Property with getter for current score
        public int CurrentScore 
        { 
            get { return _currentScore; }
            private set
            {
                // Only update if the value changed
                if (_currentScore != value)
                {
                    _currentScore = value;
                    // Notify subscribers that score changed
                    OnScoreChanged?.Invoke(_currentScore);
                }
            }
        }
        
        // Method to add points to the score
        public void AddPoint()
        {
            CurrentScore ++;
        }
        
        // Method to reset the score
        public void ResetScore()
        {
            CurrentScore = 0;
        }
    }
}