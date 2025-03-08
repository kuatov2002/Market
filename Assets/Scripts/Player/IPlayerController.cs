using UnityEngine;

namespace Game.Player
{
    /// <summary>
    /// Interface for the player controller to allow for testing
    /// </summary>
    public interface IPlayerController
    {
        Transform CameraTransform { get; }
    }
}