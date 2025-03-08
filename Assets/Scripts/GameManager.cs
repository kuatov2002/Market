using Game.UI;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    private UIManager uiManager;
    private ItemInteraction itemInteraction;
    
    [Inject]
    public void Construct(PlayerController playerController, UIManager uiManager, ItemInteraction itemInteraction)
    {
        this.playerController = playerController;
        this.uiManager = uiManager;
        this.itemInteraction = itemInteraction;
    }
    
    private void Start()
    {
        // Initialize game state or set up references between components
        Debug.Log("Game initialized with Zenject");
    }
}