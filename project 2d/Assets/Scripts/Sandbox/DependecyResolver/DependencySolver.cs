using System;
using System.Collections.Generic;
using UnityEngine;

namespace WTF.Common
{
    public class DependencySolver : MonoBehaviour
    {
        private static class InstanceContainer<T> where T : class
        {
            public static T Instance;
        }

        private static readonly List<object> ToDispose = new List<object>();
        public static readonly List<string> RegisteredInstances = new List<string>();

        public static void Initialize()
        {
            var go = new GameObject();
            var instance = go.AddComponent<DependencySolver>();
            DontDestroyOnLoad(instance);
            go.name = "[DependencySolver]";
        }
        public static T GetInstance<T>() where T : class => InstanceContainer<T>.Instance;

        public static bool TryGetInstance<T>(out T value) where T : class => (value = InstanceContainer<T>.Instance) != null;

        public static T RegisterInstance<T>(T obj) where T : class
        {
            if (InstanceContainer<T>.Instance != null)
            {
                Debug.LogError($"Failed to register {typeof(T).FullName} instance.");
            }
            else
            {
                InstanceContainer<T>.Instance = obj;
                ToDispose.Add(obj);
                RegisteredInstances.Add(typeof(T).FullName);
            }

            return obj;
        }

        public static void RemoveInstance<T>(T obj = default) where T : class
        {
            if (InstanceContainer<T>.Instance != null)
            {
                RegisteredInstances.Remove(typeof(T).FullName);
                ToDispose.Remove(obj);
                InstanceContainer<T>.Instance = null;
            }
            else
            {
                Debug.LogWarning($"Failed to remove {typeof(T).FullName} instance. Does not exist.");
            }
        }

        private void Awake()
        {
            ToDispose.Clear();
            RegisteredInstances.Clear();
        }

        private void OnDestroy()
        {
            for (var i = ToDispose.Count - 1; i >= 0; i--)
                if (ToDispose[i] is IDisposable disposable)
                    disposable.Dispose();

            RegisteredInstances.Clear();
            ToDispose.Clear();
        }
    }
}