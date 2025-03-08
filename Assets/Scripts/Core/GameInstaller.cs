using Game.Input;
using Game.Interaction;
using Game.Player;
using Game.Scoring;
using Game.UI;
using UnityEngine;
using Zenject;

namespace Game.Core
{
    /// <summary>
    /// Main installer for game dependencies using Zenject.
    /// Defines the dependency injection container configuration.
    /// </summary>
    public class GameInstaller : MonoInstaller
    {
        [Header("References")] [SerializeField]
        private PlayerSettings _playerSettings;

        [SerializeField] private InteractionSettings _interactionSettings;

        public override void InstallBindings()
        {
            // Bind services as interfaces to implement proper dependency inversion
            BindScoringSystem();
            BindInputSystem();
            BindPlayerSystem();
            BindInteractionSystem();
            BindUISystem();
        }

        private void BindScoringSystem()
        {
            Container.BindInterfacesAndSelfTo<ScoreManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();
        }

        private void BindInputSystem()
        {
            Container.BindInterfacesAndSelfTo<InputService>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();

            Container.Bind<FixedJoystick>()
                .FromComponentInHierarchy()
                .AsSingle();
        }

        private void BindPlayerSystem()
        {
            // Bind the settings as a factory parameter
            Container.BindInstance(_playerSettings)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerController>()
                .FromComponentInHierarchy()
                .AsSingle();
        }

        private void BindInteractionSystem()
        {
            // Bind the settings
            Container.BindInstance(_interactionSettings)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<InteractionService>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();

            Container.Bind<ItemInteraction>()
                .FromComponentInHierarchy()
                .AsSingle();
        }

        private void BindUISystem()
        {
            Container.BindInterfacesAndSelfTo<UIManager>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}