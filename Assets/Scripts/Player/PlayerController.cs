using Game.Input;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    /// <summary>
    /// Handles player movement and camera control
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [Header("References")]
        [SerializeField] private Transform _cameraTransform;
        
        private IInputService _inputService;
        private PlayerSettings _settings;
        private CharacterController _characterController;
        private float _rotationX = 0f;
        
        public Transform CameraTransform => _cameraTransform;
        
        [Inject]
        public void Construct(IInputService inputService, PlayerSettings settings)
        {
            _inputService = inputService;
            _settings = settings;
        }
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            
            if (_cameraTransform == null)
            {
                Debug.LogError("Camera transform not assigned to PlayerController!");
                _cameraTransform = Camera.main.transform;
            }
        }
        
        private void Update()
        {
            HandleMovement();
            HandleLooking();
        }
        
        private void HandleMovement()
        {
            Vector2 input = _inputService.MovementInput;
            
            if (input.sqrMagnitude > 0.1f)
            {
                Vector3 moveDirection = transform.right * input.x + transform.forward * input.y;
                _characterController.Move(moveDirection * _settings.MoveSpeed * Time.deltaTime);
            }
        }
        
        private void HandleLooking()
        {
            if (_inputService.IsTouchingRightSide)
            {
                Vector2 lookInput = _inputService.LookInput;
                float lookSensitivity = _settings.LookSensitivity;
                
                // Vertical rotation (pitch - camera)
                _rotationX += lookInput.y * lookSensitivity;
                _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);
                _cameraTransform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
                
                // Horizontal rotation (yaw - player)
                transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);
            }
        }
    }
    
}