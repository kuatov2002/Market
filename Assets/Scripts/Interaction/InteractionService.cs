using System;
using Game.Input;
using Game.Player;
using UnityEngine;
using Zenject;

namespace Game.Interaction
{
    /// <summary>
    /// Service that manages all interactions in the game
    /// </summary>
    public class InteractionService : MonoBehaviour, IInteractionService
    {
        private IInputService _inputService;
        private InteractionSettings _settings;
        private Camera _playerCamera;

        private GameObject _heldItem;
        private Rigidbody _heldItemRb;

        public bool HasHeldItem => _heldItem != null;

        public event Action OnItemPickedUp;
        public event Action OnItemThrown;

        [Inject]
        public void Construct(
            IInputService inputService,
            IPlayerController playerController,
            InteractionSettings settings)
        {
            _inputService = inputService;
            _settings = settings;

            // Subscribe to input events
            _inputService.OnTouchBegan += HandleTouchBegan;
            _inputService.OnThrowRequested += ThrowItem;
        }

        private void Awake()
        {
            _playerCamera = Camera.main;
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (_inputService != null)
            {
                _inputService.OnTouchBegan -= HandleTouchBegan;
                _inputService.OnThrowRequested -= ThrowItem;
            }
        }

        private void HandleTouchBegan(Vector2 touchPosition)
        {
            if (_heldItem != null) return;

            Ray ray = _playerCamera.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, _settings.InteractionDistance, _settings.InteractableLayers))
            {
                if (hit.collider.CompareTag("Pickup"))
                {
                    PickupItem(hit.collider.gameObject);
                }
            }
        }

        private void PickupItem(GameObject item)
        {
            if (item == null) return;

            // Create a copy of the item
            _heldItem = Instantiate(item);
            _heldItemRb = _heldItem.GetComponent<Rigidbody>();

            if (_heldItemRb == null)
            {
                Debug.LogError("Picked up item has no Rigidbody component!");
                Destroy(_heldItem);
                _heldItem = null;
                return;
            }

            // Hide the item until thrown
            _heldItem.SetActive(false);

            // Destroy the original
            Destroy(item);

            // Notify listeners
            OnItemPickedUp?.Invoke();
        }

        public void ThrowItem()
        {
            if (_heldItem == null) return;

            // Position the item at the camera position
            _heldItem.SetActive(true);
            _heldItem.transform.position = _playerCamera.transform.position;

            // Re-enable physics
            _heldItemRb.isKinematic = false;
            _heldItemRb.useGravity = true;

            // Re-enable collider
            Collider itemCollider = _heldItem.GetComponent<Collider>();
            if (itemCollider != null)
            {
                itemCollider.enabled = true;
            }

            // Remove parent relationship
            _heldItem.transform.SetParent(null);

            // Apply force in camera forward direction
            _heldItemRb.AddForce(_playerCamera.transform.forward * _settings.ThrowForce, ForceMode.Impulse);
            
            _heldItem = null;
            _heldItemRb = null;

            // Notify listeners
            OnItemThrown?.Invoke();
        }
    }
}