using UnityEngine;

namespace TDDemo.Assets.Scripts.Util
{
    public class ColourHelper
    {
        public static Color FullOpacity => Color.white;

        public static Color HalfOpacity => new(1, 1, 1, 0.5f);

        public static Color SlowEffect => new(0, 127, 255);

        public static Color ParalyseEffect => Color.yellow;

        public static Color PoisonEffect => Color.green;

        public static Color Xp => Color.cyan;

        public static Color XpLoss => Color.blue;
    }
}
