using System;

namespace TDDemo.Assets.Scripts.Towers.Experience
{
    public class PokemonExperienceCurve : IExperienceCurve
    {
        public int GetRequiredXpForLevel(int level)
        {
            // adapted from http://howtomakeanrpg.com/a/how-to-make-an-rpg-levels.html
            return Round(4 * Math.Pow(level, 3) / 5);
        }

        private static int Round(double d)
        {
            // adapted from http://howtomakeanrpg.com/a/how-to-make-an-rpg-levels.html
            return d < 0 ? (int) Math.Ceiling(d - 0.5) : (int) Math.Floor(d + 0.5);
        }
    }
}
