using UnityEngine;

namespace WTF.Bot
{
    public class BotConfig
    {
        public static readonly Vector2 WaitBeforeMerge = new Vector2(15, 60);
        public static readonly Vector2 WaitBeforeExplosion = new Vector2(5, 30);

        public static readonly float[] WeightsForMerge = {30, 70, 90, 100};
    }
}
