using UnityEngine;
using Zenject;

// Interface for getting input
public interface IInputProvider
{
    Vector2 GetMovementInput();
    Vector2 GetLookInput();
    bool GetInteractionInputDown();
}

// Implementation for mobile
public class MobileInputProvider : IInputProvider
{
    private Vector2 _movementInput;
    private Vector2 _lookInput;
    private bool _interactionInput;
    
    // Called by the joystick component
    public void SetMovementInput(Vector2 input)
    {
        _movementInput = input;
    }
    
    // Called by the look input handler
    public void SetLookInput(Vector2 input)
    {
        _lookInput = input;
    }
    
    // Called when interaction happens
    public void SetInteractionInput(bool input)
    {
        _interactionInput = input;
    }
    
    public Vector2 GetMovementInput()
    {
        return _movementInput;
    }
    
    public Vector2 GetLookInput()
    {
        return _lookInput;
    }
    
    public bool GetInteractionInputDown()
    {
        bool value = _interactionInput;
        _interactionInput = false; // Reset after reading
        return value;
    }
}

// Manager that coordinates input
public class InputManager : MonoBehaviour
{
    [Inject] private MobileInputProvider _inputProvider;
    
    [SerializeField] private Joystick joystick; // Reference to the joystick UI component
    
    private int _touchId = -1;
    private Vector2 _previousTouchPosition;
    
    private void Update()
    {
        // Update joystick input
        if (joystick != null)
        {
            _inputProvider.SetMovementInput(new Vector2(joystick.Horizontal, joystick.Vertical));
        }
        
        // Process touch input for looking and interaction
        ProcessTouchInput();
    }
    
    private void ProcessTouchInput()
    {
        // Process all touches
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            
            // Ignore touches that are over UI elements (like joystick)
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                continue;
                
            // Handle look input
            if (_touchId == -1 && touch.phase == TouchPhase.Began)
            {
                _touchId = touch.fingerId;
                _previousTouchPosition = touch.position;
            }
            else if (touch.fingerId == _touchId)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 delta = touch.position - _previousTouchPosition;
                    _inputProvider.SetLookInput(delta * 0.1f); // Scale down the input
                    _previousTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    _touchId = -1;
                    _inputProvider.SetLookInput(Vector2.zero);
                }
            }
        }
    }
}