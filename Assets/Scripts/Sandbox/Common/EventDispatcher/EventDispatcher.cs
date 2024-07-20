using System;
using System.Collections.Generic;
using UnityEngine;

namespace WTF.Events
{
    public static class EventDispatcher<T>
    {
        private static Dictionary<CustomEvents, Action<T>> eventListeners = new Dictionary<CustomEvents, Action<T>>();

        public static void Register(CustomEvents eventName, Action<T> listener)
        {
            if (!eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName] = listener;
                return;
            }

            eventListeners[eventName] += listener;
        }

        public static void Unregister(CustomEvents eventName, Action<T> listener)
        {
            if (!eventListeners.ContainsKey(eventName))
            {
                Debug.LogWarning("Unregistering for a event that is not registered: " + eventName);
                return;
            }

            eventListeners[eventName] -= listener;
        }

        public static void Dispatch(CustomEvents eventName, T data)
        {
            if (!eventListeners.ContainsKey(eventName))
            {
                Debug.LogWarning("Firing a event that is not registered: " + eventName);
                return;
            }

            eventListeners[eventName](data);
        }
    }
}
