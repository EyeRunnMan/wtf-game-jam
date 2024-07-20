using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace WTF.PlayerControls
{
    public class RandomMoveController : MonoBehaviour
    {
        [SerializeField] private float m_moveRadius;
        [SerializeField] private float m_waitTime;

        private float m_timer;
        private NavMeshAgent m_agent;
        private bool m_isSelected;

        public bool isSelected
        {
            set { m_isSelected = value; }
        }

        private void OnEnable()
        {
            m_isSelected = false;
            m_timer = m_waitTime;

            m_agent = GetComponent<NavMeshAgent>();
        }

        private void Update() {
            if (m_isSelected)
            {
                return;
            }

            m_timer += Time.deltaTime;

            if (m_timer >= m_waitTime) {
                Vector3 newPos = GetRandomNavSphere();
                m_agent.SetDestination(newPos);
                m_timer = 0;
            }
        }

        private Vector3 GetRandomNavSphere()
        {
            Vector3 randDirection = Random.insideUnitSphere * m_moveRadius;
            randDirection += transform.position;
            randDirection.z = 0;

            NavMeshHit navHit;
            NavMesh.SamplePosition(randDirection, out navHit, m_moveRadius, -1);

            return navHit.position;
        }
    }
}
