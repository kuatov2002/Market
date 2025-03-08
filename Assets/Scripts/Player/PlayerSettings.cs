using UnityEngine;

namespace Game.Player
{
    /// <summary>
    /// Settings for player movement and controls
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Game/Player Settings")]
    public class PlayerSettings : ScriptableObject
    {
        [Header("Movement Settings")] [SerializeField]
        private float _moveSpeed = 5f;

        [SerializeField] private float _lookSensitivity = 2f;

        public float MoveSpeed => _moveSpeed;
        public float LookSensitivity => _lookSensitivity;
    }
}