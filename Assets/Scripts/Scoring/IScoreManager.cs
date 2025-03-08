using System;

namespace Game.Scoring
{
    /// <summary>
    /// Interface for score manager to allow for testing
    /// </summary>
    public interface IScoreManager
    {
        int CurrentScore { get; }
        event Action<int> OnScoreChanged;
        void AddPoint();
    }
}