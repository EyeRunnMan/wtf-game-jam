using UnityEngine;
using WTF.Common;
using WTF.Configs;
using WTF.Events;
using WTF.Helpers;
using WTF.Models;
using WTF.Players;

namespace WTF.GameControls
{
    public class RandomCreepSpawner : MonoBehaviour
    {
        [SerializeField] private CreepTypes[] m_creepTypesToSpawn;
        [SerializeField] private Vector2 m_waitTime;
        [SerializeField] private Transform m_creepParentObject;

        private bool m_canSpawn;
        private float m_timer;
        private float m_nextSpawnTime;
        private ISpawnerFactory m_factory;

        public Transform spawnParent
        {
            set { m_creepParentObject = value; }
        }

        public bool startSpawning
        {
            set { m_canSpawn = value; }
        }

        private void Start()
        {
            DependencySolver.TryGetInstance(out m_factory);
        }

        private void OnEnable()
        {
            m_canSpawn = false;
            m_timer = 0;
            m_nextSpawnTime = Random.Range(m_waitTime.x, m_waitTime.y);

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
                CreepTypes randCreepType = m_creepTypesToSpawn[Random.Range(0, m_creepTypesToSpawn.Length)];
                Creep spawnedCreep = m_factory.CreateCreep(randCreepType);
                spawnedCreep.transform.position = transform.position;
                spawnedCreep.transform.rotation = Quaternion.identity;
                spawnedCreep.transform.parent = m_creepParentObject;
                m_timer = 0;
                m_nextSpawnTime = Random.Range(m_waitTime.x, m_waitTime.y);
                SCreepSpawnInfo eventData = new SCreepSpawnInfo() {
                    creepCount = 1,
                    creepType = randCreepType
                };
                EventDispatcher<SCreepSpawnInfo>.Dispatch(CustomEvents.CreepSpawned, eventData);
            }
        }

        private void OnDisable()
        {
            EventDispatcher<bool>.Unregister(CustomEvents.GameEnd, OnGameEnd);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var creep = other.GetComponent<Creep>();

            if (creep)
            {
                m_timer = 0;
            }
        }

        private void OnGameEnd(bool _)
        {
            m_canSpawn = false;
        }
    }
}
