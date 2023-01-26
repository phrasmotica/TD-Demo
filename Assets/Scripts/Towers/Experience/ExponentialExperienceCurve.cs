using System;

namespace TDDemo.Assets.Scripts.Towers.Experience
{
    public class ExponentialExperienceCurve : IExperienceCurve
    {
        private readonly int _baseXp;

        private readonly int _coefficient;

        public ExponentialExperienceCurve(int baseXp, int coefficient)
        {
            _baseXp = baseXp;
            _coefficient = coefficient;
        }

        public int GetRequiredXpForLevel(int level)
        {
            // adapted from https://www.davideaversa.it/blog/gamedesign-math-rpg-level-based-progression/
            return (int) (_baseXp * (1 - Math.Pow(_coefficient, level)) / (1 - _coefficient));
        }
    }
}
