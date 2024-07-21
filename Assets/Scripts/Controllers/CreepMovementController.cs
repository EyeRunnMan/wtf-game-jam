using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace WTF.PlayerControls
{
    public class CreepMovementController : MonoBehaviour
    {
        [SerializeField] private float m_moveRadius;
        [SerializeField] private Vector2 m_waitTime;

        private float m_timer;
        private float m_nextSpawnTime;
        private NavMeshAgent m_agent;
        private bool m_isSelected;
        private bool m_isExploding;

        public bool isSelected
        {
            set { m_isSelected = value; }
        }

        public bool isExploding
        {
            set { m_isExploding = value; }
        }

        private void OnEnable()
        {
            m_isExploding = false;
            m_isSelected = false;
            m_nextSpawnTime = Random.Range(m_waitTime.x, m_waitTime.y);
            m_timer = m_nextSpawnTime;

            m_agent = GetComponent<NavMeshAgent>();
            m_agent.updateRotation = false;
        }

        private void Update()
        {
            if (m_isSelected || m_isExploding)
            {
                if (m_agent.pathStatus != NavMeshPathStatus.PathComplete && m_agent.remainingDistance > 0)
                {
                    m_agent.ResetPath();
                }
                return;
            }

            m_timer += Time.deltaTime;

            if (m_timer >= m_nextSpawnTime)
            {
                RandomNavigate();
                m_timer = 0;
                m_nextSpawnTime = Random.Range(m_waitTime.x, m_waitTime.y);
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

        public void RandomNavigate()
        {
            Vector3 newPos = GetRandomNavSphere();
            m_agent.SetDestination(newPos);
        }

        public async Task NavigateToDestination(Vector3 position)
        {
            m_agent.SetDestination(position);

            while(m_agent.pathStatus != NavMeshPathStatus.PathComplete && m_agent.remainingDistance > 0)
            {
                await Task.Delay(500);
            }
        }
    }
}
