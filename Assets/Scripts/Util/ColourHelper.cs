using UnityEngine;

namespace TDDemo.Assets.Scripts.Util
{
    public class ColourHelper
    {
        public static Color FullOpacity => Color.white;

        public static Color HalfOpacity => new(1, 1, 1, 0.5f);

        public static Color ZeroOpacity => new(1, 1, 1, 0f);

        public static Color SlowEffect => new(0, 127, 255);

        public static Color ParalyseEffect => Color.yellow;

        public static Color PoisonEffect => Color.green;

        public static Color Xp => Color.cyan;

        public static Color XpLoss => Color.blue;

        public static Color Coupon => new(255, 191, 171);

        public static Color DefaultWave => new(1, 165 / 255f, 0);

        public static Color BossWave => new(169 / 255f, 0, 1);

        public static Color NoWave => Color.grey;
    }
}
