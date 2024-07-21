using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace WTF.PlayerControls
{
    public class CreepMovementController : MonoBehaviour
    {
        private const int V = 1;
        [SerializeField] private float m_moveRadius;
        [SerializeField] private Vector2 m_waitTime;

        private float m_timer;
        private float m_nextSpawnTime;
        private NavMeshAgent m_agent;
        private bool m_isSelected;

        public bool isSelected
        {
            set { m_isSelected = value; }
        }

        private void OnEnable()
        {
            m_isSelected = false;
            m_nextSpawnTime = Random.Range(m_waitTime.x, m_waitTime.y);
            m_timer = m_nextSpawnTime;

            m_agent = GetComponent<NavMeshAgent>();
            m_agent.updateRotation = false;
        }

        private void Update()
        {
            if (m_isSelected)
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
                Vector3 newPos = GetRandomNavSphere();
                m_agent.SetDestination(newPos);
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

        public async Task NavigateToDestination(Vector3 position)
        {
            m_agent.SetDestination(position);
            await Task.Delay(500);

            while (m_agent.remainingDistance > V)
            {
                await Task.Delay(500);
            }
        }
    }
}
