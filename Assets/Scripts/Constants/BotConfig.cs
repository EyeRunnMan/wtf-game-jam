using UnityEngine;

namespace WTF.Bot
{
    public class BotConfig
    {
        public static readonly Vector2 WaitBeforeMerge = new Vector2(15, 35);
        public static readonly Vector2 WaitBeforeExplosion = new Vector2(5, 15);

        public static readonly float[] WeightsForMerge = {30, 70, 90, 100};
    }
}
