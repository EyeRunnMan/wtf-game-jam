using System;
using UnityEngine;

namespace WTF.Common.InputSystem
{
    public interface IInputSystem
    {
        /// <summary>
        /// When the interaction starts
        /// </summary>
        event Action OnSwipeStartEvent;
        /// <summary>
        /// When the interaction is ongoing and the position of the interaction is updated
        /// position is in screen space
        /// </summary>
        event Action<Vector2> OnDuringSwipEvent;
        /// <summary>
        /// When the interaction ends
        /// </summary>
        event Action OnSwipeEventEnded;


        /// <summary>
        /// Call this when you want to interrupt any ongoing interaction 
        /// </summary>
        void InterruptSwipe();

        event Action<Vector2> OnDoubleTapEvent;
    }
}
