using UnityEngine;
using WTF.Common;
using WTF.Common.InputSystem;
using WTF.PlayerControls;

namespace WTF.Players
{
    public class Creep : MonoBehaviour
    {
        IInputSystem m_inputSystem;
        [SerializeField] private RandomMoveController m_movementController;
        private void Start()
        {
            DependencySolver.TryGetInstance(out m_inputSystem);

            if (m_inputSystem == null)
            {
                Debug.LogWarning("No I/O!!");
                return;
            }

            Debug.LogWarning("YAY");
        }

    }
}
