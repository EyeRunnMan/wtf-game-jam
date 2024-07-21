using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using WTF.Common.InputSystem;
using WTF.Configs;
using WTF.Events;
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

            if (m_selectionType != selectedCreep.creepType)
            {
                selectedCreep.DeselectCreep(true);
                return;
            }

            m_creepsSelected.Add(selectedCreep);
        }

        private void OnSwipeEnd()
        {
            StartMerge().Wait();
        }

        private async Task StartMerge()
        {
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
                moveTasks.Add(m_creepsSelected[i].NavigateAndHide(lastCreep.transform));
            }

            await Task.WhenAll(moveTasks.ToArray());

            lastCreep.DoMerge(m_creepsSelected.Count);
            m_creepsSelected.Clear();
        }
    }
}
