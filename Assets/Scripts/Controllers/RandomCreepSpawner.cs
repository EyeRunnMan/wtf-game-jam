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

        private bool m_canSpawn;
        private float m_timer;
        private float m_nextSpawnTime;

        private void OnEnable()
        {
            m_canSpawn = false;
            m_timer = 0;
            m_nextSpawnTime = Random.Range(m_waitTime.x, m_waitTime.y);

            EventDispatcher<bool>.Register(CustomEvents.GameStart, OnGameStart);
            EventDispatcher<bool>.Register(CustomEvents.GameEnd, OnGameEnd);
        }

        private void Update()
        {
            if (!m_canSpawn)
            {
                return;
            }

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

        private void OnDisable()
        {
            EventDispatcher<bool>.Unregister(CustomEvents.GameStart, OnGameStart);
            EventDispatcher<bool>.Unregister(CustomEvents.GameEnd, OnGameEnd);
        }

        private void OnTriggerEnter(Collider other)
        {
            var creep = other.GetComponent<Creep>();

            if (creep)
            {
                m_timer = 0;
            }
        }

        private void OnGameStart(bool _)
        {
            m_canSpawn = true;
        }

        private void OnGameEnd(bool _)
        {
            m_canSpawn = false;
        }
    }
}
