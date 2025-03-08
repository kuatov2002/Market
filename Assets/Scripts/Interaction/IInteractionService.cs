using System;

namespace Game.Interaction
{
    /// <summary>
    /// Interface for interaction service to allow for testing
    /// </summary>
    public interface IInteractionService
    {
        public void ThrowItem();
        event Action OnItemPickedUp;
        event Action OnItemThrown;
        bool HasHeldItem { get; }
    }
}