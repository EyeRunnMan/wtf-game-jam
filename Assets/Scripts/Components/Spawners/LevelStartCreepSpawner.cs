using UnityEngine;
using NavMeshPlus.Components;
using WTF.Configs;
using WTF.Events;
using WTF.Helpers;
using WTF.Models;
using WTF.Players;

namespace WTF.GameControls
{
    public class LevelStartCreepSpawner : MonoBehaviour
        {
            [SerializeField] private Creep m_playerCreeps;
            [SerializeField] private Creep m_enemyCreeps;
            [SerializeField] private Transform m_creepParentObject;
            [SerializeField] private NavMeshSurface m_meshSurface;

            private bool m_creepsSpawned = false;

            public Transform spawnParent
            {
                set { m_creepParentObject = value; }
            }

            public NavMeshSurface meshSurface
            {
                set { m_meshSurface = value; }
            }

            public void SpawnCreeps()
            {
                if (m_creepsSpawned)
                {
                    Debug.LogWarning("Creeps already spawned!");
                    return;
                }

                CreateAndPlaceCreeps(m_playerCreeps, LevelCreepsConfig.LevelStartPlayerCreepsCount);
                CreateAndPlaceCreeps(m_enemyCreeps, LevelCreepsConfig.LevelStartEnemyCreepsCount);
                m_creepsSpawned = true;
            }

            private void CreateAndPlaceCreeps(Creep creep, int count)
            {
                if (!CreepsTracker.GetInstance().CanSpawnMoreCreeps(count))
                {
                    return;
                }

                Vector3 boundsMin = m_meshSurface.navMeshData.sourceBounds.min;
                Vector3 boundsMax = m_meshSurface.navMeshData.sourceBounds.max;
                for (int i = 0; i < count; ++i)
                {
                    float x = Random.Range(boundsMin.x, boundsMax.x);
                    float y = Random.Range(boundsMin.y, boundsMax.y);
                    Creep spawnedCreep = Instantiate(creep, new Vector3(x, y, 0), Quaternion.identity);
                    spawnedCreep.transform.parent = m_creepParentObject;
                }

                SCreepSpawnInfo eventData = new SCreepSpawnInfo() {
                    creepCount = count,
                    creepType = creep.creepType
                };
                EventDispatcher<SCreepSpawnInfo>.Dispatch(CustomEvents.CreepSpawned, eventData);
            }
        }
}
