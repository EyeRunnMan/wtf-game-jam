using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using WTF.Configs;
using WTF.Common;
using WTF.Common.InputSystem;
using WTF.Events;
using WTF.Models;
using WTF.PlayerControls;

namespace WTF.Players
{
    public class Creep : MonoBehaviour
    {
        [SerializeField] private CreepTypes m_type;
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private CreepMovementController m_movementController;
        [SerializeField] private float[] m_scalingRanges;
        [SerializeField] private CreepSpriteHandler m_spriteHandler;
        [SerializeField] private CreepExplosion m_explosionHandler;
        [SerializeField] private Transform m_mergedCreepsParent;

        public bool m_isSelected;
        private int m_creepCount = 1;
        private InputSystem m_inputSystem;

        private void OnEnable()
        {
            m_inputSystem = InputSystem.GetInstance();

            m_inputSystem.OnSwipeStartEvent += OnSwipeStart;
            m_inputSystem.OnDuringSwipeEvent += OnDuringSwipe;
            m_inputSystem.OnSwipeEventEnded += OnSwipeEnd;
            m_inputSystem.OnDoubleTapEvent += OnDoubleTap;
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
            m_inputSystem.OnDoubleTapEvent -= OnDoubleTap;
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

            // Do Something
//             m_isSelected = false;
//             m_movementController.isSelected = false;
        }

        private void OnDoubleTap(Vector2[] tapPos)
        {
            if (!IsPointInObject(tapPos[0]) || !IsPointInObject(tapPos[1]) || m_creepCount <= 1)
            {
                if (m_isSelected)
                {
                    DeselectCreep(true);
                }
                return;
            }

            InitiateExplosion(m_type);
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
            m_spriteHandler.UpdateSprite(m_type, m_creepCount - 1);
            m_spriteRenderer.transform.localScale = new Vector3(m_scalingRanges[m_creepCount - 1], m_scalingRanges[m_creepCount - 1], 1);
        }

        public CreepTypes creepType
        {
            get { return m_type; }
        }

        public int creepCount
        {
            get { return m_creepCount; }
        }

        public Transform mergedCreepsParent
        {
            get { return m_mergedCreepsParent; }
        }

        public CreepSpriteHandler spriteHandler
        {
            get { return m_spriteHandler; }
        }

        public CreepExplosion explosionHandler
        {
            get { return m_explosionHandler; }
        }

        public void InitiateExplosion(CreepTypes type)
        {
            if (m_type != type)
            {
                Creep newCreep = m_spriteHandler.UpdateCharacterAndSprite(type, m_creepCount - 1);
                newCreep.explosionHandler.TriggerExplosion(m_creepCount, type);

                SCreepConvertedInfo eventData = new SCreepConvertedInfo() {
                    creepCount = 1,
                    oldCreepType = m_type,
                    newCreepType = type
                };
                EventDispatcher<SCreepConvertedInfo>.Dispatch(CustomEvents.CreepConverted, eventData);
                return;
            }

            m_explosionHandler.TriggerExplosion(m_creepCount, type);
        }

        public void SelfExplode()
        {
            if (m_creepCount <= 1)
            {
                return;
            }

            while(m_mergedCreepsParent.childCount > 0)
            {
                Transform child = m_mergedCreepsParent.GetChild(0);
                Creep childCreep = child.GetComponent<Creep>();
                child.parent = transform.parent;
                child.position = transform.position;
                child.gameObject.SetActive(true);
                childCreep.ExplodeMove();
            }

            SCreepsExplodeInfo eventData = new SCreepsExplodeInfo() {
                creepCount = m_creepCount,
                creepType = m_type
            };
            EventDispatcher<SCreepsExplodeInfo>.Dispatch(CustomEvents.CreepsExploded, eventData);

            m_creepCount = 1;
            UpdateSprite();
            ExplodeMove();
        }

        public void ExplodeMove()
        {
            m_movementController.isExploding = true;
            // Play Explode anim
            m_movementController.RandomNavigate();
            m_movementController.isExploding = false;
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
            m_isSelected = false;
            m_movementController.isSelected = false;
        }

        public void DoMerge(int creepCount)
        {
            m_creepCount = creepCount;
            UpdateSprite();
            // Swap sprite and play merge anim
            m_isSelected = false;
            m_movementController.isSelected = false;
        }
    }
}
