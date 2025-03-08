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
        private IInteractionService _interactionService;

        [Inject]
        public void Construct(IScoreManager scoreManager, IInteractionService interactionService)
        {
            _scoreManager = scoreManager;
            _interactionService = interactionService;

            // Subscribe
            _scoreManager.OnScoreChanged += UpdateScoreDisplay;
            _interactionService.OnItemPickedUp += ShowThrowButton;
            _interactionService.OnItemThrown += HideThrowButton;
            // Setup throw button
            if (_throwButton != null)
            {
                _throwButton.onClick.AddListener(() => _interactionService.ThrowItem());
                _throwButton.gameObject.SetActive(false);
            }
            

        }

        private void ShowThrowButton()
        {
            _throwButton.gameObject.SetActive(true);
        }
        private void HideThrowButton()
        {
            _throwButton.gameObject.SetActive(false);
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
            if (_interactionService != null)
            {
                _interactionService.OnItemPickedUp -= ShowThrowButton;
                _interactionService.OnItemThrown -= HideThrowButton;
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