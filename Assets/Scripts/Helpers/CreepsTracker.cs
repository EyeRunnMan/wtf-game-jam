using WTF.Configs;
using WTF.Events;
using UnityEngine;

namespace WTF.Helpers
{
    public class CreepsTracker
    {
        private static CreepsTracker Instance;
        private int m_noOfCreeps = 0;

        public static CreepsTracker GetInstance()
        {
            if (Instance == null)
            {
                Instance = new CreepsTracker();
            }

            return Instance;
        }

        public bool CanSpawnMoreCreeps()
        {
            return m_noOfCreeps < LevelCreepsConfig.TotalCreepsCount;
        }

        private CreepsTracker()
        {
            m_noOfCreeps = 0;
            EventDispatcher<int>.Register(CustomEvents.CreepSpawned, OnCreepSpawned);
        }

        private void OnCreepSpawned(int creepsSpawned)
        {
            m_noOfCreeps += creepsSpawned;
        }
    }
}
