using System;
using UnityEngine;
using Zenject;

namespace Game.Input 
{
    /// <summary>
    /// Handles all input for the game, decoupling input logic from game logic
    /// </summary>
    public class InputService : MonoBehaviour, IInputService
    {
        private FixedJoystick _moveJoystick;
        private Vector2 _lookInput;
        private bool _isTouchingRightSide;
        private Vector2 _previousTouchPosition;
        private bool _wasRightSideTouched = false;
        
        // Camera sensitivity and smoothing settings
        [SerializeField] private float _touchSensitivityMultiplier = 12f;
        private Vector2 _lookInputVelocity;
        private Vector2 _smoothedLookInput;
        
        public Vector2 MovementInput => _moveJoystick != null
            ? new Vector2(_moveJoystick.Horizontal, _moveJoystick.Vertical)
            : Vector2.zero;
            
        public Vector2 LookInput => _smoothedLookInput;
        public bool IsTouchingRightSide => _isTouchingRightSide;
        
        public event Action<Vector2> OnTouchBegan;
        public event Action OnThrowRequested;
        
        [Inject]
        public void Construct(FixedJoystick moveJoystick)
        {
            _moveJoystick = moveJoystick;
        }
        
        private void Update()
        {
            ProcessTouchInput();
            SmoothLookInput();
        }
        
        private void SmoothLookInput()
        {
            // Apply smoothing to the raw look input
            _smoothedLookInput = Vector2.SmoothDamp(
                _smoothedLookInput, 
                _lookInput, 
                ref _lookInputVelocity, 
                0);
                
            // Gradually reduce look input when not actively touching
            if (!_isTouchingRightSide)
            {
                _lookInput = Vector2.Lerp(_lookInput, Vector2.zero, Time.deltaTime * 10f);
            }
        }
        
        private void ProcessTouchInput()
        {
            if (UnityEngine.Input.touchCount == 0) 
            {
                // Reset states when no touches are present
                _isTouchingRightSide = false;
                _wasRightSideTouched = false;
                return;
            }
            
            Touch touch = UnityEngine.Input.GetTouch(0);
            
            // Check if touch is on right side of screen
            bool isRightSide = touch.position.x > Screen.width * 0.5f;
            
            // Handle touch begin
            if (touch.phase == TouchPhase.Began)
            {
                _previousTouchPosition = touch.position;
                _wasRightSideTouched = isRightSide;
                OnTouchBegan?.Invoke(touch.position);
                
                if (isRightSide)
                {
                    _isTouchingRightSide = true;
                }
            }
            // Handle continued touch
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) 
            {
                // Only update camera if the touch started on the right side
                // This prevents the camera from jumping when moving from left to right
                if (_wasRightSideTouched)
                {
                    _isTouchingRightSide = true;
                    
                    if (touch.phase == TouchPhase.Moved)
                    {
                        // Calculate delta in pixels
                        Vector2 touchDelta = touch.position - _previousTouchPosition;
                        
                        // Scale delta by screen dimensions for consistent feel across devices
                        float screenFactor = 1f / Mathf.Min(Screen.width, Screen.height);
                        touchDelta *= screenFactor * _touchSensitivityMultiplier;
                        
                        // Apply sensitivity curve for more precise small movements
                        float magnitude = touchDelta.magnitude;
                        if (magnitude > 0)
                        {
                            // Apply non-linear scaling - small movements become more precise
                            // while larger swipes still allow for quick turns
                            float scaledMagnitude = Mathf.Pow(magnitude * 10f, 1.5f) * 0.1f;
                            touchDelta = touchDelta.normalized * scaledMagnitude;
                        }
                        
                        _lookInput = new Vector2(touchDelta.x, touchDelta.y);
                        
                        // Store current position for next frame
                        _previousTouchPosition = touch.position;
                    }
                }
            }
            // Handle touch end
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // We don't immediately reset _lookInput here, allowing for smooth deceleration
                _isTouchingRightSide = false;
                _wasRightSideTouched = false;
            }
        }
        
    }
}