using System;

namespace Game.Interaction
{
    /// <summary>
    /// Interface for interaction service to allow for testing
    /// </summary>
    public interface IInteractionService
    {
        event Action OnItemPickedUp;
        event Action OnItemThrown;
        bool HasHeldItem { get; }
    }
}