using UnityEngine;
using System;

namespace Game.Input
{
    /// <summary>
    /// Interface for input service to enable testing and decoupling
    /// </summary>
    public interface IInputService
    {
        Vector2 MovementInput { get; }
        Vector2 LookInput { get; }
        event Action<Vector2> OnTouchBegan;
        event Action OnThrowRequested;
        public void RequestThrow();
        bool IsTouchingRightSide { get; }
    }
}