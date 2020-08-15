using UnityEngine;

namespace Assets.Scripts.Util
{
    /// <summary>
    /// Helper methods for colours.
    /// </summary>
    public class ColourHelper
    {
        /// <summary>
        /// Gets a colour for rendering a sprite at full opacity.
        /// </summary>
        public static Color FullOpacity => Color.white;

        /// <summary>
        /// Gets a colour for rendering a sprite at half opacity.
        /// </summary>
        public static Color HalfOpacity => new Color(1, 1, 1, 0.5f);
    }
}
