using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Button throwButton;
    [SerializeField] private Text statusText;
    
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ItemInteraction itemInteraction;
    
    private void Start()
    {
        // Ensure throw button is hidden initially
        if (throwButton != null)
        {
            throwButton.gameObject.SetActive(false);
        }
        
        // Set initial status text
        if (statusText != null)
        {
            statusText.text = "Соберите фрукты и погрузите их в грузовик";
        }
    }
    
    private void Update()
    {
        // Prevent character from rotating when interacting with UI
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            bool touchingUI = IsPointerOverUIElement(touch.position);
            
            if (playerController != null)
            {
                playerController.SetLookEnabled(!touchingUI);
            }
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