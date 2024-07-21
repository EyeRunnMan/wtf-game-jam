using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using WTF.Common.InputSystem;
using WTF.Configs;
using WTF.Events;
using WTF.Helpers;
using WTF.Models;
using WTF.Players;

namespace WTF.PlayerControls
{
    public class CreepsGroupManager : MonoBehaviour
    {
        private InputSystem m_inputSystem;
        private List<Creep> m_creepsSelected;
        private CreepTypes m_selectionType;

        private void OnEnable()
        {
            m_creepsSelected = new List<Creep>();
            m_inputSystem = InputSystem.GetInstance();

            EventDispatcher<Creep>.Register(CustomEvents.CreepSelected, OnCreepSelected);
            m_inputSystem.OnSwipeEventEnded += OnSwipeEnd;
        }

        private void OnDisable()
        {
            if (m_inputSystem == null)
            {
                return;
            }

            EventDispatcher<Creep>.Unregister(CustomEvents.CreepSelected, OnCreepSelected);
            m_inputSystem.OnSwipeEventEnded -= OnSwipeEnd;
        }

        private void OnCreepSelected(Creep selectedCreep)
        {
            if (m_creepsSelected.Count == 0)
            {
                m_selectionType = selectedCreep.creepType;
            }

            bool outOfMoves = !MovesTracker.GetInstance().CanMakeMove(m_selectionType, m_creepsSelected.Count + 1);
            if (m_selectionType != selectedCreep.creepType ||
                outOfMoves)
            {
                selectedCreep.DeselectCreep(true);
                if (outOfMoves)
                {
                    EventDispatcher<bool>.Dispatch(CustomEvents.OutOfMoves, true);
                }
                return;
            }

            m_creepsSelected.Add(selectedCreep);
        }

        private void OnSwipeEnd()
        {
            StartMerge();
        }

        private async Task StartMerge()
        {
            var filteredList = new List<Creep>();
            foreach (var creep in m_creepsSelected)
            {
                if (creep.IsOnNavMesh())
                {
                    filteredList.Add(creep);
                }
                else
                {
                    creep.DeselectCreep();
                }
            }
            m_creepsSelected = filteredList;
            if (m_creepsSelected.Count == 0)
            {
                return;
            }

            if (m_creepsSelected.Count == 1)
            {
                m_creepsSelected[0].DeselectCreep();
                m_creepsSelected.Clear();
                return;
            }

            Creep lastCreep = m_creepsSelected[m_creepsSelected.Count - 1];
            List<Task> moveTasks = new List<Task>();
            for (int i = 0; i < m_creepsSelected.Count - 1; ++i)
            {
                moveTasks.Add(m_creepsSelected[i].NavigateAndHide(lastCreep));
            }

            await Task.WhenAll(moveTasks.ToArray());

            lastCreep.DoMerge(m_creepsSelected.Count);
            foreach (var item in m_creepsSelected)
            {
                item.DeselectCreep();
            }
            SCreepsGroupInfo eventData = new SCreepsGroupInfo()
            {
                creepCount = m_creepsSelected.Count,
                creepType = m_selectionType
            };
            EventDispatcher<SCreepsGroupInfo>.Dispatch(CustomEvents.CreepsGrouped, eventData);
            m_creepsSelected.Clear();
        }
    }
}
