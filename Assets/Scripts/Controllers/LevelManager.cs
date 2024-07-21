using UnityEngine;
using NavMeshPlus.Components;
using WTF.Events;

namespace WTF.GameControls
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform m_creepParentObject;
        [SerializeField] private NavMeshSurface m_meshSurface;
        [SerializeField] private GameObject[] m_levels;

        private GameObject m_currentLevel;

        private void Start()
        {
            InstantiateLevel();
            ConfigureSpawners();
            StartGame();
        }

        private void InstantiateLevel()
        {
            GameObject randLevel = m_levels[Random.Range(0, m_levels.Length)];
            m_currentLevel = Instantiate(randLevel, transform.position, Quaternion.identity);
            m_currentLevel.transform.parent = transform;

            m_meshSurface.BuildNavMesh();
        }

        private void ConfigureSpawners()
        {
            LevelStartCreepSpawner[] levelSpawners = GetComponentsInChildren<LevelStartCreepSpawner>(true);

            for (int i = 0; i < levelSpawners.Length; ++i)
            {
                levelSpawners[i].spawnParent = m_creepParentObject;
                levelSpawners[i].meshSurface = m_meshSurface;
                levelSpawners[i].SpawnCreeps();
            }

            RandomCreepSpawner[] randomSpawners = GetComponentsInChildren<RandomCreepSpawner>(true);

            for (int i = 0; i < randomSpawners.Length; ++i)
            {
                randomSpawners[i].spawnParent = m_creepParentObject;
                randomSpawners[i].startSpawning = true;
            }
        }

        private void StartGame()
        {
            EventDispatcher<bool>.Dispatch(CustomEvents.GameStart, true);
        }
    }
}
