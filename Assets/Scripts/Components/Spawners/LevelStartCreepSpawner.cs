using UnityEngine;
using NavMeshPlus.Components;
using WTF.Common;
using WTF.Configs;
using WTF.Events;
using WTF.Helpers;
using WTF.Models;
using WTF.Players;

namespace WTF.GameControls
{
    public class LevelStartCreepSpawner : MonoBehaviour
        {
            [SerializeField] private Transform m_creepParentObject;
            [SerializeField] private NavMeshSurface m_meshSurface;

            private bool m_creepsSpawned = false;
            private ISpawnerFactory m_factory;

            private void Awake()
            {
                DependencySolver.TryGetInstance(out m_factory);
            }

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

                CreateAndPlaceCreeps(CreepTypes.Player, LevelCreepsConfig.LevelStartPlayerCreepsCount);
                CreateAndPlaceCreeps(CreepTypes.Enemy, LevelCreepsConfig.LevelStartEnemyCreepsCount);
                m_creepsSpawned = true;
            }

            private void CreateAndPlaceCreeps(CreepTypes type, int count)
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
                    Creep spawnedCreep = m_factory.CreateCreep(type);
                    spawnedCreep.transform.position = new Vector3(x, y, 0);
                    spawnedCreep.transform.rotation = Quaternion.identity;
                    spawnedCreep.transform.parent = m_creepParentObject;
                }

                SCreepSpawnInfo eventData = new SCreepSpawnInfo() {
                    creepCount = count,
                    creepType = type
                };
                EventDispatcher<SCreepSpawnInfo>.Dispatch(CustomEvents.CreepSpawned, eventData);
            }
        }
}
