using UnityEngine;
using WTF.Common;
using WTF.Configs;
using WTF.Players;

namespace WTF.Helpers
{
    public class SpawnerFactory: MonoBehaviour, ISpawnerFactory
    {
        [SerializeField] private Creep m_goodCreepPrefab;
        [SerializeField] private Creep m_badCreepPrefab;

        private void Awake()
        {
            DependencySolver.RegisterInstance(this as ISpawnerFactory);
        }

        private void OnDestroy()
        {
            DependencySolver.RemoveInstance(this as ISpawnerFactory);
        }

        public Creep CreateCreep(CreepTypes type)
        {
            switch(type)
            {
                case CreepTypes.Player:
                    return CreateInstance(m_goodCreepPrefab);
                case CreepTypes.Enemy:
                    return CreateInstance(m_badCreepPrefab);
                default:
                    Debug.LogWarning("Trying to create an unknown creep type");
                    return null;
            }
        }

        private Creep CreateInstance(Creep prefab)
        {
            return Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }
    }
}
