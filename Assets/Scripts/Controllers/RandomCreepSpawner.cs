using UnityEngine;
using WTF.Events;
using WTF.Helpers;
using WTF.Players;

namespace WTF.GameControls
{
    public class RandomCreepSpawner : MonoBehaviour
    {
        [SerializeField] private Creep[] m_creepsToSpawn;
        [SerializeField] private Vector2 m_waitTime;
        [SerializeField] private Transform m_creepParentObject;

        private float m_timer;
        private float m_nextSpawnTime;

        private void OnEnable()
        {
            m_timer = 0;
            m_nextSpawnTime = Random.Range(m_waitTime.x, m_waitTime.y);
        }

        private void Update()
        {
            m_timer += Time.deltaTime;

            if (m_timer >= m_nextSpawnTime && CreepsTracker.GetInstance().CanSpawnMoreCreeps(1)) {
                Creep randCreepPrefab = m_creepsToSpawn[Random.Range(0, m_creepsToSpawn.Length)];
                Creep spawnedCreep = Instantiate(randCreepPrefab, transform.position, Quaternion.identity);
                spawnedCreep.transform.parent = m_creepParentObject;
                m_timer = 0;
                m_nextSpawnTime = Random.Range(m_waitTime.x, m_waitTime.y);
                EventDispatcher<int>.Dispatch(CustomEvents.CreepSpawned, 1);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var creep = other.GetComponent<Creep>();

            if (creep)
            {
                m_timer = 0;
            }
        }
    }
}
