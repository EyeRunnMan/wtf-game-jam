using System;
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
            EventDispatcher<SCreepConvertedInfo>.Register(CustomEvents.CreepConverted, OnCreepConverted);
        }

        private void OnCreepSpawned(SCreepSpawnInfo creepsInfo)
        {
            m_noOfCreeps += creepsInfo.creepCount;

            if (!m_creepTypeMap.ContainsKey(creepsInfo.creepType))
            {
                m_creepTypeMap[creepsInfo.creepType] = 0;
            }

            m_creepTypeMap[creepsInfo.creepType] += creepsInfo.creepCount;

            CheckEndCondition();
        }

        private void OnCreepConverted(SCreepConvertedInfo creepsInfo)
        {
            if (!m_creepTypeMap.ContainsKey(creepsInfo.newCreepType))
            {
                m_creepTypeMap[creepsInfo.newCreepType] = 0;
            }

            m_creepTypeMap[creepsInfo.newCreepType] += creepsInfo.creepCount;

            if (!m_creepTypeMap.ContainsKey(creepsInfo.oldCreepType))
            {
                m_creepTypeMap[creepsInfo.oldCreepType] = 0;
                return;
            }

            m_creepTypeMap[creepsInfo.oldCreepType] -= creepsInfo.creepCount;
            if (m_creepTypeMap[creepsInfo.oldCreepType] < 0)
            {
                m_creepTypeMap[creepsInfo.oldCreepType] = 0;
            }

            CheckEndCondition();
        }

        private void CheckEndCondition()
        {
            bool gameEnd = false;
            Nullable<CreepTypes> loser = null;
            foreach(KeyValuePair<CreepTypes, int> entry in m_creepTypeMap)
            {
                if (entry.Value == 0)
                {
                    gameEnd = true;
                    loser = entry.Key;
                    break;
                }
            }

            if (gameEnd)
            {
                EventDispatcher<bool>.Dispatch(CustomEvents.GameEnd, true);
                if (loser == CreepTypes.Player)
                {
                    EventDispatcher<bool>.Dispatch(CustomEvents.GameLose, true);
                    Debug.Log("Game Lose!!");
                }
                else
                {
                    EventDispatcher<bool>.Dispatch(CustomEvents.GameWin, true);
                    Debug.Log("Game Win!!");
                }
            }
        }
    }
}
