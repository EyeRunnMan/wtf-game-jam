using System.Collections.Generic;
using UnityEngine;
using WTF.Configs;
using WTF.Events;
using WTF.Models;

namespace WTF.Helpers
{
    public class CreepsTracker
    {
        private static CreepsTracker Instance;
        private int m_noOfCreeps = 0;
        private Dictionary<CreepTypes, int> m_creepTypeMap = new Dictionary<CreepTypes, int>();

        public static CreepsTracker GetInstance()
        {
            if (Instance == null)
            {
                Instance = new CreepsTracker();
            }

            return Instance;
        }

        public bool CanSpawnMoreCreeps(int count)
        {
            return (m_noOfCreeps + count) <= LevelCreepsConfig.TotalCreepsCount;
        }

        private CreepsTracker()
        {
            m_noOfCreeps = 0;
            EventDispatcher<SCreepSpawnInfo>.Register(CustomEvents.CreepSpawned, OnCreepSpawned);
        }

        private void OnCreepSpawned(SCreepSpawnInfo creepsInfo)
        {
            m_noOfCreeps += creepsInfo.creepCount;

            if (!m_creepTypeMap.ContainsKey(creepsInfo.creepType))
            {
                m_creepTypeMap[creepsInfo.creepType] = 0;
            }

            m_creepTypeMap[creepsInfo.creepType] += creepsInfo.creepCount;
        }
    }
}
