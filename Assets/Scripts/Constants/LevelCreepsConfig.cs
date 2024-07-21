using UnityEngine;
using WTF.global;

namespace WTF.Configs
{
    public class LevelCreepsConfig
    {
        public static readonly int LevelStartPlayerCreepsCount = 5;
        public static readonly int LevelStartEnemyCreepsCount = 5;
        public static readonly int TotalCreepsCount = 15;

        public static readonly int MaxCreepGrouping = 5;

        public static readonly Vector2 EnemyCreepBurstDelay = new Vector2(5, 10);
    }
}
