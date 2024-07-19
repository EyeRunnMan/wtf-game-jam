using UnityEngine;

namespace WTF.Common.DebugSystem
{
    public class DebugSystem : MonoBehaviour, IDebugSystem
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        private void Awake()
        {
            DependencySolver.RegisterInstance(this as IDebugSystem);
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            DependencySolver.RemoveInstance(this as IDebugSystem);
        }
    }
}
