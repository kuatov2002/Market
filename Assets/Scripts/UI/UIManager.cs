using Game.Interaction;
using Game.Scoring;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    /// <summary>
    /// Manages all UI elements and displays
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("UI References")] [SerializeField]
        private Button _throwButton;

        [SerializeField] private Text _scoreText;

        private IScoreManager _scoreManager;

        [Inject]
        public void Construct(IScoreManager scoreManager)
        {
            _scoreManager = scoreManager;

            // Subscribe to score changes
            _scoreManager.OnScoreChanged += UpdateScoreDisplay;
        }

        private void Start()
        {
            // Initialize score display
            UpdateScoreDisplay(_scoreManager.CurrentScore);

            // Ensure throw button is properly configured
            if (_throwButton == null)
            {
                Debug.LogError("Throw button not assigned in UIManager!");
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (_scoreManager != null)
            {
                _scoreManager.OnScoreChanged -= UpdateScoreDisplay;
            }
        }

        private void UpdateScoreDisplay(int score)
        {
            if (_scoreText != null)
            {
                _scoreText.text = $"Очки: {score}";
            }
        }
    }
}