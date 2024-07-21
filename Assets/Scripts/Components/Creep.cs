using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using WTF.PlayerControls;
using WTF.Configs;
using WTF.Common;
using WTF.Common.InputSystem;
using WTF.Events;
using JetBrains.Annotations;

namespace WTF.Players
{
    public class Creep : MonoBehaviour
    {
        [SerializeField] private CreepTypes m_type;
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private CreepMovementController m_movementController;
        [SerializeField] private SpriteRenderer m_charSpriteRenderer;
        [SerializeField] private float[] m_scalingRanges;
        [SerializeField] private Sprite[] m_charSprites;
        [SerializeField] private Transform m_mergedCreepsParent;

        private bool m_isSelected;
        private int m_creepCount = 1;
        private InputSystem m_inputSystem;

        private void OnEnable()
        {
            m_inputSystem = InputSystem.GetInstance();

            m_inputSystem.OnSwipeStartEvent += OnSwipeStart;
            m_inputSystem.OnDuringSwipeEvent += OnDuringSwipe;
            m_inputSystem.OnSwipeEventEnded += OnSwipeEnd;
        }

        private void OnDisable()
        {
            if (m_inputSystem == null)
            {
                return;
            }

            m_inputSystem.OnSwipeStartEvent -= OnSwipeStart;
            m_inputSystem.OnDuringSwipeEvent -= OnDuringSwipe;
            m_inputSystem.OnSwipeEventEnded -= OnSwipeEnd;
        }

        private void OnSwipeStart(Vector2 startPos)
        {
            if (!IsPointInObject(startPos) || m_isSelected || m_creepCount > 1)
            {
                return;
            }

            m_isSelected = true;
            m_movementController.isSelected = true;
            // Play Highlight Anim
            EventDispatcher<Creep>.Dispatch(CustomEvents.CreepSelected, this);
        }

        private void OnDuringSwipe(Vector2 movePos)
        {
            if (IsPointInObject(movePos) && !m_isSelected && m_creepCount == 1)
            {
                m_isSelected = true;
                m_movementController.isSelected = true;
                // Play Highlight Anim
                EventDispatcher<Creep>.Dispatch(CustomEvents.CreepSelected, this);
                return;
            }

            // DO Something
        }

        private void OnSwipeEnd()
        {
            if (!m_isSelected)
            {
                return;
            }

            // // Do Something
            // m_isSelected = false;
            // m_movementController.isSelected = false;
        }

        private bool IsPointInObject(Vector2 position)
        {
            Vector3 spriteMin = m_spriteRenderer.bounds.min;
            Vector3 spriteMax = m_spriteRenderer.bounds.max;

            return (position.x >= spriteMin.x && position.x <= spriteMax.x &&
                    position.y >= spriteMin.y && position.y <= spriteMax.y);
        }

        private void UpdateSprite()
        {
            if (m_creepCount < 1)
            {
                m_creepCount = 1;
            }
            else if (m_creepCount >= m_charSprites.Length)
            {
                m_creepCount = m_charSprites.Length;
            }

            m_charSpriteRenderer.sprite = m_charSprites[m_creepCount - 1];
            m_spriteRenderer.transform.localScale = new Vector3(m_scalingRanges[m_creepCount - 1], m_scalingRanges[m_creepCount - 1], 1);
        }

        public CreepTypes creepType
        {
            get { return m_type; }
        }

        public Transform mergedCreepsParent
        {
            get { return m_mergedCreepsParent; }
        }

        public void DeselectCreep(bool skipAnimation = false)
        {
            m_isSelected = false;
            m_movementController.isSelected = false;
            // Play Highlight anim in reverse
            EventDispatcher<Creep>.Dispatch(CustomEvents.CreepUnselected, this);
        }

        public async Task NavigateAndHide(Creep dest)
        {
            await m_movementController.NavigateToDestination(dest.transform.position);
            transform.parent = dest.mergedCreepsParent;
            gameObject.SetActive(false);
        }
        public bool IsOnNavMesh()
        {
            return m_movementController.IsOnNavMesh();
        }
        public void DoMerge(int creepCount)
        {
            m_creepCount = creepCount;
            UpdateSprite();
            // Swap sprite and play merge anim
        }
    }
}
