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
        
        private PlayerController _playerController;
        private ScoreManager _scoreManager;
        
        [Inject]
        public void Construct(PlayerController playerController, ScoreManager scoreManager)
        {
            _playerController = playerController;
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
                bool touchingUI = IsPointerOverUIElement(touch.position);
                
                if (_playerController != null)
                {
                    _playerController.SetLookEnabled(!touchingUI);
                }
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
        
        private bool IsPointerOverUIElement(Vector2 position)
        {
            if (UnityEngine.EventSystems.EventSystem.current == null)
                return false;
                
            UnityEngine.EventSystems.PointerEventData eventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            eventData.position = position;
            
            System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult> results = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, results);
            
            return results.Count > 0;
        }
    }
}