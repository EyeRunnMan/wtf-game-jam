using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WTF.Configs;
using WTF.Events;
using WTF.Models;

namespace WTF.Helpers
{
    public class MovesTracker
    {
        private static MovesTracker Instance;
        private Dictionary<CreepTypes, int> m_noOfMoves = new Dictionary<CreepTypes, int>();

        public static MovesTracker GetInstance()
        {
            if (Instance == null)
            {
                Instance = new MovesTracker();
            }

            return Instance;
        }

        public bool CanMakeMove(CreepTypes type, int count)
        {
            if (!m_noOfMoves.ContainsKey(type))
            {
                m_noOfMoves[type] = LevelCreepsConfig.MaxCreepGrouping;
            }

            return (m_noOfMoves[type] - count) >= 0;
        }

        private MovesTracker()
        {
            EventDispatcher<SCreepsGroupInfo>.Register(CustomEvents.CreepsGrouped, OnCreepsGrouped);
        }

        private void OnCreepsGrouped(SCreepsGroupInfo creepsInfo)
        {
            if (!m_noOfMoves.ContainsKey(creepsInfo.creepType))
            {
                m_noOfMoves[creepsInfo.creepType] = LevelCreepsConfig.MaxCreepGrouping;
            }

            m_noOfMoves[creepsInfo.creepType] -= creepsInfo.creepCount;
        }
    }
}
