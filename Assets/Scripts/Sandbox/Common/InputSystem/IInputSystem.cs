using System;
using UnityEngine;

namespace WTF.Common.InputSystem
{
    public interface IInputSystem
    {
        /// <summary>
        /// When the interaction starts
        /// </summary>
        event Action OnInteractionStartEvent;
        /// <summary>
        /// When the interaction is ongoing and the position of the interaction is updated
        /// position is in screen space
        /// </summary>
        event Action<Vector2> OnDuringInteractionEvent;
        /// <summary>
        /// When the interaction ends
        /// </summary>
        event Action OnInteractionEndedEvent;

        /// <summary>
        /// Call this when you want to interrupt any ongoing interaction 
        /// </summary>
        void InterruptInteraction();
    }
}
