using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WTF.Configs;
using WTF.Events;
using WTF.Helpers;
using WTF.Models;
using WTF.Players;

namespace WTF.Bot
{
    public class Grimm : MonoBehaviour
    {
        private Transform m_creepsParent;
        private HashSet<Creep> m_creeps = new HashSet<Creep>();
        private List<Creep> m_groupedCreeps = new List<Creep>();

        private void OnEnable()
        {
            EventDispatcher<bool>.Register(CustomEvents.GameEnd, OnGameEnd);
        }

        private void OnDisable()
        {
            EventDispatcher<bool>.Unregister(CustomEvents.GameEnd, OnGameEnd);
        }

        private void OnGameEnd(bool _)
        {
            StopAllCoroutines();
        }

        public Transform spawnParent
        {
            set { m_creepsParent = value; }
        }

        public void StartPlaying()
        {
            StartCoroutine(AttemptMerge());
            StartCoroutine(AttemptExplosion());
        }

        private IEnumerator AttemptMerge()
        {
            yield return new WaitForSeconds(BotConfig.WaitBeforeMerge.x);
            while(MovesTracker.GetInstance().CanMakeMove(CreepTypes.Enemy, 2))
            {
                float nextWaitTime = Random.Range(BotConfig.WaitBeforeMerge.x, BotConfig.WaitBeforeMerge.y);
                Creep[] activeCreeps = m_creepsParent.GetComponentsInChildren<Creep>(false);
                m_creeps.UnionWith(activeCreeps.Where(c => c.creepType == CreepTypes.Enemy));

                int mergeCount = RollDiceForMerge();

                if (mergeCount == -1)
                {
                    yield return new WaitForSeconds(nextWaitTime);
                    continue;
                }

                List<Creep> nextCreeps = FindCloserCreeps(m_creeps.ToArray(), mergeCount);
                Creep lastCreep = nextCreeps[nextCreeps.Count - 1];
                for (int i = 0; i < nextCreeps.Count - 1; ++i)
                {
                    nextCreeps[i].BOT_SelectCreep();
                    nextCreeps[i].NavigateAndHide(lastCreep);
                    m_creeps.Remove(nextCreeps[i]);
                }
                lastCreep.BOT_SelectCreep();
                lastCreep.DoMerge(nextCreeps.Count);
                m_groupedCreeps.Add(lastCreep);

                SCreepsGroupInfo eventData = new SCreepsGroupInfo()
                {
                    creepCount = nextCreeps.Count,
                    creepType = CreepTypes.Enemy
                };
                EventDispatcher<SCreepsGroupInfo>.Dispatch(CustomEvents.CreepsGrouped, eventData);

                yield return new WaitForSeconds(nextWaitTime);
            }
        }

        private IEnumerator AttemptExplosion()
        {
            yield return new WaitForSeconds(BotConfig.WaitBeforeExplosion.y);
            while(m_groupedCreeps.Count > 0)
            {
                float nextWaitTime = Random.Range(BotConfig.WaitBeforeExplosion.x, BotConfig.WaitBeforeExplosion.y);

                Creep nextCreep = m_groupedCreeps[Random.Range(0, m_groupedCreeps.Count)];
                nextCreep.InitiateExplosion(CreepTypes.Enemy);

                m_groupedCreeps.Remove(nextCreep);

                yield return new WaitForSeconds(nextWaitTime);
            }
        }

        private int RollDiceForMerge()
        {
            int random = Random.Range(0, 101);
            int mergeCount = -1;
            for (int i = 0; i < BotConfig.WeightsForMerge.Length; ++i)
            {
                if (random <=  BotConfig.WeightsForMerge[i])
                {
                    mergeCount = i + 2;
                    break;
                }
            }

            while(mergeCount > 0 && !MovesTracker.GetInstance().CanMakeMove(CreepTypes.Enemy, mergeCount))
            {
                mergeCount -= 1;
            }

            return mergeCount < 2 ? -1 : mergeCount;
        }

        private List<Creep> FindCloserCreeps(Creep[] creeps, int count)
        {
            var closestCreeps = new SortedSet<(float distance, Creep c)>(new CreepComparer());
            for (int i = 0; i < creeps.Length; ++i)
            {
                for (int j = i + 1; j < creeps.Length; ++j)
                {
                    float distance = Vector2.Distance(creeps[i].transform.position, creeps[j].transform.position);
                    closestCreeps.Add((distance, creeps[i]));
                    closestCreeps.Add((distance, creeps[j]));

                    if (closestCreeps.Count > count)
                    {
                        closestCreeps.Remove(closestCreeps.Max);
                    }
                }
            }

            var result = new List<Creep>();
            foreach (var (_, creep) in closestCreeps)
            {
                result.Add(creep);
                if (result.Count == count)
                    break;
            }

            return result;
        }


        private class CreepComparer : IComparer<(float distance, Creep c)>
        {
            public int Compare((float distance, Creep c) x, (float distance, Creep c) y)
            {
                return x.distance.CompareTo(y.distance);
            }
        }
    }
}
