using Game.Input;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [Header("References")]
        [SerializeField] private Transform _cameraTransform;
        
        [Header("Ground Settings")]
        [SerializeField] private float _gravity = 20.0f; // Сила гравитации
        [SerializeField] private float _groundedGravity = 2.0f; // Небольшая гравитация, когда на земле
        [SerializeField] private LayerMask _groundLayers; // Слои, определяющие землю
        [SerializeField] private float _groundCheckDistance = 0.1f; // Расстояние для проверки земли
        
        private IInputService _inputService;
        private PlayerSettings _settings;
        private CharacterController _characterController;
        private float _rotationX = 0f;
        private Vector3 _previousMoveDirection = Vector3.zero;
        
        private float _verticalVelocity;
        private bool _isGrounded;
        
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
            CheckGroundStatus();
            
            ApplyGravity();
            
            HandleLooking();
            HandleMovement();
        }
        
        private void CheckGroundStatus()
        {
            _isGrounded = Physics.CheckSphere(
                transform.position + _characterController.center - new Vector3(0, _characterController.height / 2, 0),
                _groundCheckDistance, 
                _groundLayers);
        }
        
        private void ApplyGravity()
        {
            if (_isGrounded)
            {
                _verticalVelocity = -_groundedGravity;
            }
            else
            {
                _verticalVelocity -= _gravity * Time.deltaTime;
            }
        }
        
        private void HandleMovement()
        {
            Vector2 input = _inputService.MovementInput;
            
            Vector3 moveDirection = Vector3.zero;
            
            if (input.sqrMagnitude > 0.1f)
            {
                moveDirection = transform.right * input.x + transform.forward * input.y;
                
                moveDirection = Vector3.Lerp(_previousMoveDirection, moveDirection, Time.deltaTime * 10f);
                _previousMoveDirection = moveDirection;
            }
            else
            {
                _previousMoveDirection = Vector3.zero;
            }
            
            moveDirection = moveDirection * _settings.MoveSpeed * Time.deltaTime;
            
            moveDirection.y = _verticalVelocity * Time.deltaTime;
            
            _characterController.Move(moveDirection);
        }
        
        private void HandleLooking()
        {
            // Always apply camera rotation if we have any input
            // No need to check IsTouchingRightSide again since InputService handles that
            Vector2 lookInput = _inputService.LookInput;
    
            if (lookInput.sqrMagnitude > 0.001f)
            {
                float lookSensitivity = _settings.LookSensitivity;
        
                // Remove Time.deltaTime scaling since we're already handling timing in InputService
        
                // Vertical rotation (pitch - camera)
                _rotationX -= lookInput.y * lookSensitivity;
                _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);
                _cameraTransform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
        
                // Horizontal rotation (yaw - player)
                transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);
            }
        }
    }
}