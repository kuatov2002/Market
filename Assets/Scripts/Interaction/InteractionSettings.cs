using UnityEngine;

namespace Game.Interaction
{
    /// <summary>
    /// Settings for the interaction system
    /// </summary>
    [CreateAssetMenu(fileName = "InteractionSettings", menuName = "Game/Interaction Settings")]
    public class InteractionSettings : ScriptableObject
    {
        [Header("Interaction Settings")] [SerializeField]
        private float _interactionDistance = 2f;

        [SerializeField] private LayerMask _interactableLayers;
        [SerializeField] private float _throwForce = 10f;

        public float InteractionDistance => _interactionDistance;
        public LayerMask InteractableLayers => _interactableLayers;
        public float ThrowForce => _throwForce;
    }
}