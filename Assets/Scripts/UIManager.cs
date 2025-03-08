using Game.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Game.Scoring;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button throwButton;
        [SerializeField] private Text scoreText;
        
        private ScoreManager _scoreManager;
        
        [Inject]
        public void Construct(ScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
            
            // Subscribe to score changes
            if (_scoreManager != null)
            {
                _scoreManager.OnScoreChanged += UpdateScoreDisplay;
            }
        }
        
        private void Start()
        {
            // Ensure throw button is hidden initially
            if (throwButton != null)
            {
                throwButton.gameObject.SetActive(false);
            }
            
            // Initialize score display
            UpdateScoreDisplay(_scoreManager.CurrentScore);
        }
        
        private void Update()
        {
            // Prevent character from rotating when interacting with UI
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
            }
        }
        
        // Method to update the score display
        private void UpdateScoreDisplay(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = $"Очки: {score}";
            }
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events when destroyed
            if (_scoreManager != null)
            {
                _scoreManager.OnScoreChanged -= UpdateScoreDisplay;
            }
        }
    }
}