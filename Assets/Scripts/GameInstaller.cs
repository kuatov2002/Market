using UnityEngine;
using Zenject;
using Game.Scoring;
using Game.UI;

namespace Game.Core
{
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
            Container.Bind<ScoreManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            
            // Bind input provider
            Container.Bind<IInputProvider>().To<MobileInputProvider>().AsSingle();
            
            // Bind MonoBehaviours from scene
            Container.Bind<PlayerController>().FromComponentInNewPrefab(playerControllerPrefab).AsSingle();
            Container.Bind<UIManager>().FromComponentInNewPrefab(uiManagerPrefab).AsSingle();
            Container.Bind<ItemInteraction>().FromComponentInNewPrefab(itemInteractionPrefab).AsSingle();
        }
    }
}