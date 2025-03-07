using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Header("References")]
    [SerializeField] private PlayerController playerControllerPrefab;
    [SerializeField] private UIManager uiManagerPrefab;
    [SerializeField] private ItemInteraction itemInteractionPrefab;
    
    public override void InstallBindings()
    {
        // Bind services as singletons
        Container.Bind<GameManager>().AsSingle();
        
        // Bind MonoBehaviours from scene
        Container.Bind<PlayerController>().FromComponentInNewPrefab(playerControllerPrefab).AsSingle();
        Container.Bind<UIManager>().FromComponentInNewPrefab(uiManagerPrefab).AsSingle();
        Container.Bind<ItemInteraction>().FromComponentInNewPrefab(itemInteractionPrefab).AsSingle();
    }
}