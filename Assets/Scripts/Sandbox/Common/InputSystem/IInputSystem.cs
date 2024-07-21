using System;
using UnityEngine;

namespace WTF.Common.InputSystem
{
    public interface IInputSystem
    {
        /// <summary>
        /// When the interaction starts
        /// </summary>
        event Action<Vector2> OnSwipeStartEvent;
        /// <summary>
        /// When the interaction is ongoing and the position of the interaction is updated
        /// position is in screen space
        /// </summary>
        event Action<Vector2> OnDuringSwipeEvent;
        /// <summary>
        /// When the interaction ends
        /// </summary>
        event Action OnSwipeEventEnded;
        /// <summary>
        /// When player double taps
        /// </summary>
        event Action<Vector2> OnDoubleTapEvent;

        /// <summary>
        /// Call this when you want to interrupt any ongoing interaction
        /// </summary>
        void InterruptSwipe();
    }
}
