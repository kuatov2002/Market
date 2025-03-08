using UnityEngine;
using UnityEngine.UI;

public class ItemInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private float throwForce = 10f;
    
    [Header("UI References")]
    [SerializeField] private Button throwButton;
    
    private Camera playerCamera;
    private GameObject heldItem;
    private Rigidbody heldItemRb;
    
    private void Awake()
    {
        playerCamera = Camera.main;
        
        // Initially hide throw button
        if (throwButton != null)
        {
            throwButton.gameObject.SetActive(false);
            throwButton.onClick.AddListener(ThrowItem);
        }
    }
    
    private void Update()
    {
        // Check for item pickup
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayers))
            {
                if (hit.collider.CompareTag("Pickup") && heldItem == null)
                {
                    PickupItem(hit.collider.gameObject);
                }
            }
        }
    }
    
    private void PickupItem(GameObject item)
    {
        // Store reference to item
        heldItem = Instantiate(item);
        heldItemRb = heldItem.GetComponent<Rigidbody>();
        heldItem.SetActive(false);
        Destroy(item);
        
        // Show throw button
        if (throwButton != null)
        {
            throwButton.gameObject.SetActive(true);
        }
    }
    
    public void ThrowItem()
    {
        if (heldItem != null)
        {
            heldItem.SetActive(true);
            heldItem.transform.position = playerCamera.transform.position;
            // Re-enable physics
            heldItemRb.isKinematic = false;
            heldItemRb.useGravity = true;
            
            // Re-enable collider
            Collider itemCollider = heldItem.GetComponent<Collider>();
            if (itemCollider != null)
            {
                itemCollider.enabled = true;
            }
            
            // Remove parent relationship
            heldItem.transform.SetParent(null);
            
            // Apply force
            heldItemRb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse);
            
            // Clear reference
            heldItem = null;
            heldItemRb = null;
            
            // Hide throw button
            if (throwButton != null)
            {
                throwButton.gameObject.SetActive(false);
            }
        }
    }
}