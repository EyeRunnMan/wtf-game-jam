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
        private int m_noOfMoves = 0;

        public static MovesTracker GetInstance()
        {
            if (Instance == null)
            {
                Instance = new MovesTracker();
            }

            return Instance;
        }

        public bool CanMakeMove(int count)
        {
            return (m_noOfMoves - count) >= 0;
        }

        private MovesTracker()
        {
            m_noOfMoves = LevelCreepsConfig.MaxCreepGrouping;
            EventDispatcher<int>.Register(CustomEvents.CreepsGrouped, OnCreepsGrouped);
        }

        private void OnCreepsGrouped(int creepCount)
        {
            m_noOfMoves -= creepCount;
        }
    }
}
