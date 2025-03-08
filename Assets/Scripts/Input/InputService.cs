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
        
        public Vector2 MovementInput => _moveJoystick != null
            ? new Vector2(_moveJoystick.Horizontal, _moveJoystick.Vertical)
            : Vector2.zero;
            
        public Vector2 LookInput => _lookInput;
        public bool IsTouchingRightSide => _isTouchingRightSide;
        
        // Changed to match interface: Action<Vector2> instead of Action
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
        }
        
        private void ProcessTouchInput()
        {
            if (UnityEngine.Input.touchCount == 0) 
            {
                // Reset look input and touch state when no touches are present
                _lookInput = Vector2.zero;
                _isTouchingRightSide = false;
                return;
            }
            
            Touch touch = UnityEngine.Input.GetTouch(0);
            
            // Check if touch is on right side of screen (not on joystick)
            bool isRightSide = touch.position.x > Screen.width * 0.5f;
            
            // Only update _isTouchingRightSide when touch is active
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) 
            {
                _isTouchingRightSide = isRightSide;
            }
            
            // Only apply look input when the touch is actively moving on the right side
            if (isRightSide && touch.phase == TouchPhase.Moved)
            {
                _lookInput = new Vector2(touch.deltaPosition.x, -touch.deltaPosition.y);
            }
            // Reset look input when touch is ending or canceled
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                _lookInput = Vector2.zero;
            }
            
            if (touch.phase == TouchPhase.Began)
            {
                // Modified to pass the touch position to the event
                OnTouchBegan?.Invoke(touch.position);
            }
        }
        
        public void RequestThrow()
        {
            OnThrowRequested?.Invoke();
        }
    }
}