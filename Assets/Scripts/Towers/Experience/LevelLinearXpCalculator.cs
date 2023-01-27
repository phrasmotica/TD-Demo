namespace TDDemo.Assets.Scripts.Towers.Experience
{
    public class LevelLinearXpCalculator : IXpCalculator
    {
        public int Compute(Tower tower, int baseXp) => tower.Level * baseXp;
    }
}
