using UnityEngine;
using Zenject;
using Game.Interaction;
using Game.Player;
using Game.Scoring;
using Game.UI;

namespace Game.Core
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ScoreManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            
            // Bind MonoBehaviours from scene
            Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ItemInteraction>().FromComponentInHierarchy().AsSingle();
        }
    }
}