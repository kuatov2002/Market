using Game.Input;
using UnityEngine;
using Zenject;

namespace Game.Interaction
{
    /// <summary>
    /// Component that handles the interaction UI and proxies to the interaction service
    /// </summary>
    public class ItemInteraction : MonoBehaviour
    {
        [Header("UI References")] [SerializeField]
        private UnityEngine.UI.Button _throwButton;

        private IInteractionService _interactionService;
        private IInputService _inputService;

        [Inject]
        public void Construct(IInteractionService interactionService, IInputService inputService)
        {
            _interactionService = interactionService;
            _inputService = inputService;

            // Subscribe to interaction events
            _interactionService.OnItemPickedUp += HandleItemPickedUp;
            _interactionService.OnItemThrown += HandleItemThrown;
        }

        private void Start()
        {
            // Hide throw button initially
            if (_throwButton != null)
            {
                _throwButton.gameObject.SetActive(false);
                _throwButton.onClick.AddListener(OnThrowButtonClick);
            }
            else
            {
                Debug.LogError("Throw button not assigned in ItemInteraction!");
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (_interactionService != null)
            {
                _interactionService.OnItemPickedUp -= HandleItemPickedUp;
                _interactionService.OnItemThrown -= HandleItemThrown;
            }

            // Remove button listener
            if (_throwButton != null)
            {
                _throwButton.onClick.RemoveListener(OnThrowButtonClick);
            }
        }

        private void HandleItemPickedUp()
        {
            if (_throwButton != null)
            {
                _throwButton.gameObject.SetActive(true);
            }
        }

        private void HandleItemThrown()
        {
            if (_throwButton != null)
            {
                _throwButton.gameObject.SetActive(false);
            }
        }

        private void OnThrowButtonClick()
        {
            _inputService.RequestThrow();
        }
    }
}